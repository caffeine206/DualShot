using UnityEngine;
using System.Collections;

public class WaveBlastBehavior : MonoBehaviour {
	
	public float mSpeed = 100f;

	void Start()
	{
	}

	// Update is called once per frame
	void Update () {
		//
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
		if (other.gameObject.name == "Orb" || other.gameObject.name == "Orb \t(clone)") {
			Vector2 dir = other.transform.position - transform.position;
			dir.Normalize();
			other.rigidbody2D.AddForce(mSpeed * dir * 100f);
		}
	}
}
