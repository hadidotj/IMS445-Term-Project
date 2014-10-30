using UnityEngine;
using System.Collections;

public class SimpleTarget : MonoBehaviour {

	private int hitTimes = 0;

	public Search s;

	public void LazerBeamHit(GameObject owner) {
		Team t = owner.GetComponent<Team>();
		if(t.teamName != gameObject.GetComponent<Team>().teamName) {
			hitTimes++;
			if(hitTimes >= 1) {
				s.TargetDestroyed(t.teamName);
			}
		}
	}
}
