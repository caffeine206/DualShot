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
			
			// Top Left
			spawnPoint = new Vector2( -spawnLoc, spawnLoc);
			spawnDir = new Vector2( 1, -1) * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Top Right
			spawnPoint = new Vector2( spawnLoc, spawnLoc);
			spawnDir = new Vector2( -1, -1) * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Bot Left
			spawnPoint = new Vector2( -spawnLoc, -spawnLoc);
			spawnDir = new Vector2( 1, 1) * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
			
			// Bot Right
			spawnPoint = new Vector2( spawnLoc, -spawnLoc);
			spawnDir = new Vector2( -1, 1) * mSpawnSpeed;
			spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}
		
		//Added
	}
	
	protected override void ThrowOrb(Vector2 pos, Vector2 dir, float mass) {
		GameObject e = (GameObject) Instantiate(mOrb);
		e.rigidbody2D.mass = mass;
		e.transform.position = pos;
		e.rigidbody2D.velocity = dir;
		e.transform.up = dir.normalized;
		float rotate = Random.Range (-5f, 5f);
		e.transform.Rotate (0, 0, rotate);
		e.transform.localScale = mass * Vector2.one;
		e.collider2D.isTrigger = true;
	}
	
	
}
