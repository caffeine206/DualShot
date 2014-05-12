using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour {
	
	#region PublicVar
	// the minmum mass for the 3 sizes of asteroids
	public float kSize1 = 1f;
	public float kSize2 = 5;
	public float kSize3 = 10;
	
	public float kExplodeForce = 25f;
	public float health = 100.0f;
	public const int kPieces = 2;
	#endregion
	
	#region PrivateVar
	private const float kScale = 1f; // the constant for determining the diameter, might be Pi
	// Use this for initialization
	private GameObject mObject = null; // The prefab of this object.
	private GameObject mPowerUp = null;
	#endregion

	void Start () {
		// Get Prefab
		if (mObject == null) {
			mObject = (GameObject) Resources.Load ("Prefabs/Orb");
		}

		if (mPowerUp == null) {
			mPowerUp = (GameObject) Resources.Load ("Prefabs/PowerUp");
		}
		
		// Set mass and adjust the scale to match
		float mass = rigidbody2D.mass;
		float diameter = Mathf.Sqrt(mass) / kScale;
		transform.localScale = new Vector3(diameter, diameter);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetButtonDown("Fire2")) {
			Debug.Log("Smash");
			--health;
		}
		*/
		if (health <= 0.0f) {
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
		
		// This is a steaming mess that I need to clean up somehow
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

		float random = Random.Range ( 0.0f, 1.0f ); 
		if (random <= .01) { // 1% chance of power up spawning upon orb death
			GameObject powerUp = (GameObject) Instantiate(mPowerUp);
			powerUp.transform.position = transform.position;
		}

		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "ShotgunBlastBlue(Clone)" || other.gameObject.name == "ShotgunBlastOrange(Clone)") {
			health -= 50.0f;
			Destroy(other.gameObject);
		}
		if (other.gameObject.name == "Orb(Clone)") {
			health -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);
		}
	}
}
