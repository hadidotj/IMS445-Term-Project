using UnityEngine;
using System.Collections;

public enum NetworkChannel { GAMEPLAY, PROTOCOL, OTHER };

public class NetworkManager : MonoBehaviour {
	public const int DEFAULT_MAX_CONNECTIONS = 10;
	public const int DEFAULT_PORT = 28965;

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
				GameObject.DestroyObject(MenuManager.instance.gameObject);
				Application.LoadLevel("main_menu");
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
	}

	public void OnConnectedToServer() {
		MenuManager.Open_Menu("IngameMenu");
		Application.LoadLevel("Ice_Cap");
	}

	public void OnLevelWasLoaded(int level) {
		MenuManager.DisplayDialogBox(null);
	}

	[RPC]
	public void LoadNetworkLevel(string levelName, int levelIdent) {
		SetNetworkChannel(NetworkChannel.GAMEPLAY, false);
		Network.isMessageQueueRunning = false;

		Network.SetLevelPrefix(levelIdent);
		Application.LoadLevel(levelName);

		Network.isMessageQueueRunning = true;
		SetNetworkChannel(NetworkChannel.GAMEPLAY, true);
	}
}