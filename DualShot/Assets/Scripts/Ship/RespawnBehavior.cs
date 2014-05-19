/*
 * John Louie
 * 
 * Controls the respawn and reset behavior of the ships.
*/

using UnityEngine;
using System.Collections;

public class RespawnBehavior : MonoBehaviour {

	private GameObject respawnParticle;
	//The length of invulnerability after respawn.
	private const float invulnTime = 5f;
	
	void Start () {
		if (respawnParticle == null) {
			respawnParticle = Resources.Load("Prefabs/RespawnParticle") as GameObject;
		}
	}
	
	void Update () {
		Reset();
	}

	#region Respawn support
	public void Respawn(Ship theShip) {
		StartCoroutine("RespawnHelper", theShip);
	}
	
	IEnumerator RespawnHelper(Ship theShip) {
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
	private IEnumerator Blink(Ship theShip) {
		float endTime = Time.realtimeSinceStartup + invulnTime;

		theShip.isInvulnerable = true;

		while (Time.realtimeSinceStartup < endTime) {
			theShip.renderer.enabled = false;
			yield return new WaitForSeconds(0.15f);
			theShip.renderer.enabled = true;
			yield return new WaitForSeconds(0.15f);
		}

		theShip.isInvulnerable = false;
	}
	#endregion

	#region Reset the current session
	private void Reset() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Application.LoadLevel(1);
		}
	}
	#endregion
}
