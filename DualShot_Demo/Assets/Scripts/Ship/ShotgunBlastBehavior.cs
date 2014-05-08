using UnityEngine;
using System.Collections;

public class ShotgunBlastBehavior : MonoBehaviour {
	
	private float mSpeed = 100f;
	private float kShotgunLife;
	private float kShotgunSpawnTime;

	void Start()
	{
		kShotgunLife = 0.5f;
		kShotgunSpawnTime =Time.realtimeSinceStartup;;
	}

	// Update is called once per frame
	void Update () {
		if ((Time.realtimeSinceStartup - kShotgunSpawnTime) > kShotgunLife) {
			Destroy(this.gameObject);
		}

		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        //string level = MenuState.TheGameState.getLevel;
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

	public Vector3 RotateX( Vector3 v, float angle )
	{
		float sin = Mathf.Sin( angle );
		float cos = Mathf.Cos( angle );
		
		float ty = v.y;
		float tz = v.z;
		v.y = (cos * ty) - (sin * tz);
		v.z = (cos * tz) + (sin * ty);
		
		return v;
	}
}
