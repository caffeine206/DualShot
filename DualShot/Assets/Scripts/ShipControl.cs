using UnityEngine;	
using System.Collections;

public class ShipControl : MonoBehaviour {

	public GameObject mProjectile = null;

	private float kHeroSpeed = 30f;
	private Vector3 mClampedPosition;
	private float mWaveBlastSpawnTime = -1.0f;
	private float kWaveBlastSpawnInterval = 0.1f;
	private AudioClip mClip;
	
	void Start () {
		// mClip = (AudioClip)Resources.Load ("Sounds/GunShot");
		if (null == mProjectile)
			mProjectile = Resources.Load ("Prefabs/WaveBlast") as GameObject;
	}

	void Update () {
		LocalGameBehavior LocalGameBehavior2 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
	
		#region user movement control
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
		mClampedPosition = new Vector3(Mathf.Clamp(this.transform.position.x, LocalGameBehavior2.mWorldMin.x, LocalGameBehavior2.mWorldMax.x),
		Mathf.Clamp(this.transform.position.y, LocalGameBehavior2.mWorldMin.y, LocalGameBehavior2.mWorldMax.y), 0.0f);
		this.transform.position = mClampedPosition;
		// transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (kHeroRotateSpeed * Time.smoothDeltaTime));
		#endregion

		if (Input.GetAxis ("Fire1") > 0f) { // this is Left-Control
			if ((Time.realtimeSinceStartup - mWaveBlastSpawnTime) > kWaveBlastSpawnInterval) {
				GameObject e = Instantiate(mProjectile) as GameObject;
				WaveBlastBehavior waveBlast = e.GetComponent<WaveBlastBehavior>(); // Shows how to get the script from GameObject

				mWaveBlastSpawnTime = Time.realtimeSinceStartup;
				if (null != waveBlast) {
					e.transform.position = transform.position;
					LocalGameBehavior localGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
					Vector3 mousePos = localGameBehavior.mMainCamera.ScreenToWorldPoint(Input.mousePosition);
					mousePos.z = 0;
					waveBlast.SetForwardDirection(mousePos - e.transform.position);
				}
				// Play (mClip, .25f, 1);
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
}
