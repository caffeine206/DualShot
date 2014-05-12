/*
 * John Louie
 * 
 * Controls the respawn and reset behavior of the ships.
*/

using UnityEngine;
using System.Collections;

public class RespawnBehavior : MonoBehaviour {

	private GameObject respawnParticle;
	private RespawnShip periwinkle;
	private RespawnShip orangeRed;
	//The length of invulnerability after respawn.
	private const float invulnTime = 5f;
	
	void Start () {
		if (respawnParticle == null) {
			respawnParticle = Resources.Load("Prefabs/RespawnParticle") as GameObject;
		}

		periwinkle = GameObject.Find("PeriwinkleShip").GetComponent<RespawnShip>();
		orangeRed = GameObject.Find("OrangeRedShip").GetComponent<RespawnShip>();
	}
	
	void Update () {
		//For testing purposes.
		KillShip();
		Reset();
	}

	#region Respawn support
	public void Respawn(RespawnShip theShip) {
		StartCoroutine("RespawnHelper", theShip);
	}
	
	IEnumerator RespawnHelper(RespawnShip theShip) {
		if (theShip.GetCurrentHealth() <= 0f) {
			
			theShip.RestoreHealth();
			
			yield return new WaitForSeconds(3f);
			
			GameObject r = Instantiate(respawnParticle) as GameObject;
			r.transform.position = theShip.transform.position;
			
			yield return new WaitForSeconds(1f);
			
			theShip.gameObject.SetActive(true);

			StartCoroutine("Blink", theShip);
		} 
	}
	#endregion

	#region Makes the ships blink and makes them invulnerable
	IEnumerator Blink(RespawnShip theShip) {
		float endTime = Time.realtimeSinceStartup + invulnTime;
		Transform shield = theShip.transform.Find("Shield");

		//To do: Change damage flag to false.

		while (Time.time < endTime) {
			theShip.renderer.enabled = false;
			shield.renderer.enabled = false;
			yield return new WaitForSeconds(0.15f);
			theShip.renderer.enabled = true;
			shield.renderer.enabled = true;
			yield return new WaitForSeconds(0.15f);
		}

		//To do: Change damage flag to true.
	}
	#endregion

	#region Destroy the ships
	//For testing purposes.
	private void KillShip() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			periwinkle.Suicide();
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			orangeRed.Suicide();
		}
	}
	#endregion

	#region Reset the current session
	private void Reset() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			//OrbBehavior orbs = GameObject.Find("Orb").GetComponent<OrbBehavior>();

			periwinkle.gameObject.SetActive(true);
			orangeRed.gameObject.SetActive(true);
			
			StopCoroutine("RespawnHelper");
			StopCoroutine("Blink");
			
			periwinkle.Reset();
			orangeRed.Reset();

			//orbs.DestroyAllOrbs();
			
			/*
			 * To do:
			 * Restore city health
			 * Clean up orbs
			 * Reset orb spawn wave
			*/
		}
	}
	#endregion
}
