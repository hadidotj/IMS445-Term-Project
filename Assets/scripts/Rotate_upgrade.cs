using UnityEngine;
using System.Collections;

public class Rotate_upgrade : MonoBehaviour {
	
	private float t = 0.0f;
	void Update() {
		t += Time.deltaTime % 360.0f;
		gameObject.transform.Rotate(new Vector3(0, 1, 0), t);
	}
}
