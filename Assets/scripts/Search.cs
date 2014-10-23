using UnityEngine;
using System.Collections;

public class Search : MonoBehaviour {

	private int redScore = 0;
	private int greenScore = 0;
	private int index = 0;

	private GameObject[] redTargets;
	private GameObject[] greenTargets;

	// Use this for initialization
	void Start () {
		redTargets = GameObject.FindGameObjectsWithTag("RedTarget");
		greenTargets = GameObject.FindGameObjectsWithTag("GreenTarget");

		// The game doesn't like this code
		redTargets[0].GetComponent<SimpleTarget>().enableTarget();
		greenTargets[0].GetComponent<SimpleTarget>().enableTarget();
	}
	
	// Update is called once per frame
	void Update () {
		checkEndGame();
	}

	public void addPoint(string team){
		if (team.Equals ("RedTeam"))
			redScore++;
		else
			greenScore++;
	}
	public void enableNextTarget(string team){
		index++;
		// Game doesn't like this code
		if(team.Equals("RedTeam"))
			redTargets[index].GetComponent<SimpleTarget>().enableTarget();
		else
			greenTargets[index].GetComponent<SimpleTarget>().enableTarget();
	}

	public int getScore(string team){
		if (team.Equals ("RedTeam"))
			return redScore;
		else
			return greenScore;
	}
	private void checkEndGame(){
		if(redScore >= 5)
			print ("Red Team Wins!");
		else if(greenScore >= 5)
			print ("Green Team Wins!");
	}
}
