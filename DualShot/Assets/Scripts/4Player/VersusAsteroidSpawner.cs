using UnityEngine;
using System.Collections;

public class VersusAsteroidSpawner : AsteroidSpawner {
	
/*
	
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
/*		
		if ( Time.realtimeSinceStartup - mlastRamp > mRampInterval ) {
			mlastRamp = Time.realtimeSinceStartup;
			/*if ( mSpawnTime > mMinSpawnTime ) { 
				mSpawnTime -= mTimeInterval;
				}*/
		/*	mSpawnMinSize += mMassInterval;
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
	
	/*
	#region Asteroid Spawner
	
	public int Orbs {
		get { return mCurOrbs; }
		set { mCurOrbs = value; }
	}
	*/
	protected override void SpawnOrbs() {
		int topNum, botNum; // Number spawning from top/bottom
		VersusWorldBehavior world = GetComponent<VersusWorldBehavior>();
		Vector2 spawnPoint, spawnDir;
		float spawnMass;
		
		for(int i = 0; i < mSpawnNum; ++i) {
			float spawnLoc = world.WorldMax.y + (i + 1) * mSpawnStagger;
			
			// Top
			spawnPoint = new Vector2( 0f, spawnLoc);
			spawnDir = new Vector2( Random.Range (-mSpawnSpread, mSpawnSpread), -1f).normalized * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Left
			spawnPoint = new Vector2( -spawnLoc, 0f);
			spawnDir = new Vector2( 1f, Random.Range (-mSpawnSpread, mSpawnSpread)).normalized * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Bot
			spawnPoint = new Vector2( 0f, -spawnLoc);
			spawnDir = new Vector2( Random.Range (-mSpawnSpread, mSpawnSpread), 1f).normalized * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Right
			spawnPoint = new Vector2( spawnLoc, 0f);
			spawnDir = new Vector2( -1f, Random.Range (-mSpawnSpread, mSpawnSpread)).normalized * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}
		
		//Added
	}
	
	
}
