using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	private bool held = false;
	private Vector3 startingPos;
	private Capture_the_flag_controler cfc;
	private GameObject obj = null;

	void Start() {
		startingPos = transform.position;
		cfc = (Capture_the_flag_controler)GameObject.FindWithTag("GameController").GetComponent<Capture_the_flag_controler>();
		Debug.Log ("starting at " + startingPos.ToString());
	}

	[RPC]
	void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag.Equals("Player") && !held) {
			if (collider.transform == null) return;
			obj = collider.gameObject;
			transform.parent = collider.transform;
			held = true;
		} else if(held && collider.transform.tag.Equals("Pedistal")) {
			held = false;
			cfc.flagCaptured((transform.parent.GetComponent<Team>().teamName.Equals("Red")) ? 1 : 2);
			transform.parent = null;
			transform.position = startingPos;
			obj = null;
		}
	}

	void FixedUpdate () {
		if(held) { // reduce the player's charge
			Player_Controler pc = (Player_Controler) obj.GetComponent<Player_Controler>();
			pc.subtractCharge(0.1f);
			held = pc.getCharge() > 0.0f;
			if (!held){ // can nolonger hold it
				transform.parent = null; // the scene
				obj = null;
			}
		}
	}
}
