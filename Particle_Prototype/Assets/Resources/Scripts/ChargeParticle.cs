using UnityEngine;
using System.Collections;

public class ChargeParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Fire1")) {
			Destroy(gameObject);
		}
	}
}
