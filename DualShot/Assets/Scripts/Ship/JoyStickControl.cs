/*
 * Controls the stick with a joystick.
*/

using UnityEngine;
using System.Collections;

public class JoyStickControl : MonoBehaviour {

<<<<<<< HEAD
	RespawnShip theShip;

    public GameObject mWaveProjectile = null;
    public GameObject[] mShotgunProjectile = null;

    public float kHeroSpeed = 1500f;
=======
	Ship theShip;
    public float kHeroSpeed = 140f;
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
    private Vector3 mClampedPosition;
    private Vector3 mNewDirection;
    private Vector3 mNewRotation;
    private Vector3 mLastDirection;
    private Vector3 mDefaultDirection;  // Only needed for right joystick control
<<<<<<< HEAD

    private float mWaveBlastSpawnTime = -1.0f;
    private float kWaveBlastSpawnInterval = 0.3f;
    private float kWaveBlastChargeInterval = 0.4f;
    private float mWaveBlastChargeTime = -1.0f;
    private float kWaveTotalChargeTime = 0.0f;
    private float kWaveMaxChargeTime = 1.5f;

    private float mShotgunBlastSpawnTime = -1.0f;
    private float kShotgunBlastSpawnInterval = 0.6f;
    private float kShotgunBlastChargeInterval = 0.4f;
    private float mShotgunBlastChargeTime = -1.0f;
    private float kShotgunTotalChargeTime = 0.0f;
    private float kShotgunMaxChargeTime = 1.8f;

    private float kShotgunSpread = -10.0f;
    private int kShotgunShots = 3;
    private int kMaxShotgunShots = 8;

    private PlaySound playme;       // For initiation of playing sounds
    private AudioClip mGunShot;
    private AudioClip mWave;
=======

    private PlaySound playme;       // For initiation of playing sounds
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
    private AudioClip mBackground;  // "music by audionautix.com"

	// Use this for initialization
    void Start() {
        // Audio Files setup
<<<<<<< HEAD
        mGunShot = (AudioClip)Resources.Load("Sounds/GunFire");
        mWave = (AudioClip)Resources.Load("Sounds/WaveFire");
        mBackground = (AudioClip)Resources.Load("Sounds/DeepSpace");
        //playme.Play(mBackground, 1f, 1);
        Play(mBackground, 1f, 1);

        // Creates gameobects
        if (null == mWaveProjectile)
            mWaveProjectile = Resources.Load("Prefabs/WaveBlastOrange") as GameObject;
        mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
        for (int i = 0; i <= kMaxShotgunShots; i++) {
            mShotgunProjectile[i] = Resources.Load("Prefabs/ShotgunBlastOrange") as GameObject;
        }

        theShip = gameObject.GetComponent<RespawnShip>();
=======
        mBackground = (AudioClip)Resources.Load("Sounds/DeepSpaceY");
		Play (mBackground, 1f, 1);
        theShip = gameObject.GetComponent<Ship>();
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
	}
	
	// Update is called once per frame
    void Update()
    {
        // Player movement
        Vector2 move = new Vector2(Input.GetAxis("P2Horizontal"), Input.GetAxis("P2Vertical"));
        rigidbody2D.AddForce(move.normalized * kHeroSpeed);

        // Right Stick Aim
        transform.up = new Vector3(Input.GetAxis("P2RHorz"), Input.GetAxis("P2RVert"), 0);
        mLastDirection = transform.up;
        if (Input.GetAxis("P2RHorz") < 0.3f && Input.GetAxis("P2RHorz") > -0.3f &&
            Input.GetAxis("P2RVert") < 0.3f && Input.GetAxis("P2RVert") > -0.3f)
        { transform.up = mLastDirection.normalized; }

        // Ship clamped to world
        BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
        boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
<<<<<<< HEAD

        #region Wave Blast Weapon control
        // Weapon controls
        if (Input.GetButtonDown("P2Fire1"))
        { // this is Left-Control
            mWaveBlastChargeTime = Time.realtimeSinceStartup;
            if ((Time.realtimeSinceStartup - mWaveBlastSpawnTime) > kWaveBlastSpawnInterval)
            {
                GameObject e = Instantiate(mWaveProjectile) as GameObject;
                WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();

                mWaveBlastSpawnTime = Time.realtimeSinceStartup;
                if (null != waveBlast)
                {
                    e.transform.position = transform.position;
                    Vector3 mousePos = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;
                    waveBlast.SetForwardDirection(transform.up);
                }
                //playme.Play(mWave, 1f, 1);
                Play(mWave, .5f, 1);
            }
        }

        if (Input.GetButtonUp("P2Fire1") && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > kWaveBlastChargeInterval)
        {
            kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
            if (kWaveTotalChargeTime > kWaveMaxChargeTime)
                kWaveTotalChargeTime = kWaveMaxChargeTime;

            GameObject e = Instantiate(mWaveProjectile) as GameObject;
            WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
            if (null != waveBlast)
            {
                waveBlast.mSpeed += waveBlast.mSpeed * kWaveTotalChargeTime;
                e.transform.localScale += new Vector3(kWaveTotalChargeTime, kWaveTotalChargeTime, 0.0f);
                e.transform.position = transform.position;
                Vector3 mousePos2 = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos2.z = 0;
                waveBlast.SetForwardDirection(transform.up);
                //playme.Play(mWave, 1f, 1);
                Play(mWave, 1f, 1);
            }
        }
        #endregion

        #region Shotgun Blast Weapon control
        if (Input.GetButtonDown("P2Fire2"))
        { // this is Right-Control
            mShotgunBlastChargeTime = Time.realtimeSinceStartup;
            if ((Time.realtimeSinceStartup - mShotgunBlastSpawnTime) > kShotgunBlastSpawnInterval)
            {
                mShotgunBlastSpawnTime = Time.realtimeSinceStartup;

                FireShotgun(kShotgunShots, kShotgunSpread);
                // Play (mClip, .25f, 1);
            }
        }

        if (Input.GetButtonUp("P2Fire2") && (Time.realtimeSinceStartup - mShotgunBlastChargeTime) > kShotgunBlastChargeInterval)
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
    }
 
    private void FireShotgun(int shots, float spread)
    {
        for (int i = 0; i <= shots; i++)
        {
            GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
            e.transform.up = transform.up;

            ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();

            if (null != shotgunBlast)
            {
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
		if (Input.GetButtonDown("P2Fire1") || Input.GetButtonDown("P2Fire2")) {
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
		} else if (Input.GetButtonUp("P2Fire1") || Input.GetButtonUp("P2Fire2")) {
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
=======
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
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
}
