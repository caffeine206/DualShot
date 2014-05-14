using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour {
	
	#region PublicVar
	// the minmum mass for the 3 sizes of asteroids
	public float kSize1 = 1f;
	public float kSize2 = 5;
	public float kSize3 = 10;
	public float kScale = 1f; // the constant for determining the diameter, might be Pi 

	public float kExplodeForce = 25f;
	public float mInvulTime = 1f;
	public float health = 100.0f;
	public const int kPieces = 2;
	#endregion
	
	#region PrivateVar
	// Use this for initialization
	private GameObject mObject = null; // The prefab of this object.
	private BoundsControl mWorld = null;
	private GameObject mPowerUp = null;
	
	private float mSpawnTime;
	private bool mInvul;
	
	#endregion

	void Start () {
		// Get Prefab
		if (mObject == null) {
			mObject = (GameObject) Resources.Load ("Prefabs/Orb");
		}
		if (mWorld == null) {
			mWorld = GameObject.Find("GameManager").GetComponent<BoundsControl>();
		}
		if (mPowerUp == null) {
			mPowerUp = (GameObject) Resources.Load ("Prefabs/PowerUp");
		}
		
		// Set mass and adjust the scale to match
		float mass = rigidbody2D.mass;
		float diameter = Mathf.Sqrt(mass) * kScale;
		transform.localScale = new Vector3(diameter, diameter);
			mWorld.Orbs++;
			
		
		mInvul = true;
		mSpawnTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (health <= 0) {
			explode();
		}
		if ( mInvul && Time.realtimeSinceStartup - mSpawnTime > mInvulTime )
		{
			mInvul = false;
		}

		DestroyAllOrbs();
	}

	private void explode() {
		if (rigidbody2D.mass > kSize3) {
			smash(kSize2, kSize3, 2);
		} else if (rigidbody2D.mass > kSize2) {
			smash(kSize1, kSize2, 2);
		} else {
			Destroy(this.gameObject);
			mWorld.Orbs--;

		}
	}
	
	private void smash(float minSize, float maxSize, int pieces) {
		// get a new mass for the object
		// This is a steaming mess that I need to clean up somehow
		
		float newMass = rigidbody2D.mass / pieces;
		
		for ( int i = 0; i < pieces; ++i ) {
			GameObject e = (GameObject) Instantiate(mObject);
			e.rigidbody2D.mass = newMass;// change mass
			
			float rotatepiece = -90 + (180 * i) / pieces;
			// the orbs each get an equal portion of a 180 arc the direction of the exploding orb
			
			e.transform.up = rigidbody2D.velocity; // Calculate new velocity
			e.transform.up.Normalize();
			e.transform.Rotate(0,0, rotatepiece + Random.Range (0f, 180f / pieces));
			
			e.transform.position = transform.position + e.transform.up * Mathf.Sqrt(newMass) / kScale;
			e.rigidbody2D.velocity = (Vector2)(e.transform.up * kExplodeForce) + rigidbody2D.velocity;
			e.transform.up = rigidbody2D.velocity.normalized;
			
			e.transform.localScale = Vector2.one * newMass;
		}
		

		float random = Random.Range ( 0.0f, 1.0f ); 
		if (random <= .01) { // 1% chance of power up spawning upon orb death
			GameObject powerUp = (GameObject) Instantiate(mPowerUp);
			powerUp.transform.position = transform.position;
		}

		Destroy(this.gameObject);
		mWorld.Orbs--;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!mInvul) {
			if (other.gameObject.name == "ShotgunBlastBlue(Clone)" || other.gameObject.name == "ShotgunBlastOrange(Clone)") {
				health -= 50.0f;
				Destroy(other.gameObject);
			}
			if (other.gameObject.name == "Orb(Clone)") {
				health -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);
			}
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		string otherName = other.gameObject.name;
		if (otherName == "Top" || otherName == "Bot") {
			collider2D.isTrigger = false;
		}
	}

	private void DestroyAllOrbs() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Destroy(this.gameObject);
		}
	
}

