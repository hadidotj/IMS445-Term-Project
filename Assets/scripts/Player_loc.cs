using UnityEngine;
using System.Collections;

public class Player_loc : MonoBehaviour {

	private Vector3 loc;

	void Start () {
		loc = gameObject.transform.position;
	}

	public Vector3 getPos() {
		return loc;
	}

	public void updateLoc(Vector3 nLoc) {
		loc = nLoc;
	}
}
