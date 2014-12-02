using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	private bool held = false;
	private Vector3 startingPos;
	private Capture_the_flag_controler cfc;

	void Start() {
		startingPos = transform.position;
		cfc = (Capture_the_flag_controler)GameObject.FindWithTag("GameController").GetComponent<Capture_the_flag_controler>();
	}

	void OnCollisionEnter(Collision other) {
		Debug.Log ("I the flag was collided");
	}

	void OnTriggerEnter(Collider collider) {
		Debug.Log ("I the flag was triggered");
		if(collider.transform.tag.Equals("Player") && !held) {
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
