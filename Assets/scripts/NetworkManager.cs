using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NetworkChannel { GAMEPLAY, PROTOCOL, OTHER };

public class NetworkManager : MonoBehaviour {
	public const int DEFAULT_MAX_CONNECTIONS = 10;
	public const int DEFAULT_PORT = 28965;

	public GameObject playerPrefab;
	public GameObject spectatorPrefab;

	public string currentLevelName;
	private int currentLevelIdent = 0;
	private int team = 0;
	public LazerTagGametype gametype;
	public IDictionary<NetworkPlayer, string> playerNames = new Dictionary<NetworkPlayer, string>();

	public enum PLAYER_TEAMS { GREEN, RED, SPECTATOR };
	private int[] teamCounts = {0,0,0};
	public IDictionary<NetworkPlayer, PLAYER_TEAMS> playerTeams = new Dictionary<NetworkPlayer, PLAYER_TEAMS>();

	public static NetworkManager instance;
	public static string playerName;
	public static string hostName;

	private static string useMap = "Ice_Cap";
	private static bool spectate;

	public void Awake() {
		instance = this;
		playerName = "Player" + Random.Range(100, 1000);
		hostName = playerName;
		networkView.group = (int)NetworkChannel.PROTOCOL;
	}

	public static void CreateServer(string mapName, bool spectate, int maxConnections = DEFAULT_MAX_CONNECTIONS, int port = DEFAULT_PORT) {
		useMap = mapName;
		NetworkManager.spectate = spectate;
		Network.InitializeSecurity();
		Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
		MenuManager.DisplayDialogBox("Starting up server...");
	}

	public static void ConnectToServer(string serverIP, bool spectate, int port = DEFAULT_PORT) {
		NetworkManager.spectate = spectate;
		Network.Connect(serverIP, port);
		MenuManager.DisplayDialogBox("Trying to connect...");
	}

	public static void DisconnectFromServer() {
		Network.Disconnect();
		GameObject[] objs = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in objs) {
			GameObject.Destroy(obj);
		}
		Application.LoadLevel("main_menu");
	}

	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		MenuManager.DisplayDialogBox("Lost connection to server!", "MainMenu");
	}

	public void OnPlayerDisconnected(NetworkPlayer player) {
		teamCounts[(int)playerTeams[player]]--;
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		networkView.RPC("SendTextMessageRPC", RPCMode.All, playerNames[player] + " left the game", Vector3.one);
		playerNames.Remove(player);
	}

	public static void SetNetworkChannel(NetworkChannel channel, bool enable) {
		Network.SetSendingEnabled((int)channel, enable);
	}

	public void OnFailedToConnect(NetworkConnectionError error) {
		MenuManager.DisplayDialogBox("Failed to connect to server!", "JoinMenu");
	}

	public void OnServerInitialized() {
		MenuManager.Open_Menu("IngameMenu");
		Application.LoadLevel(useMap);
		currentLevelName = useMap;
		currentLevelIdent++;
	}

	public void OnConnectedToServer() {
		MenuManager.Open_Menu("IngameMenu");
		MenuManager.DisplayDialogBox("Obtaining level info...");
		networkView.RPC("GetCurrentLevel", RPCMode.Server, NetworkManager.spectate);
	}

	public void OnLevelWasLoaded(int level) {
		if(level == 0) { return; }

		MenuManager.DisplayDialogBox(null);

		Network.isMessageQueueRunning = true;
		SetNetworkChannel(NetworkChannel.GAMEPLAY, true);
	
		if(Network.isServer) {
			team = getTeam(Network.player, NetworkManager.spectate);
			playerNames.Add(Network.player, playerName);
			SendTextMessageRPC(playerName + " joined the game", Vector3.one);
		} else {
			networkView.RPC("SetPlayerName", RPCMode.Server, playerName);
		}

		spawn(NetworkManager.spectate);
	}

	public void spawn(bool spectate) {
		GameObject[] sps = GameObject.FindGameObjectsWithTag((team%2 == 0) ? "SpawnPointGreen" : "SpawnPointRed");
		Transform sp = sps[Random.Range(0, sps.Length)].transform;

		if(spectate) {
			Instantiate(spectatorPrefab, sp.position + new Vector3(0, 5.0f, 0), sp.rotation);
		} else {
			GameObject player = Network.Instantiate(playerPrefab, sp.position + new Vector3(0, 1.2f, 0), sp.rotation, 0) as GameObject;
			player.name = "LocalPlayer";
			player.GetComponent<MouseLook>().enabled = true;
			((MonoBehaviour)player.GetComponent("FPSInputController")).enabled = true;
			player.transform.FindChild("Main Camera").gameObject.SetActive(true);
			player.GetComponent<PowerUp_Controler>().enabled = true;
			player.GetComponent<Player_loc>().enabled = true;
			player.GetComponent<Player_Controler>().enabled = true;
			player.GetComponent<Player_Network_Controller>().setTeam(team%2 == 0, playerName);
		}
	}

	public static void SetGametype(LazerTagGametype gametype) {
		instance.gametype = gametype;
	}

	public static void GametypeSend(string msg, string args) {
		Debug.Log("Sending Message: " + msg + "(" + args + ")");
		instance.networkView.RPC("GametypeMessage", RPCMode.AllBuffered, msg, args);
	}

	[RPC]
	public IEnumerator GametypeMessage(string msg, string args) {
		int times = 5;
		while(gametype == null && times != 0) {
			Debug.Log("Gametype not yet set... Waiting " + times + " more times!");
			yield return new WaitForSeconds(0.25f);
			times --;
		}
		if(gametype != null) {
			Debug.Log("Received Message: " + msg + "(" + args + ")");
			gametype.BroadcastMessage(msg, args, SendMessageOptions.DontRequireReceiver);
		}
	}

	public int getTeam(NetworkPlayer player, bool spectating) {
		PLAYER_TEAMS team = PLAYER_TEAMS.SPECTATOR;
		if(!spectating) {
			int redCount = teamCounts[(int)PLAYER_TEAMS.RED];
			int greenCount = teamCounts[(int)PLAYER_TEAMS.GREEN];
			if(redCount > greenCount) {
				team = PLAYER_TEAMS.GREEN;
			} else if(greenCount > redCount) {
				team = PLAYER_TEAMS.RED;
			} else {
				int random = Random.Range(0, 2);
				team = (random == 0) ? PLAYER_TEAMS.GREEN : PLAYER_TEAMS.RED;
			}
		}
		playerTeams[player] = team;
		teamCounts[(int)team]++;
		return (int)team;
	}

	[RPC]
	public void GetCurrentLevel(bool spectating, NetworkMessageInfo info) {
		int team = getTeam(info.sender, spectating);
		networkView.RPC("LoadNetworkLevel", info.sender, currentLevelName, currentLevelIdent, (int)team, hostName);
	}

	[RPC]
	public void LoadNetworkLevel(string levelName, int levelIdent, int team, string hostName) {
		MenuManager.DisplayDialogBox("Loading level...");

		SetNetworkChannel(NetworkChannel.GAMEPLAY, false);
		Network.isMessageQueueRunning = false;
		Network.SetLevelPrefix(levelIdent);

		GameObject[] objs = FindObjectsOfType<GameObject>();
		foreach(GameObject obj in objs) {
			if(obj.name != "Main Camera") {
				GameObject.DontDestroyOnLoad(obj);
			}
		}

		this.team = team;
		this.currentLevelName = levelName;
		NetworkManager.hostName = hostName;

		Application.LoadLevel(levelName);
	}

	[RPC]
	public void SetPlayerName(string name, NetworkMessageInfo info) {
		playerNames.Add(info.sender, name);
		if(Network.isServer) {
			networkView.RPC("SendTextMessageRPC", RPCMode.All, name + " joined the game", Vector3.one);
		}
	}

	public static void SendTextMessage(string msg, Color color) {
		instance.networkView.RPC("SendTextMessageRPC", RPCMode.All, msg, new Vector3(color.r, color.g, color.b));
	}

	[RPC]
	public void SendTextMessageRPC(string msg, Vector3 color) {
		object[] args = {msg, new Color(color.x, color.y, color.z, 1.0f)};
		MenuManager.instance.SendMessage("Message", args);
	}
}