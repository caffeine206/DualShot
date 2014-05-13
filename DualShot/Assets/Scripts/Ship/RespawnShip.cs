﻿using UnityEngine;
using System.Collections;

public class RespawnShip : MonoBehaviour {

	//Keeps track of the max health of a player.
	private const float HEALTH = 20f;
	//Keeps track of the current health of the player. Set to public for testing purposes.
	private float currentHealth = HEALTH;
	//Used to instantiate a small explosion upon death.
	private GameObject explosion = null;
	//The start location of the ship. Currently set to the middle of the screen because
	//I'm not sure were we want to spawn the ship. Spawn points can be set in the inspector.
	public Vector3 startLocation = new Vector3(0f, 0f, 0f);
	//The middle of our world. Used to reorient the ships to their default direction after respawning.
	private Vector3 originOfWorld = new Vector3(0f, 0f, 0f);

	void Start () {
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}

		transform.position = startLocation;
	}
	
	void Update () {
		Die();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "Orb(Clone)") {
			currentHealth -= ((other.gameObject.rigidbody2D.velocity.magnitude * 
					other.gameObject.rigidbody2D.mass) / 100.0f);
		}
	}

	#region Ship dies
	private void Die() {
		if (currentHealth <= 0f) {
			RespawnBehavior respawnControls = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
			GameObject e = Instantiate(explosion) as GameObject;
			
			e.transform.position = transform.position;
			
			transform.position = startLocation;
			//Doesn't point the ship to the center of the world. :(
			transform.up = originOfWorld;
			
			respawnControls.Respawn(this);
			
			Transform flames = transform.Find("ShipFlames");
			flames.particleSystem.Clear();
			
			gameObject.SetActive(false);
		}
	}
	#endregion

	#region Reset the ship
	public void Reset() {
		transform.position = startLocation;
		transform.up = originOfWorld;
		RestoreHealth();
		rigidbody2D.velocity = Vector3.zero;
		Transform flames = transform.Find("ShipFlames");
		flames.particleSystem.Clear();
		Transform shield = transform.Find("Shield");
		renderer.enabled = true;
		shield.renderer.enabled = true;

		/*
		 * To do:
		 * Remove powerups
		*/
	}
	#endregion

	#region Small useful functions
	public void RestoreHealth() {
		currentHealth = HEALTH;
	}

	public void Suicide() {
		currentHealth = 0f;
	}

	public float GetCurrentHealth() {
		return currentHealth;
	}
	#endregion
}
