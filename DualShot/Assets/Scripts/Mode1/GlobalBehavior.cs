using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {

	private string mCurrentLevel = "MenuLevel";

    public int DeadBase = 0; // For deciding which base is destroyed

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCurrentLevel(string level){
		mCurrentLevel = level;
	}

	public void PrintCurrentLevel() {
		Debug.Log ("Current Level is " + mCurrentLevel);
	}
}
