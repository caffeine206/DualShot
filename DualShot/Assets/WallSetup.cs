using UnityEngine;
using System.Collections;

public class WallSetup : MonoBehaviour {
	
	public int mSide;

	// Use this for initialization
	void Start () {
		Camera mMain = Camera.main;	
		transform.position = new Vector3((mMain.aspect * mMain.orthographicSize * mSide * 1.04f), 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
