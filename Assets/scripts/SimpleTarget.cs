using UnityEngine;
using System.Collections;

public class SimpleTarget : MonoBehaviour {

	private int hitTimes = 0;
	private bool enabled = false;
	// This is wrong, but I don't know how to access the script.
	Search s;

	void start(){
		// They  still don't start disabled for some reason
		gameObject.SetActive(false); 
	}

	public void LazerBeamHit(GameObject owner) {
		Team t = owner.GetComponent<Team>();
		Debug.Log("Target hit by " + owner.name + " on team " + t.teamName);
		if(t.teamName != gameObject.GetComponent<Team>().teamName) {
			hitTimes++;
			if(hitTimes >= 1) {
				disableTarget();
				// This is the wrong way of doing it (s doesn't exist really), 
				// and it doesn't access the script to add points/enable new target.
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
