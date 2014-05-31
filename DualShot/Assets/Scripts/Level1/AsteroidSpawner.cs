﻿using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	#region Asteroid Spawner
	// Vars for Asteroid Spawning
	private float mSpawnTime = 12f;
	//private float mMinSpawnTime = 3f;
	private int mMaxOrbs = 20;
	public int mSpawnNum = 6;
	public float mSpawnMinSize = 1f;
	public float mSpawnMaxSize = 15f;
	public float mSpawnSpread = 20f;
	public float mSpawnSpeed = 30f;
	public float mSpawnStagger = 10f;
	
	private float mRampInterval = 15f;
	private float mlastRamp;
	//private float mTimeInterval = 0.25f;
	private float mMassInterval = 5f;
	
	public GameObject mOrb = null;
	//public GUIText mEcho = null;
	protected float mMass = 1f;
	protected float mVelocity = 50f;
	
	protected int mCurOrbs;
	protected float mLastSpawn = 0f; // The last time that orbs were spawned
	#endregion
	
	private RespawnBehavior pause = null;
	
	// Use this for initialization
	void Start () {
	
		if (mOrb == null) {
			mOrb = (GameObject) Resources.Load ("Prefabs/Orb");                
		}
		if (pause == null) {
			pause = GetComponent<RespawnBehavior>();
		}
		
		mlastRamp = Time.realtimeSinceStartup;
		
		SpawnOrbs();
	
	}
	
	// Update is called once per frame
	void Update () {
		#region Orb Spawner Logic
		//Disabled for demo.
		/*
		if ( Input.GetKeyDown(KeyCode.F2)) { // Manual Spawner
			SpawnOrbs();
		}
		*/

		// Automated spawner. Uses the metrics of time since last spawn and limits the number of orbs on the screen.
		 
		if ( Time.realtimeSinceStartup - mlastRamp > mRampInterval ) {
			mlastRamp = Time.realtimeSinceStartup;
			/*if ( mSpawnTime > mMinSpawnTime ) { 
				mSpawnTime -= mTimeInterval;
				}*/
			mSpawnMinSize += mMassInterval;
		}
		
		if ( Time.realtimeSinceStartup - mLastSpawn > mSpawnTime && mCurOrbs < mMaxOrbs && !pause.GameIsPaused() ) {
			SpawnOrbs();
			mLastSpawn = Time.realtimeSinceStartup;
			//Added this to increase wave frequency.
			//mSpawnTime = mSpawnTime - 2f;

			//if (mSpawnTime < 0f) {
			//	mSpawnTime = 0f;
			//}
		}
		#endregion
	}
	#region Asteroid Spawner
	
	public int Orbs {
		get { return mCurOrbs; }
		set { mCurOrbs = value; }
	}
	
	private void SpawnOrbs() {
		int topNum, botNum; // Number spawning from top/bottom
		topNum = mSpawnNum/2;
		botNum = mSpawnNum - topNum;
		WorldBehavior world = GetComponent<WorldBehavior>();
		
		for(int i = 0; i < topNum; ++i) {
			Vector2 spawnPoint = new Vector2( Random.Range(-mSpawnSpread, mSpawnSpread), world.WorldMax.y + (i + 1) * mSpawnStagger);
			Vector2 spawnDir = new Vector2( Random.Range (-mSpawnSpread / 2f, mSpawnSpread / 2f), -mSpawnSpeed);
			float spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}
		
		for(int i = 0; i < botNum; ++i) {
			Vector2 spawnPoint = new Vector2( Random.Range(-mSpawnSpread, mSpawnSpread), world.WorldMin.y - (i + 1) * mSpawnStagger);
			Vector2 spawnDir = new Vector2( Random.Range (-mSpawnSpread / 2f, mSpawnSpread / 2f), mSpawnSpeed);
			float spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}
		
		//Added
	}
	
	private void ThrowOrb(Vector2 pos, Vector2 dir, float mass) {
		GameObject e = (GameObject) Instantiate(mOrb);
		e.rigidbody2D.mass = mass;
		e.transform.position = pos;
		e.rigidbody2D.velocity = dir;
		e.transform.up = dir.normalized;
		e.transform.localScale = mass * Vector2.one;
		e.collider2D.isTrigger = true;
	}
	
	#endregion
	
}