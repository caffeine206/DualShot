﻿using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour {
	
	#region PublicVar
	// the minmum mass for the 3 sizes of asteroids
	public float kSize1 = 1f;
	public float kSize2 = 5f;
	public float kSize3 = 10f;
	public float kScale = 1f; // the constant for determining the diameter, might be Pi 

	public float kExplodeForce = 15f;
	public float mInvulTime = 1f;
	public float health = 100.0f;
	public const int kPieces = 2;
	#endregion
	
	#region PrivateVar
    private float powerUpDroprate;
	// Use this for initialization
	public GameObject mObject = null; // The prefab of this object.
	private AsteroidSpawner mWorld = null;
	private GameObject mPowerUp = null;
	private GameObject mSpeedUp = null;
	private GameObject mGrowUp = null;
	private GameObject mSpikeUp = null;
	private GameObject explosion = null;
    
	
	private float mSpawnTime;
	protected bool mInvul;
	public bool incoming = false;
	
	

    private PlaySound playme;       // For initiation of playing sounds
    //private AudioClip mHit;
    private AudioClip mHitLow;
    /*private AudioClip mHitLowMid;
    private AudioClip mHitMid;
    private AudioClip mHitMidHigh;*/
	
	#endregion

	void Start () {
		// Get Prefab
        WorldBehavior world = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
        if (world.mode == 1)
        {
            powerUpDroprate = 0.13f;
        }
        else if (world.mode == 2)
        {
            powerUpDroprate = 0.09f;
        }
        else if (world.mode == 3)
        {
            powerUpDroprate = 0.10f;
        }
		
		if (mWorld == null) {
			mWorld = GameObject.Find("GameManager").GetComponent<AsteroidSpawner>();
		}
		if (mPowerUp == null) {
			mPowerUp = (GameObject) Resources.Load ("Prefabs/PowerUp");
		}
		if (mSpeedUp == null) {
			mSpeedUp = (GameObject) Resources.Load ("Prefabs/SpeedUp");
		}
		if (mGrowUp == null) {
			mGrowUp = (GameObject) Resources.Load ("Prefabs/GrowUp");
		}
		if (mSpikeUp == null) {
			mSpikeUp = (GameObject) Resources.Load ("Prefabs/SpikeUp");
		}
		
		// Set mass and adjust the scale to match
		float mass = rigidbody2D.mass;
		float diameter = Mathf.Sqrt(mass) * kScale;
		transform.localScale = new Vector3(diameter, diameter);
		mWorld.Orbs++;
			
		
		mInvul = true;
		mSpawnTime = Time.realtimeSinceStartup;

        // Orb collide sounds
        //mHit = (AudioClip)Resources.Load("Sounds/OrbCollide");                  // Orb colliding sound (original)
        mHitLow = (AudioClip)Resources.Load("Sounds/energy orb low");           // Orb colliding sound (Low)
        /*mHitLowMid = (AudioClip)Resources.Load("Sounds/energy orb low mid");    // Orb colliding sound (Low-Mid)
        mHitMid = (AudioClip)Resources.Load("Sounds/energy orb mid");           // Orb colliding sound (Mid)
        mHitMidHigh = (AudioClip)Resources.Load("Sounds/energy orb mid high");  // Orb colliding sound (Mid-High)

		explosion = Resources.Load("Prefabs/OrbExplosion") as GameObject;*/

		if (explosion == null) {
			explosion = Resources.Load("Prefabs/OrbExplosion") as GameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( mInvul && Time.realtimeSinceStartup - mSpawnTime > mInvulTime )
		{
			mInvul = false;
		}
	}

	private void explode(GameObject other) {
		if (rigidbody2D.mass > kSize3) {
			smash(kSize2, kSize3, 2, other);
		} else if (rigidbody2D.mass > kSize2) {
			smash(kSize1, kSize2, 2, other);
		} else {
			Destroy(this.gameObject);
			mWorld.Orbs--;
		}
	}
	
	private void smash(float minSize, float maxSize, int pieces, GameObject other) {
		// get a new mass for the object
		// This is a steaming mess that I need to clean up somehow
		
		float newMass = rigidbody2D.mass / pieces;
		
		for ( int i = 0; i < pieces; ++i ) {
            GameObject e = (GameObject)Instantiate(mObject);
            e.GetComponent<OrbBehavior>().mObject = mObject;
			e.rigidbody2D.mass = newMass;// change mass
			
			float rotatepiece = -90 + (180 * i) / pieces;
			// the orbs each get an equal portion of a 180 arc the direction of the exploding orb
			
			e.transform.up = rigidbody2D.velocity; // Calculate new velocity
			e.transform.up.Normalize();
			e.transform.Rotate(0,0, rotatepiece + Random.Range (0f, 180f / pieces));

			ShotgunBlastBehavior shot = other.GetComponent<ShotgunBlastBehavior>();
			e.transform.position = transform.position + e.transform.up * Mathf.Sqrt(newMass) / kScale;
			e.rigidbody2D.velocity = (Vector2)(e.transform.up * kExplodeForce) + rigidbody2D.velocity;
			e.rigidbody2D.velocity += (Vector2) shot.transform.up * 30.0f;
			e.transform.up = rigidbody2D.velocity.normalized;


			e.rigidbody2D.AddForce(shot.transform.up * 1000.0f);
			
			e.transform.localScale = Vector2.one * newMass;
		}
		
		float random = Random.Range (0.0f, 1.0f); 
		
		if (random <= powerUpDroprate) {
			float powerupSelect = Random.Range(0.0f, 1.0f);

			if (powerupSelect > 0.0f && powerupSelect <= 0.25f) {
				GameObject powerUp = (GameObject) Instantiate(mPowerUp);
				powerUp.transform.position = transform.position;
			} else if (powerupSelect > 0.25f && powerupSelect <= 0.50f) {
				GameObject speedUp = (GameObject) Instantiate(mSpeedUp);
				speedUp.transform.position = transform.position;
			} else if (powerupSelect > 0.50f && powerupSelect <= 0.75f) {
				GameObject growUp = (GameObject) Instantiate(mGrowUp);
				growUp.transform.position = transform.position;
			} else {
				GameObject spikeUp = (GameObject)Instantiate(mSpikeUp);
				spikeUp.transform.position = transform.position;
			}
		}
		
		mWorld.Orbs--;
		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!mInvul) {
			if (other.gameObject.name == "OrangeRedShip" || other.gameObject.name == "PeriwinkleShip" 
			    || other.gameObject.name == "OrangeShip" || other.gameObject.name == "BlueShip" 
			    || other.gameObject.name == "ChartreuseShip") {
				ShieldSprite shield = other.gameObject.GetComponentInChildren<ShieldSprite>();
				shield.shieldFlash();
			}
			
			if (other.gameObject.name == "OrangeCity" || other.gameObject.name == "BlueCity"
			 || other.gameObject.name == "ChartreuseCity" || other.gameObject.name == "PeriwinkleCity") {
				//Orb explosion
				GameObject ex = Instantiate(explosion) as GameObject;
				ex.transform.position = transform.position;
			}

			#region Code for orbs colliding and destroying each other
			/*
			if (other.gameObject.name == "Orb(Clone)") {
				health -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);

                // Plays different collision sound based on mass size
                if (this.rigidbody2D.mass >= 10f || other.gameObject.rigidbody2D.mass >= 10f)
                { Play(mHitLowMid, 1f, 1); }
                else if (this.rigidbody2D.mass >= 5f || other.gameObject.rigidbody2D.mass >= 5f)
                { Play(mHitMid, 1f, 1); }
                else
                { Play(mHitMidHigh, 1f, 1); }
			}*/
			#endregion
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!mInvul && !incoming) {
			if (other.gameObject.name == "ShotgunBlastBlue(Clone)" || other.gameObject.name == "ShotgunBlastOrange(Clone)"
				|| other.gameObject.name == "ShotgunBlastMul(Clone)" || other.gameObject.name == "ShotgunBlastChar(Clone)") {
                health -= 5f;
                Destroy(other.gameObject);
				Play(mHitLow, 1f, 1);
				//Orb explosion
				GameObject ex = Instantiate(explosion) as GameObject;
				ex.transform.position = transform.position;

				if (health <= 0) {
					explode(other.gameObject);
				}
			}
		

			if (other.gameObject.name == "WaveBlastBlue(Clone)" || other.gameObject.name == "WaveBlastOrange(Clone)"
				|| other.gameObject.name == "WaveBlastMul(Clone)" || other.gameObject.name == "WaveBlastChar(Clone)") {
				//Debug.Log("WaveBlastPush");
				/*Vector2 dir = other.transform.position - transform.position;
				dir.Normalize();*/
				WaveBlastBehavior wave = other.GetComponent<WaveBlastBehavior>();
				rigidbody2D.AddForce(wave.mSpeed * wave.transform.up * wave.mForce);
				//other.gameObject.rigidbody2D.AddForce(mSpeed * transform.up * mForce);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		string otherName = other.gameObject.name;
		if (otherName == "Top" || otherName == "Bot" || otherName == "Left" || otherName == "Right") {
			collider2D.isTrigger = false;
			incoming = false;
		}
	}

    // Audio clip player
    public void Play(AudioClip clip, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
    }
}
