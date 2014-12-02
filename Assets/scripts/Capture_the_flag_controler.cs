﻿using UnityEngine;
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
