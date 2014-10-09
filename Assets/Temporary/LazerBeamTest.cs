using UnityEngine;
using System.Collections;

public class LazerBeamTest : MonoBehaviour {
	private float timePassed = 0.0f;
	public Color myColor = Color.green;

	void Update () {
		if(timePassed <= 0.0f) {
			timePassed = 1.0f;
			LazerBeam.CreateLazerBeam(transform.position, transform.rotation, myColor);
		}

		timePassed -= Time.deltaTime;
	}
}
