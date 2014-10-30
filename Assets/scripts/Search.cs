using UnityEngine;
using System.Collections;

public class Search : MonoBehaviour {

	private int redScore = 0;
	private int greenScore = 0;

	private ArrayList redTargets = new ArrayList();
	private ArrayList greenTargets = new ArrayList();

	// Use this for initialization
	void Start () {
		NetworkManager.SetGametype(this);
		GameObject[] redTargetsArr = GameObject.FindGameObjectsWithTag("RedTarget");
		GameObject[] greenTargetsArr = GameObject.FindGameObjectsWithTag("GreenTarget");

		// Disable all targets
		foreach(GameObject redTarget in redTargetsArr) {
			redTargets.Add(redTarget);
			redTarget.SetActive(false);
			redTarget.GetComponent<SimpleTarget>().s = this;
		}

		foreach(GameObject greenTarget in greenTargetsArr) {
			greenTargets.Add(greenTarget);
			greenTarget.SetActive(false);
			greenTarget.GetComponent<SimpleTarget>().s = this;
		}

		// Then enable the first target for each
		redTargetsArr[0].SetActive(true);
		greenTargetsArr[0].SetActive(true);
	}

	public void TargetDestroyed(string team) {
		NetworkManager.GametypeSend("RPCTargetDestroyed", team);
	}
	
	public void RPCTargetDestroyed(string team) {
		if(team.Equals("Red")) {
			redScore++;
			Destroy((GameObject)greenTargets[0]);
			greenTargets.RemoveAt(0);
			if(greenTargets.Count <= 0) {
				MenuManager.DisplayDialogBox("Red Team Wins!", "IngameMenu");
			} else {
				((GameObject) greenTargets[0]).SetActive(true);
			}
		} else {
			greenScore++;
			Destroy((GameObject)redTargets[0]);
			redTargets.RemoveAt(0);
			if(redTargets.Count <= 0) {
				MenuManager.DisplayDialogBox("Green Team Wins!", "IngameMenu");
			} else {
				((GameObject) redTargets[0]).SetActive(true);
			}
		}
	}

	public int getScore(string team){
		if (team.Equals ("Red"))
			return redScore;
		else
			return greenScore;
	}
}
