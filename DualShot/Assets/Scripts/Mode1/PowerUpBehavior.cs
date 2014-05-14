using UnityEngine;
using System.Collections;

public class PowerUpBehavior : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		DestroyAllPowerups();
	}

	private void DestroyAllPowerups() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Destroy(gameObject);
		}
	}
}
