using UnityEngine;
using System.Collections;

public class BaseBehavior : MonoBehaviour {
	private GameObject smallExplosion = null;
	private GameObject bigExplosion = null;
	private GameObject smallExplosion2 = null;
	private GameObject bigExplosion2 = null;
	
	public const float HEALTH = 1000f;
	public float currentHealth = HEALTH;
	
	public Camera mCamera = null;
	
	void Start () {
		if (mCamera == null) {
			mCamera = Camera.main;
		}
		Debug.Log(gameObject.name);
		float sizeX = mCamera.orthographicSize * mCamera.aspect;
		Debug.Log ("size: " + sizeX);
		if ( gameObject.name == "OrangeCity") {
			Debug.Log("Move Left");
			transform.position -= new Vector3(sizeX, 0);
		} else if (gameObject.name == "BlueCity"){
			transform.position += new Vector3(sizeX, 0);
			
		}

		if (smallExplosion == null) {
			smallExplosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}

		if (bigExplosion == null) {
			bigExplosion = Resources.Load("Prefabs/BigExplosionParticle") as GameObject;
		}

		if (smallExplosion2 == null) {
			smallExplosion2 = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}

		if (bigExplosion2 == null) {
			bigExplosion2 = Resources.Load("Prefabs/BigExplosionParticle") as GameObject;
		}
		
	}
	
	void Update () {
		Win();
		Reset();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "Orb(Clone)" || other.gameObject.name == "Orb") {
			/*
			Debug.Log("Base Health: " + health);
			Debug.Log("Astroid velocity: " + other.gameObject.rigidbody2D.velocity.magnitude);
			Debug.Log ("Astroid mass: " + other.gameObject.rigidbody2D.mass);
			Debug.Log ("Mass * Velocity = " + (other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass));
			Debug.Log (" Divide by 100 = " + ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f));
			*/
			currentHealth -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);
			Debug.Log("Base Health: " + currentHealth);
		}
	}

	private void Win() {
		if (currentHealth <= 0f) {
			currentHealth = HEALTH;
			StartCoroutine("EXPLOSIVE_VICTORY");
		}
	}

	//For testing purposes.
	IEnumerator EXPLOSIVE_VICTORY() {
		GameObject s = Instantiate(smallExplosion) as GameObject;
		s.transform.position = transform.position;
		yield return new WaitForSeconds(0.5f);
		GameObject b = Instantiate(bigExplosion) as GameObject;
		b.transform.position = transform.position;
		yield return new WaitForSeconds(1f);
		GameObject b2 = Instantiate(bigExplosion2) as GameObject;
		b2.transform.position = transform.position;
		yield return new WaitForSeconds(2f);
		GameObject s2 = Instantiate(smallExplosion2) as GameObject;
		s2.transform.position = transform.position;
	}

	private void Reset() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			currentHealth = HEALTH;
		}
	}
}
