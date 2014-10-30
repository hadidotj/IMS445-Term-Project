using UnityEngine;
using System.Collections;

public class LazerBeam : MonoBehaviour {

	public static readonly float LIFETIME_SECONDS = 5.0f;
	public static readonly float BEAM_SPEED = 100.0f;
	public static readonly Vector3 LAZER_OBJECT_SCALE = new Vector3(0.05f, 1.0f, 0.05f);
	public static readonly string TAG_NAME = "LazerBeam";
	public static readonly string TRIGGER_MESSAGE = "LazerBeamHit";

	/**
	 * Creates a new LazerBeam with the given position, rotation, color and owner
	 * @param position - Lazer starting position
	 * @param rotation - Lazer starting rotation
	 * @param color - Color of Lazer Beam
	 * @param ownerRoot - Owner of lazer (cannot hit self, teammate (if enabled), own base (if enabled), etc
	 */
	public static GameObject CreateLazerBeam(Vector3 position, Quaternion rotation, Color color, GameObject ownerRoot) {
		
		// Create a new "Lazer"
		GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		beam.name = "Lazer-Beam (" + beam.GetInstanceID() + ")";
		beam.tag = TAG_NAME;
	
		// Set the transform data
		beam.transform.position = position;
		beam.transform.rotation = rotation;
		beam.transform.localScale = LAZER_OBJECT_SCALE;

		// Add the selfIllumin shader and given color
		Shader selfIllumin = Shader.Find("Self-Illumin/Diffuse");
		if(selfIllumin != null) {
			beam.renderer.material.shader = selfIllumin;
			beam.renderer.material.color = color;
		}

		// Set the collider to a trigger
		if(beam.collider != null) {
			beam.collider.isTrigger = true;
		}

		// Add the Lazer script (this script) and set owner
		LazerBeam script = beam.AddComponent<LazerBeam>();
		if(script != null) {
			script.ownerRoot = ownerRoot;
		}

		// Add a rigidbody with no gravity and add a velocity for the speed.
		// A Rigidbody is required for the OnTriggerEnter messages to be called.
		// If the Lazer was not a rigidbody AND the beam it another non-rigidbody,
		// the OnTriggerEnter message is NOT called!
		Rigidbody body = beam.AddComponent<Rigidbody>();
		if(body != null) {
			body.velocity = beam.transform.up*BEAM_SPEED;
			body.useGravity = false;
			body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}

		return beam;
	}

	/**********************************************************
	 *            Lazer Beam Controller
	 *********************************************************/

	public GameObject ownerRoot;

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

	public void OnTriggerEnter(Collider other) {
		if(other.tag != TAG_NAME && other != ownerRoot && !other.transform.IsChildOf(ownerRoot.transform)) {
			if(Network.isServer) {
				other.SendMessage(TRIGGER_MESSAGE, ownerRoot, SendMessageOptions.DontRequireReceiver);
			}
			Destroy(gameObject);
		}
	}
}
