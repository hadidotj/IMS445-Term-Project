using UnityEngine;
using System.Collections;

public class Rotate_upgrade : MonoBehaviour {

	void Update() {
		gameObject.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 10.0f);
	}
}
