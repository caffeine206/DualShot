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
		Destroy(gameObject, 5);
	}
}
