using UnityEngine;
using System.Collections;

public class Level1GameState : BoundsControl {
	

	#region  support runtime enemy creation..
	private float mPreEnergyOrbSpawnTime = -1f; // 
	private const int kStartEnemies = 1;
	private const float kEnergyOrbSpawnInterval = 3.0f; // in seconds
	public GameObject mEnergyOrbToSpawn = null;
	#endregion

	
	// Use this for initialization
	void Start () {
		mMainCamera = Camera.main;
		mWorldBound = new Bounds (Vector3.zero, Vector3.one);
		UpdateWorldBound ();

		#region initialize enemy spawning
		if (null == mEnergyOrbToSpawn) 
			mEnergyOrbToSpawn = Resources.Load("Prefabs/EnergyOrb") as GameObject;
		for (int i = 0; i < kStartEnemies; i++) {
			GameObject e = (GameObject) Instantiate(mEnergyOrbToSpawn);
			e.transform.position = new Vector3((Random.value * mWorldMax.x) + (Random.value * -mWorldMax.x), 
			                                   (Random.value * mWorldMax.y) + (Random.value * -mWorldMax.y), 0f);
		}
		#endregion
	}

	void Update () {
		// GameObject echoObject = GameObject.Find("EchoText");
		// GUIText gui = echoObject.GetComponent<GUIText>();
		
		if (Input.GetKey (KeyCode.Escape)) {
			MenuState.TheGameState.SetCurrentLevel("Menu");
			MenuState.TheGameState.PrintCurrentLevel();
			Application.LoadLevel("Menu");
		}
	}
	

	#region enemy spawning support
	private void SpawnAnEnergyOrb()
	{
		if ((Time.realtimeSinceStartup - mPreEnergyOrbSpawnTime) > kEnergyOrbSpawnInterval) {
			GameObject e = (GameObject) Instantiate(mEnergyOrbToSpawn);
			e.transform.position = new Vector3((Random.value * mWorldMax.x) + (Random.value * -mWorldMax.x), 
			                                   (Random.value * mWorldMax.y) + (Random.value * -mWorldMax.y), 0f);
			mPreEnergyOrbSpawnTime = Time.realtimeSinceStartup;
		}
	}
	#endregion

}
