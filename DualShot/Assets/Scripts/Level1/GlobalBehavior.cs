using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {

	private string mCurrentLevel = "MenuLevel";
	private int mOrWins = 0, // OrangeWins
				mBlWins = 0, // BlueWins
				mBestOf = 3,	// Rounds total
				mRoundNum = 1;	// Cur Round starting at one

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
	public int OrangeWins {
		get { return mOrWins; }
		set { mOrWins = value; }
	}
	public int BlueWins {
		get { return mBlWins; }
		set { mBlWins = value; }
	}
	public int BestOf {
		get { return mBestOf; }
		set { mBestOf = value; }
	}
	public int RoundNum {
		get { return mRoundNum; }
		set { mRoundNum = value; }
	}
}
