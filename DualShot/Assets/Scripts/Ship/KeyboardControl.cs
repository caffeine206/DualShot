/*
 * Controls the ship with a keyboard.
*/

using UnityEngine;	
using System.Collections;

public class KeyboardControl : MonoBehaviour {

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;

	public float kHeroSpeed = 140f;
	private Vector3 mClampedPosition;
	private int mPowerLevel = 1;

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
	private AudioClip mClip;
	
	void Start () {
		// mClip = (AudioClip)Resources.Load ("Sounds/GunShot");
		if (null == mWaveProjectile)
			mWaveProjectile = Resources.Load ("Prefabs/WaveBlastBlue") as GameObject;
		mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
		for (int i = 0; i <= kMaxShotgunShots; i++) {
			mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastBlue") as GameObject;
		}
	}

	void Update () {
		//transform.position += Input.GetAxis ("Vertical")  * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
	
		// New Movement
		if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f ||
			Input.GetAxis ("Vertical") > 0.1f || Input.GetAxis ("Vertical") < -0.1f) {
			Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			rigidbody2D.AddForce ( move.normalized * kHeroSpeed);
			}
		
		
		// Old Tranform
		/*if (Input.GetAxis("Horizontal") > 0.0) { //if the up arrow is pressed 
			transform.Translate(kHeroSpeed * Time.deltaTime, 0.0f, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Horizontal") < 0.0) { //if the down arrow is pressed 
			transform.Translate(-kHeroSpeed * Time.deltaTime, 0.0f, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Vertical") > 0.0) { //if the right arrow is pressed 
			transform.Translate(0.0f, kHeroSpeed * Time.deltaTime, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Vertical") < 0.0) { //if the left arrow is pressed 
			transform.Translate(0.0f, -kHeroSpeed * Time.deltaTime, 0.0f); //and then turn the plane 
		}*/

		BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
		boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		
		Vector2 mousedir;
		mousedir = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		mousedir.Normalize();
		transform.up = mousedir;
		
		
		/*LocalGameBehavior LocalGameBehavior2 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		mClampedPosition = new Vector3(Mathf.Clamp(this.transform.position.x, LocalGameBehavior2.mWorldMin.x, LocalGameBehavior2.mWorldMax.x),
		Mathf.Clamp(this.transform.position.y, LocalGameBehavior2.mWorldMin.y, LocalGameBehavior2.mWorldMax.y), 0.0f);
		this.transform.position = mClampedPosition;*/
		/*
		Vector3 mousePos3 = LocalGameBehavior2.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
		mousePos3.z = 0;
		transform.rotation = Quaternion.LookRotation (Vector3.forward, mousePos3 - transform.position);
		*/
		if (Input.GetMouseButtonDown(0)) { // this is Left-Control
			mWaveBlastChargeTime = Time.realtimeSinceStartup;
			if ((Time.realtimeSinceStartup - mWaveBlastSpawnTime) > kWaveBlastSpawnInterval) {
				GameObject e = Instantiate(mWaveProjectile) as GameObject;
				WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();

				mWaveBlastSpawnTime = Time.realtimeSinceStartup;
				if (null != waveBlast) {
					if (mPowerLevel > 1)
						waveBlast.SetPowerLevel(mPowerLevel);
					e.transform.position = transform.position;
					waveBlast.SetForwardDirection(mousedir);
				}
				// Play (mClip, .25f, 1);
			}
		}

		if (Input.GetMouseButtonUp (0) && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > (kWaveBlastSpawnInterval)) 
		{
			kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
			if (kWaveTotalChargeTime > kWaveMaxChargeTime)
				kWaveTotalChargeTime = kWaveMaxChargeTime;

			GameObject e = Instantiate(mWaveProjectile) as GameObject;
			WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
			if (null != waveBlast) {
				if (mPowerLevel > 1)
					waveBlast.SetPowerLevel(mPowerLevel);
				waveBlast.mSpeed += waveBlast.mSpeed * kWaveTotalChargeTime;
				e.transform.localScale += new Vector3(kWaveTotalChargeTime, kWaveTotalChargeTime, 0.0f);
				e.transform.position = transform.position;
				waveBlast.SetForwardDirection(mousedir);
			}
		}

		if (Input.GetMouseButtonDown(1)) { // this is Right-Control
			mShotgunBlastChargeTime = Time.realtimeSinceStartup;
			if ((Time.realtimeSinceStartup - mShotgunBlastSpawnTime) > kShotgunBlastSpawnInterval) {
				mShotgunBlastSpawnTime = Time.realtimeSinceStartup;

				FireShotgun (kShotgunShots, kShotgunSpread);
				// Play (mClip, .25f, 1);
			}
		}

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

		StartCoroutine("charging");
	}
	/*
	public void Play(AudioClip clip, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject ("Audio: " +  clip.name);
		LocalGameBehavior LocalGameBehavior2 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		go.transform.position = LocalGameBehavior2.transform.position;
		go.transform.parent = LocalGameBehavior2.transform;
		
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
		Destroy (go, clip.length);
	}
	*/
	private void FireShotgun(int shots, float spread) {
		for (int i = 0; i <= shots; i++) {
			GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
			ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();
			
			if (null != shotgunBlast) {
				if (mPowerLevel > 1)
					shotgunBlast.SetPowerLevel(mPowerLevel);
				e.transform.position = transform.position + transform.up * 12f;
				e.transform.up = transform.up;
				shotgunBlast.AddShotgunSpeed(rigidbody2D.velocity.magnitude);
				shotgunBlast.SetForwardDirection(e.transform.up);
				e.transform.Rotate(Vector3.forward, spread + (i * shots * 2));
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "PowerUp" || other.gameObject.name == "PowerUp(Clone)") {
			Destroy(other.gameObject);
			mPowerLevel++;
			//Debug.Log("Power level: " + mPowerLevel);
			if (mPowerLevel > 3)
				mPowerLevel = 3;
		}
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
}
