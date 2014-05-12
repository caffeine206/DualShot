using UnityEngine;
using System.Collections;

public class WaveBlastBehavior : MonoBehaviour {
	
	public float mSpeed = 100f;
	public float mForce = 40f;
	private float kWaveLife = 1.2f;
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
			Vector2 dir = other.transform.position - transform.position;
			dir.Normalize();
			other.rigidbody2D.AddForce(mSpeed * dir * mForce);
		}
	}

	public void SetPowerLevel(int level) {
		float increase = level * 1.0f;
		kWaveLife += increase / 4.0f;
		transform.localScale += new Vector3 (increase * 2.0f, increase * 2.0f, 0.0f);
	}
}
