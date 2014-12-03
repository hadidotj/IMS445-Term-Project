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
	private List<ScoreInfo> scoreInfo = new List<ScoreInfo>();
	private Dictionary<string, ScoreInfo> playerInfo = new Dictionary<string, ScoreInfo>();

	private class GUIMessage {
		public string text;
		public Color color;
		public GUIMessage(string text, Color color) {
			this.text = text;
			this.color = color;
		}
	}

	private class ScoreInfo {
		public bool isTeam = false;
		public string name;
		public Color color;
		public int score;
		public int tags;
		public int tagged;
		public List<ScoreInfo> players = new List<ScoreInfo>();

		public ScoreInfo(string _name, int _score, int _tags, int _tagged) {
			name = _name;
			score = _score;
			tags = _tags;
			tagged = _tagged;
			isTeam = false;
			color = Color.white;
		}

		public ScoreInfo(string _name, int _score, bool _isTeam, Color _color) {
			name = _name;
			score = _score;
			tags = tagged = 0;
			isTeam = _isTeam;
			color = _color;
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
				GUI.skin.label.fontSize = 15;
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;

				Rect scoreAreaRect = new Rect(0, 35, scoreWindowRect.width, scoreWindowRect.height - 55);
				gatherData();
				GUILayout.BeginArea(scoreAreaRect); {
					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(scoreAreaRect.width), GUILayout.Height(scoreAreaRect.height)); {
						if(scoreInfo.Count > 2) {
							printHeader(scoreAreaRect);
						}

						foreach(ScoreInfo info in scoreInfo) {
							if(info.isTeam) {
								printHeader(scoreAreaRect);
								printTeamInfo(scoreAreaRect, info);

								foreach(ScoreInfo playerInfo in info.players) {
									printPlayerScore(scoreAreaRect, playerInfo);
								}
								GUILayout.Space(10);
							} else {
								printPlayerScore(scoreAreaRect, info);
							}
						}
						
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

	private void gatherData() {
		scoreInfo.Clear();

		if(NetworkManager.instance.gametype.hasTeams()) {
			ScoreInfo red = new ScoreInfo("Red", NetworkManager.instance.gametype.getTeamScore("Red"), true, Color.red);
			ScoreInfo green = new ScoreInfo("Green", NetworkManager.instance.gametype.getTeamScore("Green"), true, Color.green);

			addPlayers(red);
			addPlayers(green);

			red.name = "Read Team";
			green.name = "Green Team";
			scoreInfo.Add(red);
			scoreInfo.Add(green);
		} else {
			addPlayers(null);
		}
	}

	private void addPlayers(ScoreInfo info) {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
			if(info != null) {
				string team = player.GetComponent<Team>().teamName;
				if(team.Equals(info.name)) {
					string name = player.GetComponent<Player_Controler>().playerName;
					if(playerInfo.ContainsKey(name)) {
						ScoreInfo tmp = playerInfo[name];
						info.tags += tmp.tags;
						info.tagged += tmp.tagged;
						info.players.Add(tmp);
					} else {
						info.players.Add(new ScoreInfo(name, 0, 0, 0));
					}
				}
			} else {
				scoreInfo.Add(playerInfo[player.GetComponent<Player_Controler>().playerName]);
			}
		}
	}

	private void printHeader(Rect scoreAreaRect) {
		GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(scoreAreaRect.width*0.6f));
		GUILayout.Label("Score", GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label("Tags", GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label("Tagged", GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label("Ratio", GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.EndHorizontal();
	}

	private void printTeamInfo(Rect scoreAreaRect, ScoreInfo info) {
		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.normal.textColor = info.color;
		GUILayout.BeginHorizontal();
		GUILayout.Label(info.name, GUILayout.Width(scoreAreaRect.width*0.6f));
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUILayout.Label(""+info.score, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label(""+info.tags, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label(""+info.tagged, GUILayout.Width(scoreAreaRect.width*0.1f));
		float ratio = (info.tags == 0 || info.tagged == 0) ? 0 : (float)info.tags/(float)info.tagged;
		GUILayout.Label(""+ratio, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.EndHorizontal();
		GUI.skin.label.normal.textColor = Color.white;
	}

	private void printPlayerScore(Rect scoreAreaRect, ScoreInfo info) {
		GUI.skin.label.fontSize = 15;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.label.normal.textColor = Color.white;

		Texture2D org = GUI.skin.label.normal.background;
		if(info.name.Equals(NetworkManager.playerName)) {
			GUI.skin.label.normal.background = new Texture2D(1,1);
			GUI.skin.label.normal.background.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.25f));
			GUI.skin.label.normal.background.Apply();
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label(info.name, GUILayout.Width(scoreAreaRect.width*0.6f));
		GUILayout.Label(""+info.score, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label(""+info.tags, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.Label(""+info.tagged, GUILayout.Width(scoreAreaRect.width*0.1f));
		float ratio = (info.tags == 0 || info.tagged == 0) ? 0 : (float)info.tags/(float)info.tagged;
		GUILayout.Label(""+ratio, GUILayout.Width(scoreAreaRect.width*0.1f));
		GUILayout.EndHorizontal();

		GUI.skin.label.normal.background = org;
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

	public void Tagged(object[] args) {
		string attacker = (string)args[0];
		string victim = (string)args[1];

		if(!playerInfo.ContainsKey(attacker)) {
			playerInfo.Add(attacker, new ScoreInfo(attacker, 0, 0, 0));
		}

		if(!playerInfo.ContainsKey(victim)) {
			playerInfo.Add(victim, new ScoreInfo(victim, 0, 0, 0));
		}

		playerInfo[attacker].score++;
		playerInfo[attacker].tags++;
		playerInfo[victim].tagged++;
	}

	public void Scored(object[] args) {
		string person = (string)args[0];
		if(!playerInfo.ContainsKey(person)) {
			playerInfo.Add(person, new ScoreInfo(person, 0, 0, 0));
		}
		playerInfo[person].score += (int)args[1];
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
