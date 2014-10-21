using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {
	private float t = 0.0f;
	private float t2 = 0.0f;
	void Update() {
		t += Time.deltaTime % 360.0f;
		t2 += (Time.deltaTime * 0.15f) % 360.0f;
		gameObject.transform.Rotate(new Vector3(0, 0, 1), t);
		gameObject.transform.Rotate(new Vector3(1, 0, 1), t2);
	}
}
