using UnityEngine;
using System.Collections;

public class LazerBeam : MonoBehaviour {

	public static readonly float LIFETIME_SECONDS = 5.0f;
	public static readonly float BEAM_SPEED = 100.0f;

	public static GameObject CreateLazerBeam(Vector3 position, Quaternion rotation, Color color) {
		GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		beam.name = "Lazer-Beam (" + beam.GetInstanceID() + ")";
	
		beam.transform.position = position;
		beam.transform.rotation = rotation;
		beam.transform.localScale = new Vector3(0.05f, 1.0f, 0.05f);

		Shader selfIllumin = Shader.Find("Self-Illumin/Diffuse");
		if(selfIllumin != null) {
			beam.renderer.material.shader = selfIllumin;
			beam.renderer.material.color = color;
		}

		if(beam.collider != null) {
			beam.collider.isTrigger = true;
		}

		beam.AddComponent<LazerBeam>();

		Rigidbody body = beam.AddComponent<Rigidbody>();
		if(body != null) {
			body.velocity = beam.transform.up*BEAM_SPEED;
			body.useGravity = false;
			body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}

		return beam;
	}

	public void Start() {
		StartCoroutine("DeleteTime");
	}

	private IEnumerator DeleteTime() {
		yield return new WaitForSeconds(LIFETIME_SECONDS);
		Destroy(gameObject);
	}

	public void FixedUpdate() {
		// TODO: {DETAILS}
		//       Add length when created/collided so it looks like it is growing/shrinking instead
		//       of just appearing at full length!
	}

	void OnTriggerEnter(Collider other) {
		other.SendMessage("LazerHit", SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
