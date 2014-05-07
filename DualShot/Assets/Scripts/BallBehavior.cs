using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {
	
	#region PublicVar
	public const float kSize1 = 1f;
	public const float kSize2 = 5;
	public const float kSize3 = 10;
	public float kExplodeForce = 100f;
	public const int kPieces = 2;
	#endregion
	
	#region PrivateVar
	private int health = 2;
	private const float kScale = 1f; // the constant for determining the diameter, might be Pi
	// Use this for initialization
	private GameObject mObject = null; // The prefab of this object.
	#endregion

	void Start () {
		if (mObject == null) {
			mObject = (GameObject) Resources.Load ("Prefab/Asteroid");
		}
		float mass = rigidbody2D.mass;
		float diameter = Mathf.Sqrt(mass) / kScale;
		transform.localScale = new Vector3(diameter, diameter);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire2")) {
			Debug.Log("Smash");
			--health;
		}
		if (health <= 0) {
			Debug.Log("Boom");
			explode();
		}
	}

	private void explode() {
		if (rigidbody2D.mass > kSize3) {
			smash(kSize2, kSize3);
		} else if (rigidbody2D.mass > kSize2) {
			smash(kSize1, kSize2);
		} else {
			Destroy(this.gameObject);
		}
	}
	
	private void smash(float minSize, float maxSize) {
		// get a new mass for the object
		float mass = rigidbody2D.mass;
		
		float newMass = Random.Range ( minSize, maxSize );
		mass -= newMass;
		GameObject e = (GameObject) Instantiate(mObject);
	
		float rotationAngle = (360f / kPieces);
		
		float rotate = rotationAngle / 2f;
		e.rigidbody2D.mass = newMass;// change mass
		
		e.transform.up = rigidbody2D.velocity; // Calculate new velocity
		e.transform.up.Normalize();
		e.transform.Rotate(0,0, rotate);
		
		e.transform.position = transform.position + e.transform.up * Mathf.Sqrt(newMass) / kScale;
		e.rigidbody2D.velocity = (Vector2)(e.transform.up * kExplodeForce) + rigidbody2D.velocity;
		e.transform.up = rigidbody2D.velocity.normalized;
		
		e = (GameObject) Instantiate(mObject);
		rotate += rotationAngle;
		
		e.rigidbody2D.mass = mass;// change mass
		
		e.transform.up = rigidbody2D.velocity; // Calculate new velocity
		e.transform.up.Normalize();
		e.transform.Rotate(0,0, rotate);
		
		e.transform.position = transform.position + e.transform.up * Mathf.Sqrt(newMass) / kScale;
		e.rigidbody2D.velocity = (Vector2)(e.transform.up * kExplodeForce) + rigidbody2D.velocity;
		e.transform.up = rigidbody2D.velocity.normalized;
		Destroy(this.gameObject);
	}
}
