using UnityEngine;
using System.Collections;

public class PowerUp_Controler : MonoBehaviour {

	private int mode = -1;
	private float waterDuration = 5.0f;
	private float waterCurrent = 0.0f;
	private Transform graphic;
	private CharacterMotor chMotor;
	private int rechargeFaster = 100;

	// Use this for initialization
	void Start () {
		string name = gameObject.transform.GetChild (0).name;
		int i = 0;
		for(; i <gameObject.transform.childCount && name != "Graphics"; ++i) {
			// increments till it finds the  the graphics componet of the fpc
			name = gameObject.transform.GetChild (i).name;
		}
		// name = gameObject.transform.GetChild (i).name;
		graphic = gameObject.transform.GetChild (i);

		chMotor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		chMotor.movement.maxForwardSpeed = 30.0f; // the default
	 	switch(mode) {
			case 1:
				//when firing them both cost 1.5 the ammo
				break;
			case 2:
				waterCurrent += Time.deltaTime;
				if(waterCurrent >= waterDuration) {
					waterCurrent = 0;
					noMode();
					graphic.renderer.enabled = true;
				} else {
					graphic.renderer.enabled = false;
				}
				break;
			case 3:
				if(isDefending()) {
					//make the ammo stronger. so the lazer will need damage to be change-able
					gameObject.GetComponent<Player_Controler>().addDamage (40.0f);
				}
				break;
			case 4:
				//double the speed rate
				chMotor.movement.maxForwardSpeed = 60.0f;
				break;
			case 5:
				//double the recharge speed
				break;
			default:
				break;
		}
	}

	public void reduce() {
		rechargeFaster--;
		if(rechargeFaster <= 0) {
			rechargeFaster = 100;
			noMode();
		}
	}

	bool isDefending() {
		//get the player's team
		//return if gameobject.transform.posistion - (closest station's posistion) < threshold
		if(gameObject.GetComponent<Player_Controler>().getDist () <= 125.0f)
			return true;
		return false;
	}

	public int getMode() {
		return mode;
	}

	private void changeMode(int i) {
		mode = i;
	}

	public void noMode() {
		changeMode(-1);
	}

	public void fireMode() {
		changeMode(1);
	}

	public void waterMode() {
		changeMode(2);
	}

	public void earthMode() {
		changeMode(3);
	}
	
	public void airMode() {
		changeMode(4);
	}
	
	public void lightningMode() {
		changeMode(5);
	}
}
