using UnityEngine;
using System.Collections;

public class WaveBlastBehavior : MonoBehaviour {
	
	public float mSpeed = 100f;
	public float mForce = 70f;
	private float kWaveLife = 1.0f;
	private float kWaveSpawnTime;

	void Start()
	{
		kWaveSpawnTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
		if ((Time.realtimeSinceStartup - kWaveSpawnTime) > kWaveLife) {
			Destroy(this.gameObject);
		}
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        //string level = MenuState.TheGameState.getLevel; // Why does my weapon care about my level?
		BoundsControl boundsControl = GameObject.Find ("GameManager").GetComponent<BoundsControl>();
		BoundsControl.WorldBoundStatus status =
            boundsControl.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != BoundsControl.WorldBoundStatus.Inside) {
			Destroy(this.gameObject);
		}
	}
	
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Orb" || other.gameObject.name == "Orb(Clone)") {
			//Debug.Log("Push");
			/*Vector2 dir = other.transform.position - transform.position;
			dir.Normalize();*/
			other.rigidbody2D.AddForce(mSpeed * transform.up * mForce);
<<<<<<< HEAD
=======
			//other.gameObject.rigidbody2D.AddForce(mSpeed * transform.up * mForce);
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
		}
	}

	public void SetPowerLevel(int level) {
		float increase = level * 1.0f;
		kWaveLife += increase / 8.0f;
		mForce += 20.0f;
		transform.localScale += new Vector3 (increase * 3.0f, increase * 3.0f, 0.0f);
	}
}
