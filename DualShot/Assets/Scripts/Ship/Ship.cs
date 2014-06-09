using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	//Keeps track of the max health of a player.
	public string controller;
	public int player;
	protected const float HEALTH = 20f;
	//Keeps track of the current health of the player. Set to public for testing purposes.
	protected float currentHealth = HEALTH;
	//Used to instantiate a small explosion upon death.
	protected GameObject explosion = null;
	//The start location of the ship. Currently set to the middle of the screen because
	//I'm not sure were we want to spawn the ship. Spawn points can be set in the inspector.
	protected Vector3 startLocation = new Vector3(0f, 0f, 0f);
	//The middle of our world. Used to reorient the ships to their default direction after respawning.
	protected Vector3 originOfWorld = new Vector3(0f, 0f, 0f);

	//Tracks the powerup level of the ship.
	protected int powerLevel = 1;
	protected bool mSpeedUp = false;
	protected float kSpeedMass = 2.0f;
	protected float kSpeedBegin = 0.0f;
	protected float kSpeedEnd = 8.0f;

	protected float kDefaultMass = 4.0f;
	public bool mGrowUp = false;
	protected float mGrowScale = 2.5f;
	protected float kGrowSpeed = 6000f;
	protected float mGrowMass = 7.0f;
	protected float kGrowBegin = 0.0f;
	protected float kGrowEnd = 8.0f;

	protected bool spikeUp = false;
	protected float spikeBegin = 0.0f;
	protected float spikeEnd = 8.0f;

	//Invulnerability flag.
	public bool isInvulnerable = true;
	public bool isController = false;

	protected const float kDefaultHeroSpeed = 3000f;
	protected float kHeroSpeed = kDefaultHeroSpeed;
	protected float kSpeedHeroSpeed = 9000f;
	protected Vector3 mClampedPosition;
	protected Vector3 mNewDirection;
	protected Vector3 mNewRotation;

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;
	protected bool hasFired = false;
	protected bool isCharging = false;

	protected const float kWaveBlastDefaultSpawnInterval = 0.5f;
	protected float kWaveBlastSpawnInterval = kWaveBlastDefaultSpawnInterval; //0.32
	protected float kWaveBlastSpeedSpawnInterval = 0.15f;
	protected float kWaveBlastChargeInterval = .5f;
	protected float mWaveBlastChargeTime = -1.0f;
	protected float kWaveTotalChargeTime = 0.0f;
	protected float kWaveMaxChargeTime = 1.5f;
	
	protected const float kShotgunBlastDefaultSpawnInterval = 0.5f;
	protected float kShotgunBlastSpawnInterval = kShotgunBlastDefaultSpawnInterval;
	protected float kShotgunBlastSpeedSpawnInterval = 0.2f;
	protected float kShotgunBlastChargeInterval = 0.5f;
	protected float kShotgunPowerInterval = .2f;

	protected const int kMinShotgunShots = 5;
	protected const float kMinShotgunSpread = -20.0f;

	protected float kShotgunSpread = kMinShotgunSpread;
	protected int kShotgunShots = kMinShotgunShots;

	protected Fire fire;
	protected Vector2 mousedir;

	protected RespawnBehavior respawn;
	protected CountdownTimer count;

	protected GameObject powerupPickup = null;
	protected GameObject speedupPickup = null;
	protected GameObject speedupParticle = null;
	protected GameObject growupPickup = null;
	protected GameObject growupParticle = null;
	protected RespawnBehavior pause = null;

	void Start () {
		// Initiate ship death and respawn
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
		if (pause == null) {
			pause = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		}

		fire = GetComponent<Fire>();
		
		float sizeX = Camera.main.orthographicSize * Camera.main.aspect;

		if (gameObject.name == "OrangeRedShip") {
			startLocation -= new Vector3(sizeX - 50f, 0f, 0f);
		} 
		else if (gameObject.name == "PeriwinkleShip") {
			startLocation += new Vector3(sizeX - 50f, 0f, 0f);
		}
		
		startLocation.y = transform.position.y;
		transform.position = startLocation;
		
        //mBackground = (AudioClip)Resources.Load("Sounds/DualShotGameplay");
        //Play(mBackground, 1f, 1);

		respawn = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		count = GameObject.Find("Countdown").GetComponent<CountdownTimer>();
		GameObject.Find("GameManager").GetComponent<WorldBehavior>().setupShip(this);
		if (powerupPickup == null) {
			powerupPickup = Resources.Load("Prefabs/PowerupPickup") as GameObject;
		}

		if (speedupPickup == null) {
			speedupPickup = Resources.Load("Prefabs/SpeedupPickup") as GameObject;
		}

		if (growupPickup == null) {
			growupPickup = Resources.Load("Prefabs/GrowupPickup") as GameObject;
		}
	}
	
	void Update () {
	
		if (pause.GameIsPaused()) {
			kSpeedBegin += Time.time;
			kGrowBegin += Time.time;
			mWaveBlastChargeTime += Time.time;
		}	
		
		if (mSpeedUp == true) {
			if (speedupParticle != null) {
				speedupParticle.transform.position = transform.position;
			}
			if (Time.realtimeSinceStartup - kSpeedBegin > kSpeedEnd) {
				mSpeedUp = false;
				kHeroSpeed = kDefaultHeroSpeed;
				rigidbody2D.mass = kDefaultMass;
				kShotgunBlastSpawnInterval = kShotgunBlastDefaultSpawnInterval;
				kWaveBlastSpawnInterval = kWaveBlastDefaultSpawnInterval;
				Destroy(speedupParticle);
			}
			}

		if (mGrowUp == true) {
			if (growupParticle != null) {
				growupParticle.transform.position = transform.position;
			}
			if (Time.realtimeSinceStartup - kGrowBegin > kGrowEnd) {
				mGrowUp = false;
				kHeroSpeed = kDefaultHeroSpeed;
				transform.localScale -= new Vector3(mGrowScale, mGrowScale, 0.0f);
				Destroy(growupParticle);
				rigidbody2D.mass = kDefaultMass;
			}
		}

		if (spikeUp == true) {
			if (Time.realtimeSinceStartup - spikeBegin > spikeEnd) {
				spikeUp = false;
			}
		}

		//No longer necessary. Maybe.
		//DieCheck(); // Check if ship is dead
		WorldBehavior WorldBehavior = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
		WorldBehavior.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		
		if (isController == false && !respawn.GameIsPaused() && !count.GetIsCounting()) {
			// Ship mouse aim
			mousedir = WorldBehavior.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			mousedir.Normalize();
			transform.up = mousedir;
			
			if (Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f ||
			    Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f) {
				Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				rigidbody2D.AddForce (move.normalized * kHeroSpeed);
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

		} else if (isController == true && !respawn.GameIsPaused() && !count.GetIsCounting()) {
			// Player movement
			//Vector2 move = new Vector2(Input.GetAxis(controller + "Horizontal"), Input.GetAxis(controller + "Vertical"));
			Vector2 move = new Vector2(Input.GetAxisRaw(controller + "Horizontal"), Input.GetAxisRaw(controller + "Vertical"));
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
		#region Support for the ship taking damage from colliding with orbs
		if (other.gameObject.name == "Orb(Clone)" && !isInvulnerable) {
			currentHealth -= ((other.gameObject.rigidbody2D.velocity.magnitude * 
					other.gameObject.rigidbody2D.mass) / 100.0f);
		}
		#endregion

		if (other.gameObject.name == "Orb" || other.gameObject.name == "Orb(Clone)") {
			if (spikeUp == true) {
				Destroy(other.gameObject);
			}
		}

		if (other.gameObject.name == "PowerUp" || other.gameObject.name == "PowerUp(Clone)") {
			powerLevel++;
			
			if (powerLevel > 3) {
				powerLevel = 3;
			}

			GameObject pickup = Instantiate(powerupPickup) as GameObject;
			pickup.transform.position = transform.position;
			pickup.particleEmitter.Emit(150);
		}

		if (other.gameObject.name == "SpeedUp" || other.gameObject.name == "SpeedUp(Clone)") {
			kSpeedBegin = Time.realtimeSinceStartup;
			mSpeedUp = true;
			if (mGrowUp == false) {
				rigidbody2D.mass = kSpeedMass;
				kHeroSpeed = kSpeedHeroSpeed;
			}
			kShotgunBlastSpawnInterval = kShotgunBlastSpeedSpawnInterval;
			kWaveBlastSpawnInterval = kWaveBlastSpeedSpawnInterval;
			if (speedupParticle == null) {
				speedupParticle = Instantiate(speedupPickup) as GameObject;
			}
		}

		if (other.gameObject.name == "GrowUp" || other.gameObject.name == "GrowUp(Clone)") {
			kGrowBegin = Time.realtimeSinceStartup;
			kHeroSpeed = kGrowSpeed;
			if (mGrowUp == false)
				transform.localScale += new Vector3(mGrowScale, mGrowScale, 0.0f);
			rigidbody2D.mass = mGrowMass;
			mGrowUp = true;
			if (growupParticle == null) {
				growupParticle = Instantiate(growupPickup) as GameObject;
			}
		}

		if (other.gameObject.name == "SpikeUp" || other.gameObject.name == "SpikeUp(Clone)") {
			spikeBegin = Time.realtimeSinceStartup;
			spikeUp = true;
		}
	}
	#endregion

	#region Ship dies
	protected void DieCheck() {
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
	
	public void killShip () {
		GameObject e = Instantiate(explosion) as GameObject;
		
		e.transform.position = transform.position;
		
		Transform flames = transform.Find("ShipFlames");
		flames.particleSystem.Clear();
		
		Transform[] shipParts = GetComponentsInChildren<Transform>();
		foreach( Transform i in shipParts) {
			i.gameObject.SetActive(false);
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
	protected void StartWaveBlast() {
		if (!hasFired && !isCharging) {
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

	protected void FireChargedWaveBlast() {
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
	protected void StartShotgunBlast() {
		if (!hasFired && !isCharging) {
			if (!mGrowUp) {
				fire.FireShotgunBlast(this.gameObject, powerLevel, 12f);
			} else {
				fire.FireShotgunBlast(this.gameObject, powerLevel, 28f);
			}
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

	protected void FireChargedShotgunBlast() {
		if (isCharging) {
			if (!mGrowUp) {
				fire.FireShotgun(kShotgunShots, kShotgunSpread, this.gameObject, powerLevel, 12f);
			} else {
				fire.FireShotgun(kShotgunShots, kShotgunSpread, this.gameObject, powerLevel, 28f);
			}
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

	protected void StopChargeParticle() {
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
