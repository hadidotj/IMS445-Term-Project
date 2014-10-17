using UnityEngine;
using System.Collections;

public class Add_Snow : MonoBehaviour {

	public GameObject snow;

	void Start () {
		Vector3 h = gameObject.transform.position;
		h.y += 10.0f;
		GameObject snow_obj = (GameObject)Instantiate(snow, h, gameObject.transform.rotation);
		snow_obj.transform.parent = gameObject.transform;
	}
}
