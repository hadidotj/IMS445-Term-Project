﻿using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {
	
	private float charge = 100.0f; // the amount of aromur and ammo can used
	private float damage = 40.0f;
	private float dist;

	public float fireCost = 10.0f;
	public Transform fireLocation1;
	public Transform fireLocation2;
	public Texture2D crosshair;
	public string playerName;

	public AudioClip fireSound;
	public AudioClip taggedSound;
	public AudioClip powerDownSound;
	public AudioClip powerUpSound;

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
		SoundUtils.playSound(gameObject, taggedSound, 1.0f);
		if(charge <= 0) {
			SoundUtils.playSound(gameObject, powerDownSound, 0.8f);
		}
	}

	public void addDamage(float damageBuff) {
		damage += damageBuff;
	}

	public void subtractDamage(float damageReduction) {
		damage -= damageReduction;
	}

	public float getDist() {
		return dist;
	}

	void OnGUI() {
		if (charge > 0.0f) {
			//GUI.Box(new Rect(5, 5, Screen.width/3, 20), "" + charge);
			GUI.Box(new Rect(5, 5, Screen.width/3/(100/charge), 20), "" + (int)charge);
		}else {
			GUI.Box (new Rect(5, 5, Screen.width/3, 20), "Out of charge!");
		}

		GUI.DrawTexture(new Rect(Screen.width/2.0f, Screen.height/2.0f + 16.0f, 16.0f, 16.0f), crosshair);
	}

	//(Player_loc)
		
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			GetComponent<Player_Network_Controller>().FireMyLazer();
		}

		dist = Vector3.Distance(gameObject.transform.position, GameObject.FindWithTag((GetComponent<Team>().teamName) + " Base").transform.position);
		if(dist <= 50.0f && charge < 100.0f) {
			if(SoundUtils.isNotPlayingClip(gameObject, powerUpSound)) {
				SoundUtils.playSoundAt(gameObject, powerUpSound, charge/100.0f*powerUpSound.length, 0.8f);
			}
			addCharge(33.0f*Time.deltaTime);

		} else {
			SoundUtils.stopAllForClip(gameObject, powerUpSound);
		}
	}
}
