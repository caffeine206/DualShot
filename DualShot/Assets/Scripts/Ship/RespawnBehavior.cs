/*
 * John Louie
 * 
 * Controls the respawn and reset behavior of the ships.
*/

using UnityEngine;
using System.Collections;

public class RespawnBehavior : MonoBehaviour {

	private GameObject respawnParticle;
	private GameObject pauseCamera;
	private CountdownTimer count;
	private const float invulnTime = 5f;
	private bool isPaused = false;
	
	void Start () {
		if (respawnParticle == null) {
			respawnParticle = Resources.Load("Prefabs/RespawnParticle") as GameObject;
		}

		pauseCamera = GameObject.Find("PauseMenuCamera");
		pauseCamera.gameObject.SetActive(false);

		count = GameObject.Find("Countdown").GetComponent<CountdownTimer>();

		Time.timeScale = 0f;
	}
	
	void Update () {
		Reset();
		
		if (Input.GetKeyDown(KeyCode.Escape) && !count.GetIsCounting()) {
			Pause();
		}
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
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel(1);
		}
	}
	#endregion

	#region Pause the game
	public void Pause() {
		if (!isPaused) {
			Time.timeScale = 0f;
			isPaused = true;
			pauseCamera.gameObject.SetActive(true);
		} else if (isPaused) {
			Time.timeScale = 1f;
			isPaused = false;
			pauseCamera.gameObject.SetActive(false);
		}
	}
	#endregion

	#region Check to see if game is paused
	public bool GameIsPaused() {
		return isPaused;
	}
	#endregion
}
