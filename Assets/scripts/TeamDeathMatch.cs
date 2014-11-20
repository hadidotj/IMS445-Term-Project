using UnityEngine;
using System.Collections;

public class TeamDeathMatch : MonoBehaviour {

	public bool isActive = true;

	public int redScore = 0;
	public int greenScore = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(redScore == 250)
			MenuManager.DisplayDialogBox("Red Team Wins!", "IngameMenu");
		else if(greenScore == 250)
			MenuManager.DisplayDialogBox("Green Team Wins!", "IngameMenu");
	}
}
