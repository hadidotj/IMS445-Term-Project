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
	}

	string teamHolding = "";
	[RPC]
	void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag.Equals("Player") && !held) {
			if (collider.transform == null) return;
			obj = collider.gameObject;
			transform.parent = collider.transform;
			held = true;
			teamHolding = obj.GetComponent<Team>().teamName;
		} else if(held && collider.transform.tag.Equals("Pedistal") &&
		          (teamHolding.Equals(collider.gameObject.GetComponent<Team>().teamName))) {
			held = false;
			cfc.flagCaptured(transform.parent.GetComponent<Player_Controler>().playerName, transform.parent.GetComponent<Team>().teamName);
			transform.parent = null;
			transform.position = startingPos;
			obj = null;
			teamHolding = "";
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
				teamHolding = "";
			}
		}
	}
}
