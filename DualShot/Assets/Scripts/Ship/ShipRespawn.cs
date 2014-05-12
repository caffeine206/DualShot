﻿/*
 * John Louie
 * 
 * Respawn particles for the ship. Attach this to the RespawnParticle
 * prefab.
*/

using UnityEngine;
using System.Collections;

public class ShipRespawn : MonoBehaviour {

	private GameObject respawnParticle;
	ParticleShip theShip;
	ParticleSystem theFlames;
	
	void Start () {
		if (respawnParticle == null) {
			respawnParticle = Resources.Load("Prefabs/RespawnParticle") as GameObject;
		}

		theShip = GameObject.Find("Ship").GetComponent<ParticleShip>();
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
