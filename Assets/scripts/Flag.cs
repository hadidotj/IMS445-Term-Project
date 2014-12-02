using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	private bool held = false;
	private Vector3 startingPos;
	private Capture_the_flag_controler cfc;

	void Start() {
		startingPos = transform.position;
		cfc = (Capture_the_flag_controler)GameObject.FindWithTag("GameController").GetComponent<Capture_the_flag_controler>();
		Debug.Log ("starting at " + startingPos.ToString());
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag.Equals("Player") && !held) {
			if (collider.transform == null) return;

			Debug.Log ("I the flag was triggered");
			transform.parent = collider.transform;
			held = true;
		} else if(held && collider.transform.tag.Equals("Pedistal")) {
			held = false;
			cfc.flagCaptured((transform.parent.GetComponent<Team>().teamName.Equals("Red")) ? 1 : 2);
			transform.parent = null;
			transform.position = startingPos;
		}
	}

	void FixedUpdate () {
//		Debug.Log("I the flag am " + ((held) ? "" : "not ") + "being held");
		if(held) { // reduce the player's charge
			Player_Controler pc = (Player_Controler) transform.parent.GetComponent<Player_Controler>();
			pc.subtractCharge(2.0f);
			held = pc.getCharge() > 0;
			if (held){ // can nolonger hold it
				transform.parent = null; // the scene
			}
		}
	}
}
