using UnityEngine;
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
		StartCoroutine("charging");
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

	#region Charge particle support
	/*
	 * This method is button dependent and will have to be stuck into the appropriate
	 * control scripts.
	*/
	IEnumerator charging() {
		Transform theCharge = transform.Find("Charge");
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
			yield return new WaitForSeconds(0.4f);
			theCharge.particleSystem.enableEmission = true;
			theCharge.particleSystem.startSize = 1.5f;
			yield return new WaitForSeconds(0.1f);
			theCharge.particleSystem.startSize = 2f;
			yield return new WaitForSeconds(0.2f);
			theCharge.particleSystem.startSize = 2.5f;
			yield return new WaitForSeconds(0.2f);
			theCharge.particleSystem.startSize = 3f;
			yield return new WaitForSeconds(0.3f);
			theCharge.particleSystem.startSize = 3.5f;
		} else if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2")) {
			StopCoroutine("charging");
			theCharge.particleSystem.startSize = 1.5f;
			theCharge.particleSystem.enableEmission = false;
		}
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
