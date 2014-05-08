/*
 * John Louie
 * Sung
 * CSS385
 * A ship class.
*/

//John was here.

using UnityEngine;
using System.Collections;

public class ParticleShip : MonoBehaviour {

	private const float Speed = 30f;
	private const float RotateSpeed = 120f;
	private const float FireRate = 1f;
	private float LastTimeFired = -1f;
	private bool isCharging = false;
	private GameObject explosion;

	void Start () {
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
	}
	
	void Update () {
		movement();
		orbBlaster();
		StartCoroutine("charging");
		selfDestruct();
	}

	private void movement() {
		transform.position += Input.GetAxisRaw("Vertical") * transform.up *
			(Speed * Time.smoothDeltaTime);
		transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (RotateSpeed * Time.smoothDeltaTime));
	}

	private void orbBlaster() {
		ParticleSystem orb = GameObject.Find("OrbBlaster").GetComponent<ParticleSystem>();

		if (Input.GetAxisRaw("Fire1") > 0f && (Time.realtimeSinceStartup - LastTimeFired) > FireRate && isCharging == false) {
			orb.Emit(5);
			LastTimeFired = Time.realtimeSinceStartup;
		} 
	}

	IEnumerator charging() {
		ParticleSystem theCharge = GameObject.Find("Charging").GetComponent<ParticleSystem>();
		if (Input.GetButtonDown("Fire1")) {
			isCharging = true;
			theCharge.enableEmission = true;
			theCharge.startSize = 1f;
			yield return new WaitForSeconds(1.5f);
			theCharge.startSize = 2f;
			yield return new WaitForSeconds(1.5f);
			theCharge.startSize = 3f;
		} else if (Input.GetButtonUp("Fire1")) {
			isCharging = false;
			StopCoroutine("charging");
			theCharge.startSize = 1f;
			theCharge.enableEmission = false;
		}
	}

	private void selfDestruct() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			GameObject e = Instantiate(explosion) as GameObject;
			e.transform.position = transform.position;
			transform.position = new Vector3(0f, 0f, 0f);
			gameObject.SetActive(false);
		}
	}
}
