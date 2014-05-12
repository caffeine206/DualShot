using UnityEngine;
using System.Collections;

public class BaseBehavior : MonoBehaviour {
	public float health = 1000.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0.0f) {

		}
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
			health -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);
		}
	}
}
