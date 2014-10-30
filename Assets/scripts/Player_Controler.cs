using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {

	private bool team;
	private bool teamSet = false;
	private float charge = 100.0f; // the amount of aromur and ammo can used
	private float damage = 10.0f;

	public float fireCost = 10.0f;
	public Transform fireLocation1;
	public Transform fireLocation2;

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

	public void LazerBeamHit(GameObject attacker) {
		if(GetComponent<Team>().teamName != attacker.GetComponent<Team>().teamName) {
			subtractCharge(damage);
		}
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
		if(Input.GetMouseButtonDown(0)) {
			networkView.RPC ("FireLazer", RPCMode.All);
		}
	}

	[RPC]
	public void FireLazer(NetworkMessageInfo info) {
		GameObject obj = info.networkView.gameObject;
		
		Player_Controler plc = obj.GetComponent<Player_Controler>();
		if(plc.getCharge() != 0.0) {
			plc.subtractCharge(10.0f);
			PowerUp_Controler pc = obj.GetComponent<PowerUp_Controler>();
			Transform fl1 = plc.fireLocation1;
			Transform fl2 = plc.fireLocation2;
			
			if(pc.getMode() == 1) { // fire
				LazerBeam.CreateLazerBeam(fl1.position, fl1.rotation, new Color(1.0f, 0.627f, 0.0f), obj);
				LazerBeam.CreateLazerBeam(fl2.position, fl2.rotation, new Color(1.0f, 0.627f, 0.0f), obj);
				// TODO remove extra charge in the player_conroler
			} else if(pc.getMode() == 2) { // water
				LazerBeam.CreateLazerBeam(fl1.position,fl1.rotation, Color.blue, obj);
			} else if(pc.getMode() == 3) { // earth
				LazerBeam.CreateLazerBeam(fl1.position,fl1.rotation, Color.yellow, obj);
			} else if(pc.getMode() == 4) { // air
				LazerBeam.CreateLazerBeam(fl1.position,fl1.rotation, Color.cyan, obj);
			} else { // default
				LazerBeam.CreateLazerBeam(fl1.position, fl1.rotation, Color.green, obj);
			}
		}
	}
}
