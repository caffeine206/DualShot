using UnityEngine;
using System.Collections;

public class RoundCounterBehavior : MonoBehaviour {

	private float mUpdateTime, mLastUpdate;
	public GameObject mTarget;
	public Vector3 mTargetPos;
	public bool stopped;
	
	private BaseSpriteManager mSprite = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if ( transform.position != mTargetPos) {
			Vector3 dir = mTargetPos - transform.position;
			if (dir.magnitude > 10f) {
				dir = dir.normalized * 10f;
			}
			transform.position += dir;
		}
	}
}
