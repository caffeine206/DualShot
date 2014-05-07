using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {

	private GameObject respawnParticle;
	Ship theShip;
	ParticleSystem theFlames;
	
	void Start () {
		if (respawnParticle == null) {
			respawnParticle = Resources.Load("Prefabs/Respawn") as GameObject;
		}

		theShip = GameObject.Find("Ship").GetComponent<Ship>();
		theFlames = GameObject.Find("ShipFlames").GetComponent<ParticleSystem>();
	}
	
	void Update () {
		StartCoroutine(respawn());
	}

	IEnumerator respawn() {
		if (Input.GetKeyDown(KeyCode.Alpha2) && theShip.gameObject.activeSelf == false) {
			GameObject r = Instantiate(respawnParticle) as GameObject;
			r.transform.position = theShip.transform.position;
			theShip.transform.up = new Vector3(0f, 0f, 0f);
			yield return new WaitForSeconds(1f);
			theShip.gameObject.SetActive(true);
			theFlames.particleSystem.Clear();
		}
	}
}
