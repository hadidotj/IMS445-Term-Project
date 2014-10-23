using UnityEngine;
using System.Collections;

public class SimpleTarget : MonoBehaviour {

	private int hitTimes = 0;
	private bool enabled = false;
	Search s;

	void start(){
		gameObject.SetActive(false);
	}

	public void LazerBeamHit(GameObject owner) {
		Team t = owner.GetComponent<Team>();
		Debug.Log("Target hit by " + owner.name + " on team " + t.teamName);
		if(t.teamName != gameObject.GetComponent<Team>().teamName) {
			hitTimes++;
			if(hitTimes >= 1) {
				disableTarget();
				s.addPoint(t.teamName);
				s.enableNextTarget(t.teamName);
			}
		}
	}

	public void enableTarget(){
		enabled = true;
		gameObject.SetActive(true);
	}

	public void disableTarget(){
		enabled = false;
		gameObject.SetActive(false);
	}
}
