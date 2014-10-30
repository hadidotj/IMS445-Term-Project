using UnityEngine;
using System.Collections;

public class FireLazer : MonoBehaviour {
	
	public Transform fireLocation;
	public Transform fireLocation2;
	public float cost = 5.0f;
	
	public void Update () {
		if(Input.GetMouseButtonDown(0) && gameObject.GetComponent<Player_Controler>().getCharge() != 0.0) {
			gameObject.GetComponent<Player_Controler>().subtractCharge(cost);
			PowerUp_Controler pc = (PowerUp_Controler)gameObject.GetComponent("PowerUp_Controler");
			if(pc.getMode() == 1) { // fire
				LazerBeam.CreateLazerBeam(fireLocation.position, fireLocation.rotation, Color.red, gameObject);
				LazerBeam.CreateLazerBeam(fireLocation2.position, fireLocation2.rotation, Color.red, gameObject);
				// TODO remove extra charge in the player_conroler
			} else if(pc.getMode() == 2) { // water
				LazerBeam.CreateLazerBeam(fireLocation.position,fireLocation.rotation, Color.blue, gameObject);
			} else if(pc.getMode() == 3) { // earth
				LazerBeam.CreateLazerBeam(fireLocation.position,fireLocation.rotation, Color.yellow, gameObject);
			} else if(pc.getMode() == 4) { // air
				LazerBeam.CreateLazerBeam(fireLocation.position,fireLocation.rotation, Color.cyan, gameObject);
			} else { // default
				LazerBeam.CreateLazerBeam(fireLocation.position, fireLocation.rotation, Color.green, gameObject);
			}
		}
	}
}
