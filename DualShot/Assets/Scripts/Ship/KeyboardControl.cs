/*
 * Controls the ship with a keyboard.
*/

using UnityEngine;	
using System.Collections;

public class KeyboardControl : MonoBehaviour {

	RespawnShip theShip;

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;

	public float kHeroSpeed = 140f;
	private Vector3 mClampedPosition;
	//private int theShip.powerLevel = 1;

	private float mWaveBlastSpawnTime = -1.0f;
	private float kWaveBlastSpawnInterval = 0.2f;
	private float kWaveBlastChargeInterval = 0.4f;
	private float mWaveBlastChargeTime = -1.0f;
	private float kWaveTotalChargeTime = 0.0f;
	private float kWaveMaxChargeTime = 1.5f;

	private float mShotgunBlastSpawnTime = -1.0f;
	private float kShotgunBlastSpawnInterval = 0.6f;
	private float kShotgunBlastChargeInterval = 0.4f;
	private float mShotgunBlastChargeTime = -1.0f;
	private float kShotgunTotalChargeTime = 0.0f;
	private float kShotgunMaxChargeTime = 1.2f;

	private float kShotgunSpread = -10.0f;
	private int kShotgunShots = 3;
	private int kMaxShotgunShots = 8;

    private PlaySound playme;       // For initiation of playing sounds
    private AudioClip mGunShot;
    private AudioClip mWave;
    private AudioClip mBackground;  // "music by audionautix.com"
	
	void Start () {
        // Audio Files setup
        mGunShot = (AudioClip)Resources.Load("Sounds/GunFire");
        mWave = (AudioClip)Resources.Load("Sounds/WaveFire");
        mBackground = (AudioClip)Resources.Load("Sounds/DeepSpaceY");
        //playme.Play(mBackground, 1f, 1);
        Play(mBackground, 1f, 1);

		if (null == mWaveProjectile)
			mWaveProjectile = Resources.Load ("Prefabs/WaveBlastBlue") as GameObject;
		mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
		for (int i = 0; i <= kMaxShotgunShots; i++) {
			mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastBlue") as GameObject;
		}

		theShip = gameObject.GetComponent<RespawnShip>();
	}

	void Update () {
		//transform.position += Input.GetAxis ("Vertical")  * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
	
		// New Movement
		if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f ||
			Input.GetAxis ("Vertical") > 0.1f || Input.GetAxis ("Vertical") < -0.1f) {
			Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			rigidbody2D.AddForce ( move.normalized * kHeroSpeed);
			}

        // Boundary control
		BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
		boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		
        // Ship mouse aim
		Vector2 mousedir;
		mousedir = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		mousedir.Normalize();
		transform.up = mousedir;

        #region Wave Blast Weapon control
        // Wave Blast control
		if (Input.GetMouseButtonDown(0)) { // this is Left-Control
			mWaveBlastChargeTime = Time.realtimeSinceStartup;
			if ((Time.realtimeSinceStartup - mWaveBlastSpawnTime) > kWaveBlastSpawnInterval) {
				GameObject e = Instantiate(mWaveProjectile) as GameObject;
				WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();

				mWaveBlastSpawnTime = Time.realtimeSinceStartup;
				if (null != waveBlast) {
					if (theShip.powerLevel > 1)
						waveBlast.SetPowerLevel(theShip.powerLevel);
					e.transform.position = transform.position;
					waveBlast.SetForwardDirection(mousedir);
				}
                //playme.Play(mWave, 1f, 1);
                Play(mWave, .5f, 1);
			}
		}
        // Wave Blast Charge
		if (Input.GetMouseButtonUp (0) && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > (kWaveBlastSpawnInterval)) 
		{
			kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
			if (kWaveTotalChargeTime > kWaveMaxChargeTime)
				kWaveTotalChargeTime = kWaveMaxChargeTime;

			GameObject e = Instantiate(mWaveProjectile) as GameObject;
			WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
			if (null != waveBlast) {
				if (theShip.powerLevel > 1)
					waveBlast.SetPowerLevel(theShip.powerLevel);
				waveBlast.mSpeed += waveBlast.mSpeed * kWaveTotalChargeTime;
				e.transform.localScale += new Vector3(kWaveTotalChargeTime, kWaveTotalChargeTime, 0.0f);
				e.transform.position = transform.position;
				waveBlast.SetForwardDirection(mousedir);
                //playme.Play(mWave, 1f, 1);
                Play(mWave, 1f, 1);
			}
        }
        #endregion

        #region Shotgun Blast Weapon control
        // Shotgun Blast control
		if (Input.GetMouseButtonDown(1)) { // this is Right-Control
			mShotgunBlastChargeTime = Time.realtimeSinceStartup;
			if ((Time.realtimeSinceStartup - mShotgunBlastSpawnTime) > kShotgunBlastSpawnInterval) {
				mShotgunBlastSpawnTime = Time.realtimeSinceStartup;

				FireShotgun (kShotgunShots, kShotgunSpread);
			}
		}
        // Shotgun Blast charge control
		if (Input.GetMouseButtonUp(1) && (Time.realtimeSinceStartup - mShotgunBlastChargeTime) > kShotgunBlastChargeInterval) 
		{
			kShotgunTotalChargeTime = Time.realtimeSinceStartup - mShotgunBlastChargeTime;
			if (kShotgunTotalChargeTime > kShotgunMaxChargeTime) {
				FireShotgun(8, -60.0f);
			} else if (kShotgunTotalChargeTime > .9f) {
				FireShotgun(7, -50.0f);
			} else if (kShotgunTotalChargeTime > .7f) {
				FireShotgun(6, -40.0f);
			} else if (kShotgunTotalChargeTime > 0.5f) {
				FireShotgun(5, -30.0f);
			} else {
				FireShotgun(4, -20.0f);
			}
        }
        #endregion

        StartCoroutine("charging");
	} // End of update()
	
    // Shotgun firing
	private void FireShotgun(int shots, float spread) {
		for (int i = 0; i <= shots; i++) {
			GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
			ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();
			
			if (null != shotgunBlast) {
				if (theShip.powerLevel > 1)
					shotgunBlast.SetPowerLevel(theShip.powerLevel);
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

	#region Charge particle support
	IEnumerator charging() {
		Transform theCharge = transform.Find("Charge");
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
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
		} else if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2")) {
			StopCoroutine("charging");
			theCharge.particleSystem.startSize = 1.5f;
			theCharge.particleSystem.enableEmission = false;
		}
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
