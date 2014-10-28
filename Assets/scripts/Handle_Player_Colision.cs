using UnityEngine;
using System.Collections;

public class Handle_Player_Colision : MonoBehaviour {
	
	private void OnTriggerEnter(Collider other) {
		if(other.transform.tag.Equals("Player")) {
			Player_loc pl=(Player_loc)other.gameObject.GetComponent("Player_loc");
			other.gameObject.transform.position = pl.getPos();
			PowerUp_Controler pc = (PowerUp_Controler)other.gameObject.GetComponent("PowerUp_Controler");
			pc.noMode();
		}
	}
}