using UnityEngine;
using System.Collections;

public class Capture_the_flag_controler : MonoBehaviour {


	private int redScore = 0, greenScore = 0;

	void Awake() {
		NetworkManager.SetGametype(this);
	}

	private bool isGameOver() {
		return (redScore == 3 || greenScore == 3);
	}

	public void flagCaptured(int team) {
		switch(team) {
		case 1:
			redScore++;
			break;
		case 2:
			greenScore++;
			break;
		default:
			Debug.Log("There was an error in increaasing the score.");
			break;
		}
	}
}
