/*
 * Controls the ship with a keyboard.
*/

using UnityEngine;	
using System.Collections;

public class KeyboardControl : MonoBehaviour {

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;

	private float kHeroSpeed = 40f;
	private Vector3 mClampedPosition;

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
		if (Input.GetAxis("Vertical") < 0.0) { //if the up arrow is pressed 
			transform.Translate(kHeroSpeed * Time.deltaTime, 0.0f, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Vertical") > 0.0) { //if the down arrow is pressed 
			transform.Translate(-kHeroSpeed * Time.deltaTime, 0.0f, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Horizontal") > 0.0) { //if the right arrow is pressed 
			transform.Translate(0.0f, kHeroSpeed * Time.deltaTime, 0.0f); //and then turn the plane 
		}
		if (Input.GetAxis("Horizontal") < 0.0) { //if the left arrow is pressed 
			transform.Translate(0.0f, -kHeroSpeed * Time.deltaTime, 0.0f); //and then turn the plane 
		}

		LocalGameBehavior LocalGameBehavior2 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		mClampedPosition = new Vector3(Mathf.Clamp(this.transform.position.x, LocalGameBehavior2.mWorldMin.x, LocalGameBehavior2.mWorldMax.x),
		Mathf.Clamp(this.transform.position.y, LocalGameBehavior2.mWorldMin.y, LocalGameBehavior2.mWorldMax.y), 0.0f);
		this.transform.position = mClampedPosition;
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
					e.transform.position = transform.position;
					LocalGameBehavior LocalGameBehavior3 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
					Vector3 mousePos = LocalGameBehavior3.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
					mousePos.z = 0;
					waveBlast.SetForwardDirection(mousePos - e.transform.position);
				}
				// Play (mClip, .25f, 1);
			}
		}

		if (Input.GetMouseButtonUp (0) && (Time.realtimeSinceStartup - mWaveBlastChargeTime) > kWaveBlastChargeInterval) 
		{
			kWaveTotalChargeTime = Time.realtimeSinceStartup - mWaveBlastChargeTime;
			if (kWaveTotalChargeTime > kWaveMaxChargeTime)
				kWaveTotalChargeTime = kWaveMaxChargeTime;

			GameObject e = Instantiate(mWaveProjectile) as GameObject;
			WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
			if (null != waveBlast) {
				waveBlast.mSpeed += waveBlast.mSpeed * kWaveTotalChargeTime;
				e.transform.localScale += new Vector3(kWaveTotalChargeTime, kWaveTotalChargeTime, 0.0f);
				e.transform.position = transform.position;
				LocalGameBehavior localGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
				Vector3 mousePos2 = localGameBehavior.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
				mousePos2.z = 0;
				waveBlast.SetForwardDirection(mousePos2 - e.transform.position);
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
			} else if (kShotgunTotalChargeTime > 1.5f) {
				FireShotgun(7, -50.0f);
			} else if (kShotgunTotalChargeTime > 1.1f) {
				FireShotgun(6, -40.0f);
			} else if (kShotgunTotalChargeTime > 0.7f) {
				FireShotgun(5, -30.0f);
			} else {
				FireShotgun(4, -20.0f);
			}
		}
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
				e.transform.position = transform.position;
				e.transform.Rotate(Vector3.forward, spread + (i * shots * 2));
				shotgunBlast.SetForwardDirection(e.transform.up);
			}
		}
	}
}
