using UnityEngine;
using System.Collections;

public class FireLazer : MonoBehaviour {

	public Transform fireLocation;
	
	public void Update () {
		if(Input.GetMouseButtonDown(0)) {
			LazerBeam.CreateLazerBeam(fireLocation.position, fireLocation.rotation, Color.green, gameObject);
		}
	}
}
