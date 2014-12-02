using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

	private ArrayList players = new ArrayList();

	void Start () {
		GameObject[] playersArr = GameObject.FindGameObjectsWithTag("Player");
		
		// Disable all targets
		foreach(GameObject player in playersArr) {
			players.Add(player);
		}
	}

	public void createScoreBoard() {
		GUILayout.BeginArea(new Rect(Screen.width/2 - 200, Screen.height/2 - 250, 400, 500));

		foreach(GameObject player in players) {
			GUILayout.BeginHorizontal("Box");

			GUILayout.BeginVertical(GUILayout.Width(150));
			GUILayout.Label( player.GetComponent<Player_Network_Controller>().playerName, GUILayout.Width(150));
			GUILayout.EndVertical();

			GUILayout.BeginVertical(GUILayout.Width(75));
			GUILayout.Label( player.GetComponent<Player_Network_Controller>().playerScore.ToString(), GUILayout.Width(150));
			GUILayout.EndVertical();

			GUILayout.EndHorizontal ();
		}
	}

	void OnGUI() {
		if(Input.GetKey(KeyCode.CapsLock)) {
			createScoreBoard();
		}
	}
}
