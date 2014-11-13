using UnityEngine;
using System.Collections;

public class Player_Controler : MonoBehaviour {
	
	public float charge = 100.0f; // the amount of aromur and ammo can used
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

	private Camera myCam;

	public void Start() {
		myCam = gameObject.GetComponentInChildren<Camera>();
	}

	public void addCharge(float amnt) {
		charge = Mathf.Clamp(charge+amnt, 0, 100);
		if(charge >= fireCost) {
			setChargedColor(true);
		}
	}

	public void subtractCharge(float amnt) {
		charge = Mathf.Clamp(charge-amnt, 0, 100);
		if(charge < fireCost) {
			setChargedColor(false);
		}
	}

	public void setChargedColor(bool on) {
		GetComponent<Player_Network_Controller>().setChargedColor(on);
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
			PowerUp_Controler pc = (PowerUp_Controler) gameObject.GetComponent("PowerUp_Controler");
			float rate = (pc.getMode() == 5) ? 2.0f : 1.0f;
			addCharge(33.0f * Time.deltaTime * rate);
			pc.reduce();
		} else {
			SoundUtils.stopAllForClip(gameObject, powerUpSound);
		}

		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(myCam);
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
			if(obj != gameObject) {
				GUIText text = obj.GetComponentInChildren<GUIText>();
				float distance = Vector3.Distance(transform.position, obj.transform.position);
				if(distance < 50 && GeometryUtility.TestPlanesAABB(planes, obj.collider.bounds)) {
					// Scale text between 15 and 5 depending on distance
					text.fontSize = (int)Mathf.Lerp(15, 5, distance/50);
					text.transform.position = myCam.WorldToViewportPoint(obj.transform.position + new Vector3(0.0f, 1.5f, 0.0f));
					text.enabled = true;
				} else {
					text.enabled = false;
				}
			}
		}
	}
}
