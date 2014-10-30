using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {

	private bool team;
	private bool teamSet = false;
	private float charge = 100.0f; // the amount of aromur and ammo can used
	private float damage = 40.0f;
	private float dist;

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
			GetComponent<Team>().teamName = ((t) ? "Green" : "Red");
			teamSet = true;
		}
	}

	public void LazerBeamHit(GameObject attacker) {
		if(GetComponent<Team>().teamName != attacker.GetComponent<Team>().teamName) {
			gameObject.networkView.RPC("LazerBeamHitMe", RPCMode.All);
		}
	}

	[RPC]
	public void LazerBeamHitMe() {
		subtractCharge(damage);
	}

	public void addDamage(float damageBuff) {
		damage += damageBuff;
	}

	public void subtractDamage(float damageReduction) {
		damage -= damageReduction;
	}

	public void recharge() {

		if(charge < 100.0f)
			charge += 20.0f*Time.deltaTime*1;
		if(charge > 100.0f)
			charge = 100.0f;
	}

	void OnGUI() {
		if (charge > 0.0f) {
			//GUI.Box(new Rect(5, 5, Screen.width/3, 20), "" + charge);
			GUI.Box(new Rect(5, 5, Screen.width/3/(100/charge), 20), "" + (int)charge);
		}else {
			GUI.Box (new Rect(5, 5, Screen.width/3, 20), "Out of charge!");
		}
	}

	//(Player_loc)
		
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			FireMyLazer();
		}

		dist = Vector3.Distance(gameObject.transform.position, GameObject.FindWithTag((GetComponent<Team>().teamName) + " Base").transform.position);
		if(dist <= 50.0f) {
			recharge ();
		}
	}

	public void FireMyLazer() {
		Player_Controler plc = gameObject.GetComponent<Player_Controler>();
		if(plc.getCharge() >= 10.0) {
			plc.subtractCharge(10.0f);
			PowerUp_Controler pc = gameObject.GetComponent<PowerUp_Controler>();

			Color fireColor = ((gameObject.GetComponent<Team>().teamName == "Green") ? Color.green : Color.red);
			
			if(pc.getMode() == 1) { // fire
				fireColor = new Color(1.0f, 0.627f, 0.0f);
				networkView.RPC("FireLazer", RPCMode.All, plc.fireLocation2.position, plc.fireLocation2.rotation, fireColor);
				// TODO remove extra charge in the player_conroler
			} else if(pc.getMode() == 2) { // water
				fireColor = Color.blue;
			} else if(pc.getMode() == 3) { // earth
				fireColor = Color.yellow;
			} else if(pc.getMode() == 4) { // air
				fireColor = Color.cyan;
			}
			networkView.RPC("FireLazer", RPCMode.All, plc.fireLocation1.position, plc.fireLocation1.rotation, new Vector3(fireColor.r, fireColor.g, fireColor.b));
		}
	}
	
	[RPC]
	public void FireLazer(Vector3 pos, Quaternion rot, Vector3 color, NetworkMessageInfo info) {
		LazerBeam.CreateLazerBeam(pos, rot, new Color(color.x, color.y, color.z, 1.0f), info.networkView.gameObject);
	}
}
