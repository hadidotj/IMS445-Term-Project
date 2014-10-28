using UnityEngine;
using System.Collections;

public class PowerUp_Controler : MonoBehaviour {

	private int mode = -1;
	private float waterDuration = 5.0f;
	private float waterCurrent = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 	switch(mode) {
			case 1:
				//when firing them both cost 1.5 the ammo
				break;
			case 2:
				waterCurrent += Time.deltaTime;
				if(waterCurrent >= waterDuration) {
					waterCurrent = 0;
					changeMode(-1);
				} else {
					//set invisable
				}
				break;
			case 3:
				if(isDefending()) {
					//make the ammo stronger. so the lazer will need damage to be change-able
				}
				break;
			case 4:
				//double the speed rate
				break;
			default:
				break;
		}
	}

	bool isDefending() {
		//get the player's team
		//return if gameobject.transform.posistion - (closest station's posistion) < threshold

		return false;
	}

	public int getMode() {
		return mode;
	}

	private void changeMode(int i) {
		mode = i;
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
}
