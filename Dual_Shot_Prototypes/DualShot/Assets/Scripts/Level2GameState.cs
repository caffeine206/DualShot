using UnityEngine;
using System.Collections;

public class Level2GameState : LocalGameBehavior {
	
	
	#region  support runtime enemy creation
	// to support time ...
	private float mPreEnemySpawnTime = -1f; // 
	private const int kStartEnemies = 50;
	private const float kEnemySpawnInterval = 3.0f; // in seconds
	// spawning enemy ...
	public GameObject mEnemyToSpawn = null;
	#endregion
	
	
	// Use this for initialization
	void Start () {

		mMainCamera = Camera.main;
		mWorldBound = new Bounds (Vector3.zero, Vector3.one);
		UpdateWorldBound ();
		
		#region initialize enemy spawning
		if (null == mEnemyToSpawn) 
			mEnemyToSpawn = Resources.Load("Prefabs/Enemy") as GameObject;
		for (int i = 0; i < kStartEnemies; i++) {
			GameObject e = (GameObject) Instantiate(mEnemyToSpawn);
			e.transform.position = new Vector3((Random.value * mWorldMax.x) + (Random.value * -mWorldMax.x), 
			                                   (Random.value * mWorldMax.y) + (Random.value * -mWorldMax.y), 0f);
		}
		#endregion
	}
	
	// Update is called once per frame
	void Update () {
		// To echo text to a defined GUIText
		//GameObject echoObject = GameObject.Find("EchoText");
		//GUIText gui = echoObject.GetComponent<GUIText>();
		
		if (Input.GetKey (KeyCode.Escape)) {
			MenuState.TheGameState.SetCurrentLevel("MenuLevel");
			MenuState.TheGameState.PrintCurrentLevel();
			Application.LoadLevel("MenuLevel");
		}
	}
	
	
	#region enemy spawning support
	private void SpawnAnEnemy()
	{
		if ((Time.realtimeSinceStartup - mPreEnemySpawnTime) > kEnemySpawnInterval) {
			GameObject e = (GameObject) Instantiate(mEnemyToSpawn);
			e.transform.position = new Vector3((Random.value * mWorldMax.x) + (Random.value * -mWorldMax.x), 
			                                   (Random.value * mWorldMax.y) + (Random.value * -mWorldMax.y), 0f);
			mPreEnemySpawnTime = Time.realtimeSinceStartup;
		}
	}
	#endregion
	
}
