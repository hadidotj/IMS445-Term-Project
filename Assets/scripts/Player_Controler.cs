using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {

	private bool team;
	private bool teamSet = false;
	private double charge = 0.0; // the amount of aromur and ammo can used

	void Start() {
		addCharge(100.0);
	}

	public void addCharge(double amnt) {
		charge += amnt;
	}

	private bool canFire() {
		return charge > 0.0;
	}
	
	public void setTeam(bool t) {
		if(!teamSet) {
			team = t;
			teamSet = true;
		}
	}

	//(Player_loc)
		
	void Update () {
		//check for button press
			//then fire
		//if in base
			//addCharge(5.0);

	}
}
