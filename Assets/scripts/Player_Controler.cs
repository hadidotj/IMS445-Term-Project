using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {

	private bool team;
	private bool teamSet = false;
	private float charge = 0.0f; // the amount of aromur and ammo can used
	private float damage = 10.0f;

	void Start() {
		addCharge(100.0f);
	}

	public void addCharge(float amnt) {
		charge += amnt;
	}

	public void subtractCharge(float amnt) {
		charge -= amnt;
		print (charge);
	}

	public double getCharge(){
		return charge;
	}

	private bool canFire() {
		return charge > 0.0f;
	}
	
	public void setTeam(bool t) {
		if(!teamSet) {
			team = t;
			teamSet = true;
		}
	}

	public void playerHit() {
		subtractCharge(damage);
	}

	public void addDamage(float damageBuff) {
		damage += damageBuff;
	}

	public void subtractDamage(float damageReduction) {
		damage -= damageReduction;
	}

	void OnGUI() {
		if (charge > 0.0f) {
			//GUI.Box(new Rect(5, 5, Screen.width/3, 20), "" + charge);
			GUI.Box(new Rect(5, 5, Screen.width/3/(100/charge), 20), "" + charge);
		}else {
			GUI.Box (new Rect(5, 5, Screen.width/3, 20), "Out of charge!");
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
