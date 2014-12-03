using UnityEngine;
using System.Collections;

public class TeamDeathMatch : LazerTagGametype {

	public bool isActive = true;

	public int redScore = 0;
	public int greenScore = 0;

	// Use this for initialization
	void Awake () {
		name = "Team Death Match";
		NetworkManager.SetGametype(this);
	}

	public void IncRedScore() {
		redScore++;
		checkEnd();
		NetworkManager.SendTextMessage("Red Score: " + redScore, Color.red);
	}

	public void IncGreenScore() {
		greenScore++;
		checkEnd();
		NetworkManager.SendTextMessage("Green Score: " + greenScore, Color.green);
	}

	public void checkEnd() {
		if(redScore == 10)
			MenuManager.DisplayDialogBox("Red Team Wins!", "IngameMenu");
		else if(greenScore == 10)
			MenuManager.DisplayDialogBox("Green Team Wins!", "IngameMenu");
	}

	public override int getTeamScore(string team) {
		return ("Red".Equals(team)) ? redScore : greenScore;
	}
}
