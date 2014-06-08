using UnityEngine;
using System.Collections;

public class RoundCounterBehavior : MonoBehaviour {

	private float mUpdateTime, mLastUpdate;
	public GameObject mTarget;
	public Vector3 mTargetPos;
	public bool frame;
	private float spawnTime;
	
	private SpriteManager mSprite = null;
	// Use this for initialization
	void Start () {
		spawnTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if ( transform.position != mTargetPos && Time.realtimeSinceStartup - spawnTime > 2f) {
			Vector3 dir = mTargetPos - transform.position;
			if (dir.magnitude > 5f) {
				dir = dir.normalized * 5f;
			}
			transform.position += dir;
		}
	}
	
	public void makeFrame() {
		GetComponent<SpriteManager>().nextSprite();
		frame = true;
	}
}
