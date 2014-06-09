using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	#region Asteroid Spawner
	// Vars for Asteroid Spawning
	public float mSpawnTime = 12f;
	//protected float mMinSpawnTime = 3f;
	public int mMaxOrbs = 30;
	public int mSpawnNum = 6;
	public float mSpawnMinSize = 1f;
	public float mSpawnMaxSize = 15f;
	public float mSpawnSpread = 20f;
	public float mSpawnSpeed = 30f;
	public float mSpawnStagger = 10f;
	
	public float mRampInterval = 20f;
	protected float mlastRamp;
	//protected float mTimeInterval = 0.25f;
	protected float mMassInterval = 3f;
	
	public GameObject[] mOrb;
	//public GUIText mEcho = null;
	protected float mMass = 1f;
	protected float mVelocity = 50f;
	
	public int mCurOrbs;
	protected float mLastSpawn = 0f; // The last time that orbs were spawned
	#endregion
	
	protected RespawnBehavior pause = null;
	
	// Use this for initialization
	void Start () {

        mOrb = new GameObject[5];

        if (mOrb[0] == null)
        {
            mOrb[0] = (GameObject)Resources.Load("Prefabs/Orb1");
        }
        if (mOrb[1] == null)
        {
            mOrb[1] = (GameObject)Resources.Load("Prefabs/Orb2");
        }
        if (mOrb[2] == null)
        {
            mOrb[2] = (GameObject)Resources.Load("Prefabs/Orb3");
        }
        if (mOrb[3] == null)
        {
            mOrb[3] = (GameObject)Resources.Load("Prefabs/Orb4");
        }
        if (mOrb[4] == null)
        {
            mOrb[4] = (GameObject)Resources.Load("Prefabs/Orb");
        }
		if (pause == null) {
			pause = GetComponent<RespawnBehavior>();
		}
		
		mlastRamp = Time.realtimeSinceStartup + 4f;
		
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
		if (pause.GameIsPaused()) {
			mlastRamp += Time.time;
		}
		if ( Time.realtimeSinceStartup - mlastRamp > mRampInterval ) {
			mlastRamp = Time.realtimeSinceStartup;
			/*if ( mSpawnTime > mMinSpawnTime ) { 
				mSpawnTime -= mTimeInterval;
				}*/
			mSpawnMinSize += mMassInterval;
            mSpawnMaxSize += mMassInterval;
            mSpawnStagger += mMassInterval;
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
		
		if ( mCurOrbs < 8) 
			SpawnOrbs();
		#endregion
	}
	#region Asteroid Spawner
	
	public int Orbs {
		get { return mCurOrbs; }
		set { mCurOrbs = value; }
	}
	
	protected virtual void SpawnOrbs() {
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
	
	protected void ThrowOrb(Vector2 pos, Vector2 dir, float mass) {
        float rand = Random.Range(0f, 100f);
        GameObject e;
        if (rand <= 25f)
        {
            e = (GameObject)Instantiate(mOrb[0]);
            e.GetComponent<OrbBehavior>().mObject = mOrb[0];
        }
        else if (rand <= 50f)
        {
            e = (GameObject)Instantiate(mOrb[1]);
            e.GetComponent<OrbBehavior>().mObject = mOrb[1];
        }
        else if (rand <= 75f)
        {
            e = (GameObject)Instantiate(mOrb[2]);
            e.GetComponent<OrbBehavior>().mObject = mOrb[2];
        }
        else if (rand <= 99f)
        {
            e = (GameObject)Instantiate(mOrb[3]);
            e.GetComponent<OrbBehavior>().mObject = mOrb[3];
        }
        else
        {
            e = (GameObject)Instantiate(mOrb[4]);
            e.GetComponent<OrbBehavior>().mObject = mOrb[4];
        }
		e.rigidbody2D.mass = mass;
		e.transform.position = pos;
		e.rigidbody2D.velocity = dir;
		e.transform.up = dir.normalized;
		e.transform.localScale = mass * Vector2.one;
		e.collider2D.isTrigger = true;
		e.GetComponent<OrbBehavior>().incoming = true;
	}
	
	#endregion
	
}
