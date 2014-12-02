using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngameMenu : AbstractMenu {

	public Texture2D header;
	public Texture2D back;

	private int currentHoveredOver = -1;
	private bool showMenu = false;
	private bool showScores = false;
	private bool lockCursor = true;
	private Vector2 scrollPosition;
	private List<GUIMessage> msgs = new List<GUIMessage>();

	private class GUIMessage {
		public string text;
		public Color color;
		public GUIMessage(string text, Color color) {
			this.text = text;
			this.color = color;
		}
	}

	public override void Draw(){
		if(showMenu) {
			Vector3 mouse = Input.mousePosition;
			mouse.y = Screen.height-mouse.y;

			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

			// Draw header
			GUI.DrawTexture(new Rect(Screen.width/2.0f - header.width/2.0f, 10, header.width, header.height), header);
			
			// Back Button
			GUI.backgroundColor = Color.red;
			Rect backButtonRect = new Rect(10.0f, Screen.height-back.height-10.0f, back.width, back.height);
			if(GUI.Button (backButtonRect, back)){
				ButtonPressed();
				NetworkManager.DisconnectFromServer();
			} else if(currentHoveredOver != 1 && backButtonRect.Contains(mouse)) {
				currentHoveredOver = 1;
				ButtonHover();
			} else if(currentHoveredOver == 1 && !backButtonRect.Contains(mouse)) {
				currentHoveredOver = -1;
			}
		} else if(showScores) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

			Rect scoreWindowRect = new Rect(Screen.width*0.125f, 25f, Screen.width*0.75f, Screen.height-50f);
			GUI.Box(scoreWindowRect, "");

			GUILayout.BeginArea(scoreWindowRect); {
				GUI.skin.label.fontSize = 20;
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(5, 0, scoreWindowRect.width-10, 30), NetworkManager.instance.gametype.getName());
				GUI.skin.label.alignment = TextAnchor.MiddleRight;
				GUI.Label(new Rect(5, 0, scoreWindowRect.width-10, 30), NetworkManager.instance.currentLevelName);
				GUI.skin.label.fontSize = 12;
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;

				Rect scoreAreaRect = new Rect(0, 35, scoreWindowRect.width, scoreWindowRect.height - 55);
				GUILayout.BeginArea(scoreAreaRect); {
					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(scoreAreaRect.width), GUILayout.Height(scoreAreaRect.height)); {
						// Column headers
						GUILayout.BeginHorizontal();
						GUILayout.Label("", GUILayout.Width(scoreAreaRect.width*0.6f));
						GUILayout.Label("Score", GUILayout.Width(scoreAreaRect.width*0.1f));
						GUILayout.Label("Tags", GUILayout.Width(scoreAreaRect.width*0.1f));
						GUILayout.Label("Tagged", GUILayout.Width(scoreAreaRect.width*0.1f));
						GUILayout.Label("Ratio", GUILayout.Width(scoreAreaRect.width*0.1f));
						GUILayout.EndHorizontal();

					} GUILayout.EndScrollView();
				} GUILayout.EndArea();

				GUI.skin.label.fontSize = 12;
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(5, scoreWindowRect.height-20, scoreWindowRect.width-10, 20), NetworkManager.hostName);

				NetworkPlayer nullPlayer = new NetworkPlayer();
				NetworkPlayer player = (Network.isServer) ? Network.player : ((Network.connections.Length > 0) ? Network.connections[0] : nullPlayer);
				if(nullPlayer.Equals(player)) {
					GUI.skin.label.alignment = TextAnchor.MiddleRight;
					GUI.Label(new Rect(5, scoreWindowRect.height-20, scoreWindowRect.width-10, 20), player.ipAddress + ":" + player.port);
				}
			} GUILayout.EndArea();
		} else if(msgs.Count > 0) {
			for(int i=0; i<msgs.Count; i++) {
				GUIStyle style = new GUIStyle();
				style.normal.textColor = msgs[i].color;
				style.fontSize = 18;
				style.alignment = TextAnchor.MiddleRight;
				GUI.Label(new Rect(5, i*20, Screen.width-10, 20), msgs[i].text, style);
			}
		}

		Screen.lockCursor = lockCursor;
	}

	public IEnumerator Message(object[] args) {
		string msg = (string) args[0];
		Color color = (Color) args[1];

		GUIMessage guimsg = new GUIMessage(msg, color);
		if(msgs.Count == 5) {
			msgs.RemoveAt(0);
		}
		msgs.Add(guimsg);
		yield return new WaitForSeconds(30.0f);
		msgs.Remove(guimsg);
	}

	public void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			showScores = false;
			showMenu = !showMenu;
			lockCursor = !lockCursor;
		} else if(!showMenu) {
			showScores = Input.GetKey(KeyCode.Tab);
			lockCursor = !showScores;
		}
	}
}
