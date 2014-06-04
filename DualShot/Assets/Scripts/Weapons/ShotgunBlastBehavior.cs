using UnityEngine;
using System.Collections;

public class ShotgunBlastBehavior : MonoBehaviour {
	
	private float mSpeed = 170f;
	private float kShotgunLife = 0.3f;
	private float kShotgunSpawnTime;

	void Start()
	{
		kShotgunSpawnTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
		if ((Time.realtimeSinceStartup - kShotgunSpawnTime) > kShotgunLife) {
			Destroy(this.gameObject);
		}

		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        //string level = MenuState.TheGameState.getLevel;
		/*WorldBehavior WorldBehavior = GameObject.Find ("GameManager").GetComponent<WorldBehavior>();
		WorldBehavior.WorldBoundStatus status =
            WorldBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != WorldBehavior.WorldBoundStatus.Inside) {
			Destroy(this.gameObject);
		}*/
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

	public void AddShotgunSpeed(float f) {
		mSpeed += f;
	}

	public void SetPowerLevel(int level) {
		float increase = level * 1.0f;
		kShotgunLife += increase / 6.0f;
		transform.localScale += new Vector3 (increase * 3.0f, increase * 3.0f, 0.0f);
	}
}
