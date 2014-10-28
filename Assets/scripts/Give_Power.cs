using UnityEngine;
using System.Collections;

public class Give_Power : MonoBehaviour {
	public int powerNum;

	
	void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag.Equals("Player")) {
			PowerUp_Controler pc = (PowerUp_Controler) collider.gameObject.GetComponent("PowerUp_Controler");
			if(powerNum == 1) {
		 		pc.fireMode();
			} else if(powerNum == 2) {
				pc.waterMode();
			} else if(powerNum == 3) {
				pc.earthMode();
			} else if(powerNum == 4) {
				pc.airMode();
			} else {
				Debug.Log("There wss an error assigning the power up. Look in Give_Power.cs.");
			}
		}
	}
}
