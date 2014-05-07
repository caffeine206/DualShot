/*
 * John Louie
 * Sung
 * CSS385
 * A ship class.
*/

using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	private const float Speed = 20f;
	private const float RotateSpeed = 120f;
	private const float FireRate = 1f;
	private float LastTimeFired = -1f;
	public int eggsOnScreen = 0;
	private float worldHeight;
	private float worldWidth;
	//The hero is a cube! Height and width are equal.
	private float heroWidth;
	//private ParticleSystem charge = null;
	private bool isCharging = false;
	private GameObject explosion;

	void Start () {
		/*
		worldHeight = Camera.main.orthographicSize;
		worldWidth = worldHeight * Camera.main.aspect;
		heroWidth = transform.lossyScale.x;
		*/
		
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosion") as GameObject;
		}
	}
	
	void Update () {
		movement();
		orbBlaster();
		StartCoroutine("charging");
		selfDestruct();
		//collision();
		//fireProjectile();
	}

	//The hero's movement, moves 20 units/second
	private void movement() {
		//GetAxisRaw removes the speed up and slow down of the hero's movement,
		//makes movement more precise
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

	/*
	private void createCharge() {
		if (Input.GetButtonDown("Fire1")) {
			if (isCharging == false) {
				isCharging = true;
				ParticleSystem c = Instantiate(charge) as ParticleSystem;
				StartCoroutine(charging(c));
			}
		} else if (Input.GetButtonUp("Fire1")) {
			isCharging = false;
		}
	}

	IEnumerator charging(ParticleSystem theCharge) {
		theCharge.startSize = 1f;
		yield return new WaitForSeconds(5f);
		theCharge.startSize = 2f;
		yield return new WaitForSeconds(5f);
		theCharge.startSize = 3f;
	}
	*/
	/*
	private void collision() {
		if (transform.position.y > worldHeight - heroWidth / 2) {
			transform.position -= new Vector3(0f, transform.position.y - (worldHeight - heroWidth / 2), 0f);
		}
		
		if (transform.position.y < -(worldHeight - heroWidth / 2)) {
			transform.position -= new Vector3(0f, transform.position.y + (worldHeight - heroWidth / 2), 0f);
		}
		
		if (transform.position.x > worldWidth - heroWidth / 2) {
			transform.position -= new Vector3(transform.position.x - (worldWidth - heroWidth / 2), 0f);
		}

		if (transform.position.x < -(worldWidth - heroWidth / 2)) {
			transform.position -= new Vector3(transform.position.x + (worldWidth - heroWidth / 2), 0f);
		}
	}

	private void fireProjectile() {
		if (Input.GetAxisRaw("Fire1") > 0f && (Time.realtimeSinceStartup - LastTimeFired) > FireRate) {
			GameObject p = Instantiate(projectile) as GameObject;
			Projectile proj = p.GetComponent<Projectile>();
			if (proj != null) {
				p.transform.position = transform.position;
				proj.SetForwardDirection(transform.up);
			}
			eggsOnScreen++;
			LastTimeFired = Time.realtimeSinceStartup;
		}
	}
	*/
}
