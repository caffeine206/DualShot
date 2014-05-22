using UnityEngine;
using System.Collections;

public class WaveBlastBehavior : MonoBehaviour {
	
	public float mSpeed = 200f;
	public float mForce = 70f;
	private float kWaveLife = 0.33f;
	private float kWaveSpawnTime;
	private int mCurSrite = 0;
	
	private BaseSpriteManager mSpriteManager = null;

	void Start()
	{
		kWaveSpawnTime = Time.realtimeSinceStartup;
		
		if ( mSpriteManager == null ) {
			mSpriteManager = GetComponent<BaseSpriteManager>();
		}
	}

	// Update is called once per frame
	void Update () {
		float percentLife = 1 - ((Time.realtimeSinceStartup - kWaveSpawnTime) / kWaveLife);
		
//		Debug.Log (percentLife);
		if (percentLife < 0.95f && mCurSrite == 0) {
			mSpriteManager.nextSprite();
			mCurSrite++;
		} else if (percentLife < 0.90f && mCurSrite == 1) {
			mSpriteManager.nextSprite();
			mCurSrite++;
		} else if (percentLife < 0.8f && mCurSrite == 2) {
			mSpriteManager.nextSprite();
			mCurSrite++;
		} else if (percentLife < 0.2f && mCurSrite == 3) {
			mSpriteManager.nextSprite();
			mCurSrite++;
		} else if (percentLife < 0.1f && mCurSrite == 4) {
			mSpriteManager.nextSprite();
			mCurSrite++;
		}
	
		if ((Time.realtimeSinceStartup - kWaveSpawnTime) > kWaveLife) {
			Destroy(this.gameObject);
		}
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        //string level = MenuState.TheGameState.getLevel; // Why does my weapon care about my level?
		WorldBehavior WorldBehavior = GameObject.Find ("GameManager").GetComponent<WorldBehavior>();
		WorldBehavior.WorldBoundStatus status =
            WorldBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status == WorldBehavior.WorldBoundStatus.Outside) {
			Destroy(this.gameObject);
		}
	}
	
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Orb" || other.gameObject.name == "Orb(Clone)") {
			//Debug.Log("Push");
			/*Vector2 dir = other.transform.position - transform.position;
			dir.Normalize();*/
			other.rigidbody2D.AddForce(mSpeed * transform.up * mForce);
			//other.gameObject.rigidbody2D.AddForce(mSpeed * transform.up * mForce);
		}
	}

	public void SetPowerLevel(int level) {
		float increase = level * 1.0f;
		kWaveLife += increase / 8.0f;
		mForce += 20.0f;
		transform.localScale *= 1f + (0.25f * increase);
//		transform.localScale += new Vector3 (increase * 1.25f, increase * 1.25f, 0.0f);
	}
	
}
