using UnityEngine;
using System.Collections;

public enum NetworkChannel { GAMEPLAY, PROTOCOL, OTHER };

public class NetworkManager : MonoBehaviour {
	public const int DEFAULT_MAX_CONNECTIONS = 10;
	public const int DEFAULT_PORT = 28965;

	public GameObject playerPrefab;

	private string currentLevelName;
	private int currentLevelIdent = 0;
	private int team = 0;

	public void Start() {
		networkView.group = (int)NetworkChannel.PROTOCOL;
	}

	public static void CreateServer(int maxConnections = DEFAULT_MAX_CONNECTIONS, int port = DEFAULT_PORT) {
		Network.InitializeSecurity();
		Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
		MenuManager.DisplayDialogBox("Starting up server...");
	}

	public static void ConnectToServer(string serverIP, int port = DEFAULT_PORT) {
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
		if(info == NetworkDisconnection.LostConnection) {
			MenuManager.DisplayDialogBox("Lost connection to server!", "main_menu");
		}
	}

	public void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	public static void SetNetworkChannel(NetworkChannel channel, bool enable) {
		Network.SetSendingEnabled((int)channel, enable);
	}

	public void OnFailedToConnect(NetworkConnectionError error) {
		MenuManager.DisplayDialogBox("Failed to connect to server!", "JoinMenu");
	}

	public void OnServerInitialized() {
		MenuManager.Open_Menu("IngameMenu");
		Application.LoadLevel("Ice_Cap");
		currentLevelName = "Ice_Cap";
		currentLevelIdent++;
	}

	public void OnConnectedToServer() {
		MenuManager.Open_Menu("IngameMenu");
		MenuManager.DisplayDialogBox("Obtaining level info...");
		networkView.RPC("GetCurrentLevel", RPCMode.Server);
	}

	public void OnLevelWasLoaded(int level) {
		if(level == 0) { return; }

		MenuManager.DisplayDialogBox(null);

		Network.isMessageQueueRunning = true;
		SetNetworkChannel(NetworkChannel.GAMEPLAY, true);

		spawn();
	}

	public void spawn() {
		GameObject[] sps = GameObject.FindGameObjectsWithTag((team%2 == 0) ? "SpawnPointGreen" : "SpawnPointRed");
		Transform sp = sps[Random.Range(0, sps.Length)].transform;

		GameObject player = Network.Instantiate(playerPrefab, sp.position + new Vector3(0, 1.2f, 0), sp.rotation, 0) as GameObject;
		player.GetComponent<MouseLook>().enabled = true;
		((MonoBehaviour)player.GetComponent("FPSInputController")).enabled = true;
		player.transform.FindChild("Main Camera").gameObject.SetActive(true);
		player.GetComponent<PowerUp_Controler>().enabled = true;
		player.GetComponent<Player_loc>().enabled = true;
		player.GetComponent<Player_Controler>().enabled = true;
		player.GetComponent<Player_Controler>().setTeam(team%2 == 0);
		player.name = "LocalPlayer";
	}

	[RPC]
	public void GetCurrentLevel(NetworkMessageInfo info) {
		networkView.RPC("LoadNetworkLevel", info.sender, currentLevelName, currentLevelIdent, ++team);
	}

	[RPC]
	public void LoadNetworkLevel(string levelName, int levelIdent, int team) {
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

		Application.LoadLevel(levelName);
	}
}