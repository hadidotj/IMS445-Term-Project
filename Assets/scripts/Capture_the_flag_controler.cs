using UnityEngine;
using System.Collections;

public class Capture_the_flag_controler : LazerTagGametype {


	private int redScore = 0, greenScore = 0;

	void Awake() {
		name = "Capture the Flag";
		NetworkManager.SetGametype(this);
	}

	void FixedUpdate() {
		if(isGameOver()) {
			Destroy(GameObject.FindGameObjectWithTag("Flag"));
			string message = ((redScore == 3) ? "Red" : "Green" )+ " Team Wins!";
			MenuManager.DisplayDialogBox(message, "IngameMenu");
		}
	}

	private bool isGameOver() {
		return (redScore == 3 || greenScore == 3);
	}

	public void flagCaptured(string name, string team) {
		string otherTeam = "Red";
		Color color = Color.green;
		
		if(team.Equals("Red")) {
			otherTeam = "Green";
			color = Color.red;
		}

		NetworkManager.SendTextMessage(name + " captured a " + otherTeam + " flag!", color);
		NetworkManager.GametypeSend("RPCTargetDestroyed", team);
	}

	public void RPCTargetDestroyed(string team) {
		if(team.Equals("Red")) {
			redScore++;
		} else {
			greenScore++;
		}
	}

	public int getScore(string team){
		if (team.Equals ("Red"))
			return redScore;
		else
			return greenScore;
	}
	
	public override int getTeamScore(string team) {
		return getScore(team);
	}
}
