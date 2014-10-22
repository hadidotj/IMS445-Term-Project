using UnityEngine;
using System.Collections;

public enum NetworkChannel { GAMEPLAY, PROTOCOL, OTHER };

public class NetworkManager : MonoBehaviour {
	public static readonly int DEFAULT_MAX_CONNECTIONS = 10;
	public static readonly int DEFAULT_PORT = 28965;

	public static void CreateServer(int maxConnections = DEFAULT_MAX_CONNECTIONS, int port = DEFAULT_PORT) {
		Network.InitializeSecurity();
		Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
		// Switch menu to loading menu (state=setting up)
	}

	public static void ConnectToServer(string serverIP, int port = DEFAULT_PORT) {
		Network.Connect(serverIP, port);
		// Switch menu to loading menu (state=contacting server)
	}

	public static void SetNetworkChannel(NetworkChannel channel) {
		Network.SetSendingEnabled(channel, false);
		Network.SetReceivingEnabled(channel, false);
	}

	public void OnFailedToConnect(NetworkConnectionError error) {
		// Switch menu to main menu. Show disconnected error.
	}

	public void OnServerInitialized() {
		// Server only method. Switch menu to loading menu (state=loading)
	}

	public void OnConnectedToServer() {
		// Client only method. Switch menu to loading menu (state=syncing). Request current level RPC
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