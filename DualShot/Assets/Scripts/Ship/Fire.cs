using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	
	public bool isOrange = true;

	private AudioClip mGunShot1;
    private AudioClip mGunShot2;
    private AudioClip mWave;

	public GameObject mWaveProjectile = null;
	public GameObject[] mShotgunProjectile = null;

	private float kWaveMaxChargeTime = 1.5f;

	private float kShotgunMaxChargeTime = 1.1f;
	
	private float kShotgunSpread = -20.0f;
	private int kShotgunShots = 5;
	private int kMaxShotgunShots = 9;

	void Start () {
        // Audio Files setup
        mGunShot1 = (AudioClip)Resources.Load("Sounds/shotgun");
        mGunShot2 = (AudioClip)Resources.Load("Sounds/shotgun2");
        mWave = (AudioClip)Resources.Load("Sounds/WaveFire");

		// Initiate weapons
		if (null == mWaveProjectile) {
			if (isOrange == false)
				mWaveProjectile = Resources.Load ("Prefabs/WaveBlastBlue") as GameObject;
			else
				mWaveProjectile = Resources.Load ("Prefabs/WaveBlastOrange") as GameObject;
		}
		mShotgunProjectile = new GameObject[kMaxShotgunShots + 1];
		for (int i = 0; i <= kMaxShotgunShots; i++) {
			if (isOrange == false)
				mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastBlue") as GameObject;
			else
				mShotgunProjectile[i] = Resources.Load ("Prefabs/ShotgunBlastOrange") as GameObject;
		}
	}
	
	void Update () {
	}

	public void FireWaveBlast(Vector2 mousedir, GameObject ship, int powerLevel) {
		GameObject e = Instantiate(mWaveProjectile) as GameObject;
		WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();

		if (null != waveBlast) {
			if (powerLevel > 1)
				waveBlast.SetPowerLevel(powerLevel);
			e.transform.position = ship.transform.position;
			waveBlast.SetForwardDirection(mousedir);
		}
		Play(mWave, .5f, 1);
	}
	
	public void FireChargedWaveBlast(Vector2 mousedir, GameObject ship, int powerLevel, float kWaveTotalChargeTime) {
		GameObject e = Instantiate(mWaveProjectile) as GameObject;
		WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>();
		if (null != waveBlast) {
			if (powerLevel > 1)
				waveBlast.SetPowerLevel(powerLevel);
			waveBlast.mSpeed += (waveBlast.mSpeed * kWaveTotalChargeTime) / 2.0f;
			waveBlast.mForce += kWaveTotalChargeTime * 20.0f;
			e.transform.localScale += new Vector3(kWaveTotalChargeTime * 2, kWaveTotalChargeTime * 2, 0.0f);
			e.transform.position = ship.transform.position;
			waveBlast.SetForwardDirection(mousedir);
			Play(mWave, 1f, 1);
		}
	}
	
	public void FireShotgunBlast(GameObject ship, int powerLevel) {
		FireShotgun(kShotgunShots, kShotgunSpread, ship, powerLevel);
		Play(mGunShot1, 1f, 1);
	}

	/*
	public void FireChargedShotgunBlast(GameObject ship, int powerLevel, float kShotgunTotalChargeTime) {
		if (kShotgunTotalChargeTime > kShotgunMaxChargeTime) {
			FireShotgun(9, -80.0f, ship, powerLevel);
		} else if (kShotgunTotalChargeTime > .9f) {
			FireShotgun(8, -70.0f, ship, powerLevel);
		} else if (kShotgunTotalChargeTime > .7f) {
			FireShotgun(7, -45.0f, ship, powerLevel);
		} else {
			FireShotgun(6, -35.0f, ship, powerLevel);
		} 

	}
	*/
	
	// Shotgun firing
	public void FireShotgun(int shots, float spread, GameObject ship, int powerLevel) {
		for (int i = 0; i <= shots; i++) {
			GameObject e = Instantiate(mShotgunProjectile[i]) as GameObject;
			ShotgunBlastBehavior shotgunBlast = e.GetComponent<ShotgunBlastBehavior>();
			
			if (null != shotgunBlast) {
				if (powerLevel > 1)
					shotgunBlast.SetPowerLevel(powerLevel);
				e.transform.position = ship.transform.position + ship.transform.up * 14f;
				e.transform.up = ship.transform.up;
				shotgunBlast.AddShotgunSpeed(rigidbody2D.velocity.magnitude);
				shotgunBlast.SetForwardDirection(e.transform.up);
				e.transform.Rotate(Vector3.forward, spread + (i * shots * 2));
			}
		}
		Play(mGunShot2, 1f, 1);
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
