using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {
	
	private float charge = 100.0f; // the amount of aromur and ammo can used
	private float damage = 40.0f;
	private float dist;

	public float fireCost = 10.0f;
	public Transform fireLocation1;
	public Transform fireLocation2;
	public string playerName;

	public void addCharge(float amnt) {
		charge = Mathf.Clamp(charge+amnt, 0, 100);
	}

	public void subtractCharge(float amnt) {
		charge = Mathf.Clamp(charge-amnt, 0, 100);
	}

	public double getCharge(){
		return charge;
	}

	private bool canFire() {
		return charge > 0.0f;
	}

	public void LazerBeamHit(GameObject attacker) {
		if(charge > 0 && GetComponent<Team>().teamName != attacker.GetComponent<Team>().teamName) {
			gameObject.networkView.RPC("LazerBeamHitMe", RPCMode.All);
			if(charge <= 0) {
				NetworkManager.SendTextMessage(attacker.GetComponent<Player_Controler>().playerName + " tagged " + playerName, Color.yellow);
			}
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
			addCharge(20.0f*Time.deltaTime);
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
			GetComponent<Player_Network_Controller>().FireMyLazer();
		}

		dist = Vector3.Distance(gameObject.transform.position, GameObject.FindWithTag((GetComponent<Team>().teamName) + " Base").transform.position);
		if(dist <= 50.0f) {
			recharge ();
		}
	}
}
