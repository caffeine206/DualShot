using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	//Keeps track of the max health of a player.
	public string controller;
	private const float HEALTH = 20f;
	//Keeps track of the current health of the player. Set to public for testing purposes.
	private float currentHealth = HEALTH;
	//Used to instantiate a small explosion upon death.
	private GameObject explosion = null;
	//The start location of the ship. Currently set to the middle of the screen because
	//I'm not sure were we want to spawn the ship. Spawn points can be set in the inspector.
	private Vector3 startLocation = new Vector3(0f, 0f, 0f);
	//The middle of our world. Used to reorient the ships to their default direction after respawning.
	private Vector3 originOfWorld = new Vector3(0f, 0f, 0f);
	//Tracks the powerup level of the ship.
	public int powerLevel = 1;
	//Invulnerability flag.
	public bool isInvulnerable = true;
	public bool isController = false;

	private float kHeroSpeed = 2500f;
	private Vector3 mClampedPosition;
	private Vector3 mNewDirection;
	private Vector3 mNewRotation;
    //private AudioClip mBackground;  // "music by audionautix.com"

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;
	private bool hasFired = false;
	private bool isCharging = false;

	//private float mAbsoluteWeaponInterval = .4f;
	//private float mTimeOfLastCharge = 0.0f;
	
	//private float mWaveBlastSpawnTime = -0.0f;
	private float kWaveBlastSpawnInterval = 0.5f; //0.32
	private float kWaveBlastChargeInterval = .5f;
	private float mWaveBlastChargeTime = -1.0f;
	private float kWaveTotalChargeTime = 0.0f;
	private float kWaveMaxChargeTime = 1.5f;
	//private float mWaveBlastLastCharge = -1.0f;
	
	//private float mShotgunBlastSpawnTime = -1.0f;
	private float kShotgunBlastSpawnInterval = 0.8f;
	private float kShotgunBlastChargeInterval = 0.5f;
	//private float mShotgunBlastChargeTime = -1.0f;
	//private float kShotgunTotalChargeTime = 0.0f;
	//private float kShotgunMaxChargeTime = 1.1f;
	//private float mShotgunBlastLastCharge = -1.0f;
	private float kShotgunPowerInterval = .2f;

	private const int kMinShotgunShots = 5;
	private const float kMinShotgunSpread = -20.0f;

	private float kShotgunSpread = kMinShotgunSpread;
	private int kShotgunShots = kMinShotgunShots;
	//private int kMaxShotgunShots = 9;

	Fire fire;
	Vector2 mousedir;

	RespawnBehavior respawn;
	CountdownTimer count;

	void Start () {
		// Initiate ship death and respawn
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
		string[] joysticks = Input.GetJoystickNames();
		foreach( string i in joysticks ) {
			Debug.Log (i);
		}

		fire = GetComponent<Fire>();
		
		float sizeX = Camera.main.orthographicSize * Camera.main.aspect;

		if (gameObject.name == "OrangeRedShip") {
			startLocation -= new Vector3(sizeX - 50f, 0f, 0f);
		} 
		else if (gameObject.name == "PeriwinkleShip") {
			startLocation += new Vector3(sizeX - 50f, 0f, 0f);
		}
		
		transform.position = startLocation;
		
        //mBackground = (AudioClip)Resources.Load("Sounds/DualShotGameplay");
        //Play(mBackground, 1f, 1);

		respawn = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		count = GameObject.Find("Countdown").GetComponent<CountdownTimer>();
	}
	
	void Update () {

		//No longer necessary. Maybe.
		//DieCheck(); // Check if ship is dead
		WorldBehavior WorldBehavior = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
		WorldBehavior.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		
		if (isController == false && !respawn.GameIsPaused() && !count.GetIsCounting()) {
			// Ship mouse aim
			mousedir = WorldBehavior.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			mousedir.Normalize();
			transform.up = mousedir;

			if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f ||
			    Input.GetAxis ("Vertical") > 0.1f || Input.GetAxis ("Vertical") < -0.1f) {
				Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				rigidbody2D.AddForce ( move.normalized * kHeroSpeed);
			}

			// Wave Blast single click
			if (Input.GetButtonDown("Fire1")) {
				StartWaveBlast();
				StartCoroutine("WaveBlastChargeCoroutine");
				mWaveBlastChargeTime = Time.realtimeSinceStartup;
			}

			// Wave Blast Charge fire
			if (Input.GetButtonUp("Fire1")) {
				StopCoroutine("WaveBlastChargeCoroutine");
				StopChargeParticle();
				FireChargedWaveBlast();
			}

			// Shotgun Blast single click
			if (Input.GetButtonDown("Fire2")) {
				StartShotgunBlast();
				StartCoroutine("ShotgunBlastChargeCoroutine");
			}

			// Shotgun Blast charge control
			if (Input.GetButtonUp("Fire2")) {
				StopCoroutine("ShotgunBlastChargeCoroutine");
				StopChargeParticle();
				FireChargedShotgunBlast();
			}

		} else if (isController == true) {
			// Player movement
			Vector2 move = new Vector2(Input.GetAxis(controller + "Horizontal"), Input.GetAxis(controller + "Vertical"));
			rigidbody2D.AddForce(move.normalized * kHeroSpeed);
			
			// Right Stick Aim
			transform.up += new Vector3(Input.GetAxis(controller + "RHorz"), Input.GetAxis(controller + "RVert"), 0);			
			
			// Wave blast single click
			if (Input.GetButtonDown(controller + "Fire1")) {
				StartWaveBlast();
			    StartCoroutine("WaveBlastChargeCoroutine");
			    mWaveBlastChargeTime = Time.realtimeSinceStartup;
			}

			// Wave blast charge
			if (Input.GetButtonUp(controller + "Fire1")) {
				StopCoroutine("WaveBlastChargeCoroutine");
				StopChargeParticle();
				
				FireChargedWaveBlast();
			}

			// Shotgun single click
			if (Input.GetButtonDown(controller + "Fire2")) { // this is Right-Control
				StartShotgunBlast();
				StartCoroutine("ShotgunBlastChargeCoroutine");
			}

			// Shotgun charge
			if (Input.GetButtonUp(controller + "Fire2")) {
				StopCoroutine("ShotgunBlastChargeCoroutine");
				StopChargeParticle();
				FireChargedShotgunBlast();
			}
		}
	}

	#region Collision with orbs and pickup powerups
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "Orb(Clone)" && !isInvulnerable) {
			currentHealth -= ((other.gameObject.rigidbody2D.velocity.magnitude * 
					other.gameObject.rigidbody2D.mass) / 100.0f);
		}

		if (other.gameObject.name == "PowerUp" || other.gameObject.name == "PowerUp(Clone)") {
			Destroy(other.gameObject);
			
			powerLevel++;
			
			if (powerLevel > 3) {
				powerLevel = 3;
			}
		}
	}
	#endregion

	#region Ship dies
	private void DieCheck() {
		if (currentHealth <= 0f) {
			RespawnBehavior respawnControls = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
			GameObject e = Instantiate(explosion) as GameObject;
			
			e.transform.position = transform.position;
			
			transform.position = startLocation;
			//Doesn't point the ship to the center of the world. :(
			transform.up = originOfWorld;
			
			respawnControls.Respawn(this);
			
			Transform flames = transform.Find("ShipFlames");
			flames.particleSystem.Clear();
			
			gameObject.SetActive(false);

            //Play(mShipDead, 1f, 1);
		}
	}
	#endregion

	#region Reset the ship
	public void Reset() {
		transform.position = startLocation;
		transform.up = originOfWorld;
		RestoreHealth();
		rigidbody2D.velocity = Vector3.zero;
		Transform flames = transform.Find("ShipFlames");
		flames.particleSystem.Clear();
		Transform shield = transform.Find("Shield");
		renderer.enabled = true;
		shield.renderer.enabled = true;
		powerLevel = 0;
	}
	#endregion

	#region Small useful functions
	public void RestoreHealth() {
		currentHealth = HEALTH;
	}

	public void Suicide() {
		currentHealth = 0f;
	}

	public float GetCurrentHealth() {
		return currentHealth;
	}
	#endregion

	#region Wave blast single/charge fire support
	private void StartWaveBlast() {
		if (!hasFired) {
			if (isController == true)
				fire.FireWaveBlast(transform.up, this.gameObject, powerLevel);
			else
				fire.FireWaveBlast(mousedir, this.gameObject, powerLevel);
			StartCoroutine("WaveBlastStallTime");
		}
	}

	IEnumerator WaveBlastStallTime() {
		hasFired = true;
		yield return new WaitForSeconds(kWaveBlastSpawnInterval);
		hasFired = false;
	}

	IEnumerator WaveBlastChargeCoroutine() {
		if (!isCharging) {
			yield return new WaitForSeconds(kWaveBlastChargeInterval);
			isCharging = true;
			mWaveBlastChargeTime = Time.realtimeSinceStartup;
			StartCoroutine("ChargeParticleCoroutine");
		}
	}

	private void FireChargedWaveBlast() {
		if (isCharging) {
			kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
			if (kWaveTotalChargeTime > kWaveMaxChargeTime)
				kWaveTotalChargeTime = kWaveMaxChargeTime;
			if (isController == true)
				fire.FireChargedWaveBlast(transform.up, this.gameObject, powerLevel, kWaveTotalChargeTime);
			else
				fire.FireChargedWaveBlast(mousedir, this.gameObject, powerLevel, kWaveTotalChargeTime);
			isCharging = false;
			StartCoroutine("WaveBlastStallTime");
		}
	}
	#endregion

	#region Shotgun blast single/charge fire support
	private void StartShotgunBlast() {
		if (!hasFired) {
			fire.FireShotgunBlast(this.gameObject, powerLevel);
			StartCoroutine("ShotgunBlastStallTime");
		}
	}

	IEnumerator ShotgunBlastStallTime() {
		hasFired = true;
		yield return new WaitForSeconds(kShotgunBlastSpawnInterval);
		hasFired = false;
	}

	IEnumerator ShotgunBlastChargeCoroutine() {
		if (!isCharging) {
			yield return new WaitForSeconds(kShotgunBlastChargeInterval);
			isCharging = true;
			StartCoroutine("ChargeParticleCoroutine");
			yield return new WaitForSeconds(kShotgunPowerInterval);
			kShotgunShots++;
			kShotgunSpread = -35.0f;
			yield return new WaitForSeconds(kShotgunPowerInterval);
			kShotgunShots++;
			kShotgunSpread = -45.0f;
			yield return new WaitForSeconds(kShotgunPowerInterval);
			kShotgunShots++;
			kShotgunSpread = -70.0f;
			yield return new WaitForSeconds(kShotgunPowerInterval);
			kShotgunShots++;
			kShotgunSpread = -80.0f;
		}
	}

	private void FireChargedShotgunBlast() {
		if (isCharging) {
			fire.FireShotgun(kShotgunShots, kShotgunSpread, this.gameObject, powerLevel);
			isCharging = false;
			StartCoroutine("ShotgunBlastStallTime");
		}
		kShotgunShots = kMinShotgunShots;
		kShotgunSpread = kMinShotgunSpread;
	}
	#endregion

	#region Charge PARTICLE support
	IEnumerator ChargeParticleCoroutine() {
		Transform theCharge = transform.Find("Charge");

		theCharge.particleSystem.enableEmission = true;
		theCharge.particleSystem.startSize = 1.5f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 2f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 2.5f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 3f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 3.5f;
	}

	private void StopChargeParticle() {
		Transform theCharge = transform.Find("Charge");
		StopCoroutine("ChargeParticleCoroutine");
		theCharge.particleSystem.startSize = 1.5f;
		theCharge.particleSystem.enableEmission = false;
	}
	#endregion

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
