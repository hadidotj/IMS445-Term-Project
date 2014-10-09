using UnityEngine;
using System.Collections;

public class SimpleTarget : MonoBehaviour {

	private int hitTimes = 0;

	public void LazerBeamHit(GameObject owner) {
		Team t = owner.GetComponent<Team>();
		Debug.Log("Target hit by " + owner.name + " on team " + t.teamName);
		if(t.teamName != gameObject.GetComponent<Team>().teamName) {
			hitTimes++;
			if(hitTimes > 5) {
				Destroy(gameObject);
			}
		}
	}
}
