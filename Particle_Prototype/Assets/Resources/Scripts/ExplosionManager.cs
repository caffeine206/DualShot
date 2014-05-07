using UnityEngine;
using System.Collections;

public class ExplosionManager : MonoBehaviour {

	private GameObject explosion = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		explosions();
	}

	private void explosions() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			explosion = Resources.Load("Prefabs/SmallExplosion") as GameObject;
			GameObject p = Instantiate(explosion) as GameObject;
		} else if (Input.GetKeyDown(KeyCode.W)) {
			explosion = Resources.Load("Prefabs/BigExplosion") as GameObject;
			GameObject p = Instantiate(explosion) as GameObject;
		}
	}
}
