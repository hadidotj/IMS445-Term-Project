using UnityEngine;
using System.Collections;

public class Give_Power : MonoBehaviour {
	public int powerNum;

	
	void OnTriggerEnter(Collider collider) {
		if(collider.transform.tag.Equals("Player")) {
			PowerUp_Controler pc = (PowerUp_Controler) collider.gameObject.GetComponent("PowerUp_Controler");
			pc.fireMode();
		}
	}
}
