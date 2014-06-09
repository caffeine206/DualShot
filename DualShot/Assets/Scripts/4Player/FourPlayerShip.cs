using UnityEngine;
using System.Collections;

public class FourPlayerShip : Ship {
	
	void Start () {
		// Initiate ship death and respawn
		if (explosion == null) {
			explosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
		if (pause == null) {
			pause = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		}
		
		fire = GetComponent<Fire>();
		
		float sizeY = 65f;
		
		if (gameObject.name == "OrangeShip") {
			startLocation = new Vector3(-sizeY, 0f, 0f);
		} else if (gameObject.name == "BlueShip") {
			startLocation = new Vector3(sizeY, 0f, 0f);
		} else if (gameObject.name == "ChartreuseShip") {
			startLocation = new Vector3(sizeY, 0f, 0f);
		} else if (gameObject.name == "PeriwinkleShip") {
			startLocation = new Vector3(-sizeY, 0f, 0f);
		}
		
		startLocation.y = transform.position.y;
		transform.position = startLocation;
		
		//mBackground = (AudioClip)Resources.Load("Sounds/DualShotGameplay");
		//Play(mBackground, 1f, 1);
		
		respawn = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		count = GameObject.Find("Countdown").GetComponent<CountdownTimer>();
		GameObject.Find("GameManager").GetComponent<VersusWorldBehavior>().setupShip(this);
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

		if (mSpikeUp == true) {
			if (Time.realtimeSinceStartup - kSpikeBegin > kSpikeEnd) {
				mSpikeUp = false;
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
		
		//No longer necessary. Maybe.
		//DieCheck(); // Check if ship is dead
		VersusWorldBehavior world = GameObject.Find("GameManager").GetComponent<VersusWorldBehavior>();
		world.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		if (isController == false && !respawn.GameIsPaused() && !count.GetIsCounting()) {
			// Ship mouse aim
			mousedir = world.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			mousedir.Normalize();
			transform.up = mousedir;
			
			if (Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f ||
			    Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f) {
				Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				rigidbody2D.AddForce (move.normalized * kHeroSpeed);
			}
			
			/*
			if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f ||
				Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f) {
				Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				rigidbody2D.AddForce(move.normalized * kHeroSpeed);
			}
			*/
			//Enable with GetAxisRaw instead of GetAxis for the movement for exact controls.
			/*
			if (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f) {
				rigidbody2D.velocity = Vector3.zero;
				rigidbody2D.angularVelocity = 0f;
			}
			*/
			
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
			if (mSpikeUp == true) {
				Destroy(other.gameObject);
				VersusAsteroidSpawner astroidSpawner = GameObject.Find("GameManager").GetComponent<VersusAsteroidSpawner>();
				astroidSpawner.mCurOrbs--;
			}
		}
		
		if (other.gameObject.name == "PowerUp" || other.gameObject.name == "PowerUp(Clone)") {
			Destroy(other.gameObject);
			
			powerLevel++;
			
			if (powerLevel > 3) {
				powerLevel = 3;
			}
			GameObject pickup = Instantiate(powerupPickup) as GameObject;
			pickup.transform.position = transform.position;
			pickup.particleEmitter.Emit(150);
		}
		
		if (other.gameObject.name == "SpeedUp" || other.gameObject.name == "SpeedUp(Clone)") {
			Destroy(other.gameObject);
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
			Destroy(other.gameObject);
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
			Destroy(other.gameObject);
			kSpikeBegin = Time.realtimeSinceStartup;
			mSpikeUp = true;
		}
	}
	#endregion
	
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
	
	
	#region Shotgun blast single/charge fire support
	
	IEnumerator ShotgunBlastStallTime() {
		hasFired = true;
		yield return new WaitForSeconds(kShotgunBlastDisplacementInterval);
		if (powerLevel >= 2) {
			fire.FireShotgunBlast(this.gameObject, powerLevel, mGrowUp);
		}
		yield return new WaitForSeconds(kShotgunBlastDisplacementInterval);
		if (powerLevel >= 3) {
			fire.FireShotgunBlast(this.gameObject, powerLevel, mGrowUp);
		}
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
	
	#endregion
}
