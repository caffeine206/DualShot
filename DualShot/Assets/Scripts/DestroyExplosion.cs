/*
 * John Louie
 * Sung
 * CSS385
 * 5-7-14
 * Attach this script to particle prefabs to delete them from memory after
 * five seconds.
 */

using UnityEngine;
using System.Collections;

public class DestroyExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		die();
	}

	private void die() {
		//Destroys this object after five seconds.
		Destroy(gameObject, 5);
	}
}
