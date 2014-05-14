/*
 * Controls the stick with a joystick.
*/

using UnityEngine;
using System.Collections;

public class JoyStickControl : MonoBehaviour {

	RespawnShip theShip;

    public GameObject mWaveProjectile = null;
    public GameObject[] mShotgunProjectile = null;

    private float kHeroSpeed = 1.5f;
    private Vector3 mClampedPosition;
    private Vector3 mNewDirection;
    private Vector3 mNewRotation;
    //private Vector3 mDefaultDirection = new Vector3(1,0,0);

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
    private AudioClip mClip;

	// Use this for initialization
    void Start() {
        // mClip = (AudioClip)Resources.Load ("Sounds/GunShot");
        if (null == mWaveProjectile)
            mWaveProjectile = Resources.Load("Prefabs/WaveBlastOrange") as GameObject;
        mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
        for (int i = 0; i <= kMaxShotgunShots; i++) {
            mShotgunProjectile[i] = Resources.Load("Prefabs/ShotgunBlastOrange") as GameObject;
        }	
	}
	
	// Update is called once per frame
    void Update()
    {
        // Player movement
        mNewDirection = new Vector3(Input.GetAxis("P2Horizontal"), Input.GetAxis("P2Vertical"), 0.0f);
        transform.position += mNewDirection * kHeroSpeed;  // 

        // Right Stick Aim
        transform.up = new Vector3(Input.GetAxis("RHorz"), Input.GetAxis("RVert"), 0);
        //if (Input.GetAxis("RHorz") < 0.1f && Input.GetAxis("RVert") < 0.1f)
        //{ transform.up = mDefaultDirection; }

        // Ship clamped to world
        BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
        boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
        /*mClampedPosition = new Vector3(Mathf.Clamp(this.transform.position.x, boundsControl.mWorldMin.x, boundsControl.mWorldMax.x),
        Mathf.Clamp(this.transform.position.y, boundsControl.mWorldMin.y, boundsControl.mWorldMax.y), 0.0f);
        this.transform.position = mClampedPosition;*/

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
                // Play (mClip, .25f, 1);
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
            if (kShotgunTotalChargeTime > kShotgunMaxChargeTime)
            {
                FireShotgun(8, -60.0f);
            }
            else if (kShotgunTotalChargeTime > 1.5f)
            {
                FireShotgun(7, -50.0f);
            }
            else if (kShotgunTotalChargeTime > 1.1f)
            {
                FireShotgun(6, -40.0f);
            }
            else if (kShotgunTotalChargeTime > 0.7f)
            {
                FireShotgun(5, -30.0f);
            }
            else
            {
                FireShotgun(4, -20.0f);
            }
        }
        #endregion

		StartCoroutine("charging");
    }
    /*
    public void Play(AudioClip clip, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject ("Audio: " +  clip.name);
        LocalGameBehavior boundsControl = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
        go.transform.position = boundsControl.transform.position;
        go.transform.parent = boundsControl.transform;
		
        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play ();
        Destroy (go, clip.length);
    }
    */
    private void FireShotgun(int shots, float spread)
    {
        for (int i = 0; i <= shots; i++)
        {
            GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
            e.transform.up = transform.up;

            ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();

            if (null != shotgunBlast)
            {
                e.transform.position = transform.position;
                e.transform.Rotate(transform.forward, spread + (i * shots * 2));
                shotgunBlast.SetForwardDirection(e.transform.up);
            }
        }
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
}
