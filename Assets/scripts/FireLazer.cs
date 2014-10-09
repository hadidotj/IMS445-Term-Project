using UnityEngine;
using System.Collections;

public class FireLazer : MonoBehaviour {

	public Transform fireLocation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			LazerBeam.CreateLazerBeam(fireLocation.position, fireLocation.rotation, Color.green);
		}
	}
}
