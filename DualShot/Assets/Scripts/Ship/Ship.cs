using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	//Keeps track of the max health of a player.
	private const float HEALTH = 20f;
	//Keeps track of the current health of the player. Set to public for testing purposes.
	private float currentHealth = HEALTH;
	//Used to instantiate a small explosion upon death.
	private GameObject explosion = null;
	//The start location of the ship. Currently set to the middle of the screen because
	//I'm not sure were we want to spawn the ship. Spawn points can be set in the inspector.
	public Vector3 startLocation = new Vector3(0f, 0f, 0f);
	//The middle of our world. Used to reorient the ships to their default direction after respawning.
	private Vector3 originOfWorld = new Vector3(0f, 0f, 0f);
	//Tracks the powerup level of the ship.
	public int powerLevel = 1;
	//Invulnerability flag.
	public bool isInvulnerable = true;
	public bool isController = false;

	public float kHeroSpeed = 140f;
	private Vector3 mClampedPosition;
	private Vector3 mNewDirection;
	private Vector3 mNewRotation;
	private Vector3 mLastDirection;
	private Vector3 mDefaultDirection;  // Only needed for right joystick control
	
	private PlaySound playme;       // For initiation of playing sounds
	private AudioClip mGunShot;
	private AudioClip mWave;
	private AudioClip mBackground;  // "music by audionautix.com"

    private AudioClip mShipHit;  // For audio clips
    private AudioClip mShipDead;

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;

	private float mAbsoluteWeaponInterval = .4f;
	private float mTimeOfLastCharge = 0.0f;
	
	private float mWaveBlastSpawnTime = -1.0f;
	private float kWaveBlastSpawnInterval = 0.32f;
	private float kWaveBlastChargeInterval = 0.4f;
	private float mWaveBlastChargeTime = -1.0f;
	private float kWaveTotalChargeTime = 0.0f;
	private float kWaveMaxChargeTime = 1.5f;
	
	private float mShotgunBlastSpawnTime = -1.0f;
	private float kShotgunBlastSpawnInterval = 0.6f;
	private float kShotgunBlastChargeInterval = 0.4f;
	private float mShotgunBlastChargeTime = -1.0f;
	private float kShotgunTotalChargeTime = 0.0f;
	private float kShotgunMaxChargeTime = 1.1f;
	
	private float kShotgunSpread = -10.0f;
	private int kShotgunShots = 5;
	private int kMaxShotgunShots = 8;

	void Start () {
		// Initiate ship death and respawn
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;

            mShipHit = (AudioClip)Resources.Load("Sounds/ShipHit");
            mShipDead = (AudioClip)Resources.Load("Sounds/ShipDead");
		}
		transform.position = startLocation;

		// Initiate Sounds
		mGunShot = (AudioClip)Resources.Load("Sounds/GunFire");
		mWave = (AudioClip)Resources.Load("Sounds/WaveFire");
		mBackground = (AudioClip)Resources.Load("Sounds/DeepSpaceY");
		Play (mBackground, 1f, 1);

		// Initiate weapons
		if (null == mWaveProjectile) {
			if (gameObject.name == "PeriwinkleShip")
				mWaveProjectile = Resources.Load ("Prefabs/WaveBlastBlue") as GameObject;
			else
				mWaveProjectile = Resources.Load ("Prefabs/WaveBlastOrange") as GameObject;
		}
		mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
		for (int i = 0; i <= kMaxShotgunShots; i++) {
			if (gameObject.name == "PeriwinkleShip")
				mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastBlue") as GameObject;
			else
				mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastOrange") as GameObject;
		}
	}
	
	void Update () {
		DieCheck(); // Check if ship is dead
		BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
		boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);

		if (isController == false) {
			// Ship mouse aim
			Vector2 mousedir;
			mousedir = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			mousedir.Normalize();
			transform.up = mousedir;

			if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f ||
			    Input.GetAxis ("Vertical") > 0.1f || Input.GetAxis ("Vertical") < -0.1f) {
				Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				rigidbody2D.AddForce ( move.normalized * kHeroSpeed);
			}

			if (Input.GetMouseButtonDown(0)) {
				FireWaveBlast(mousedir);
			}
			// Wave Blast Charge
			if (Input.GetMouseButtonUp (0) && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > (kWaveBlastChargeInterval)) 
			{
				FireChargedWaveBlast(mousedir);
			}
			
			// Shotgun Blast control
			if (Input.GetMouseButtonDown(1)) { // this is Right-Control
				FireShotgunBlast();
			}
			// Shotgun Blast charge control
			if (Input.GetMouseButtonUp(1) && (Time.realtimeSinceStartup - mShotgunBlastChargeTime) > kShotgunBlastChargeInterval) 
			{
				FireChargedShotgunBlast();
			}

		} else if (isController == true) {
			// Player movement
			Vector2 move = new Vector2(Input.GetAxis("P2Horizontal"), Input.GetAxis("P2Vertical"));
			rigidbody2D.AddForce(move.normalized * kHeroSpeed);
			
			// Right Stick Aim
			transform.up = new Vector3(Input.GetAxis("P2RHorz"), Input.GetAxis("P2RVert"), 0);
			mLastDirection = transform.up;
			if (Input.GetAxis("P2RHorz") < 0.3f && Input.GetAxis("P2RHorz") > -0.3f &&
			    Input.GetAxis("P2RVert") < 0.3f && Input.GetAxis("P2RVert") > -0.3f)
			{ 
				transform.up = mLastDirection.normalized;
			}

			// Weapon controls
			if (Input.GetButtonDown("P2Fire1"))
			{ // this is Left-Control
				FireWaveBlast(mLastDirection.normalized);
			}
			
			if (Input.GetButtonUp("P2Fire1") && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > kWaveBlastSpawnInterval)
			{
				FireChargedWaveBlast(mLastDirection.normalized);
			}

			if (Input.GetButtonDown("P2Fire2"))
			{ // this is Right-Control
				FireShotgunBlast();
			}
			
			if (Input.GetButtonUp("P2Fire2") && (Time.realtimeSinceStartup - mShotgunBlastChargeTime) > kShotgunBlastChargeInterval)
			{
				FireChargedShotgunBlast();
			}
		}

		Charging();
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

            Play(mShipDead, 1f, 1);
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


	
	#region Charge particle support
	private void Charging() {
		if (gameObject.name == "PeriwinkleShip") {
			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
				StartCoroutine("ChargeCoroutine");
			} else if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2")) {
				StopCharge();
			}
		} else if (gameObject.name == "OrangeRedShip") {
			if (Input.GetButtonDown("P2Fire1") || Input.GetButtonDown("P2Fire2")) {
				StartCoroutine("ChargeCoroutine");
			} else if (Input.GetButtonUp("P2Fire1") || Input.GetButtonUp("P2Fire2")) {
				StopCharge();
			}
		}
	}
	
	IEnumerator ChargeCoroutine() {
		Transform theCharge = transform.Find("Charge");
		
		yield return new WaitForSeconds(0.4f);
		theCharge.particleSystem.enableEmission = true;
		theCharge.particleSystem.startSize = 1.5f;
		yield return new WaitForSeconds(0.1f);
		theCharge.particleSystem.startSize = 2f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 2.5f;
		yield return new WaitForSeconds(0.2f);
		theCharge.particleSystem.startSize = 3f;
		yield return new WaitForSeconds(0.3f);
		theCharge.particleSystem.startSize = 3.5f;
	}

	private void StopCharge() {
		Transform theCharge = transform.Find("Charge");
		StopCoroutine("ChargeCoroutine");
		theCharge.particleSystem.startSize = 1.5f;
		theCharge.particleSystem.enableEmission = false;
	}

	#endregion

	public void FireWaveBlast(Vector2 mousedir) {
		mWaveBlastChargeTime = Time.realtimeSinceStartup;
		if (((Time.realtimeSinceStartup - mWaveBlastSpawnTime) > kWaveBlastSpawnInterval) &&
			(Time.realtimeSinceStartup - mShotgunBlastSpawnTime) > mAbsoluteWeaponInterval)
		{
			GameObject e = Instantiate(mWaveProjectile) as GameObject;
			WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
			
			mWaveBlastSpawnTime = Time.realtimeSinceStartup;
			if (null != waveBlast) {
				if (powerLevel > 1)
					waveBlast.SetPowerLevel(powerLevel);
				e.transform.position = transform.position;
				waveBlast.SetForwardDirection(mousedir);
			}
			Play(mWave, .5f, 1);
		}
	}

	public void FireChargedWaveBlast(Vector2 mousedir) {
		kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
		if (Time.realtimeSinceStartup - mTimeOfLastCharge > .1f) {
			if (kWaveTotalChargeTime > kWaveMaxChargeTime)
				kWaveTotalChargeTime = kWaveMaxChargeTime;
			
			GameObject e = Instantiate(mWaveProjectile) as GameObject;
			WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
			if (null != waveBlast) {
				if (powerLevel > 1)
					waveBlast.SetPowerLevel(powerLevel);
				waveBlast.mSpeed += (waveBlast.mSpeed * kWaveTotalChargeTime) / 2.0f;
				waveBlast.mForce += kWaveTotalChargeTime * 20.0f;
				e.transform.localScale += new Vector3(kWaveTotalChargeTime * 2, kWaveTotalChargeTime * 2, 0.0f);
				e.transform.position = transform.position;
				waveBlast.SetForwardDirection(mousedir);
				Play(mWave, 1f, 1);
			}
			mTimeOfLastCharge = Time.realtimeSinceStartup;
		}
	}

	public void FireShotgunBlast() {
		mShotgunBlastChargeTime = Time.realtimeSinceStartup;
		if (((Time.realtimeSinceStartup - mShotgunBlastSpawnTime) > kShotgunBlastSpawnInterval) &&
		(Time.realtimeSinceStartup - mWaveBlastSpawnTime) > mAbsoluteWeaponInterval) {
			mShotgunBlastSpawnTime = Time.realtimeSinceStartup;
			FireShotgun(kShotgunShots, kShotgunSpread);
		}
	}

	public void FireChargedShotgunBlast() {
		kShotgunTotalChargeTime = Time.realtimeSinceStartup - mShotgunBlastChargeTime;
		if (Time.realtimeSinceStartup - mTimeOfLastCharge > .1f) {
			if (kShotgunTotalChargeTime > kShotgunMaxChargeTime) {
				FireShotgun(9, -70.0f);
			} else if (kShotgunTotalChargeTime > .9f) {
				FireShotgun(8, -60.0f);
			} else if (kShotgunTotalChargeTime > .7f) {
				FireShotgun(7, -50.0f);
			} else if (kShotgunTotalChargeTime > .5f) {
				FireShotgun(6, -40.0f);
			} else {
				FireShotgun(5, -30.0f);
			} 
			mTimeOfLastCharge = Time.realtimeSinceStartup;
		}
	}

	// Shotgun firing
	private void FireShotgun(int shots, float spread) {
		for (int i = 0; i <= shots; i++) {
			GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
			ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();
			
			if (null != shotgunBlast) {
				if (powerLevel > 1)
					shotgunBlast.SetPowerLevel(powerLevel);
				e.transform.position = transform.position + transform.up * 12f;
				e.transform.up = transform.up;
				shotgunBlast.AddShotgunSpeed(rigidbody2D.velocity.magnitude);
				shotgunBlast.SetForwardDirection(e.transform.up);
				e.transform.Rotate(Vector3.forward, spread + (i * shots * 2));
			}
		}
		//playme.Play(mGunShot, 1f, 1);
		Play(mGunShot, 1f, 1);
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
