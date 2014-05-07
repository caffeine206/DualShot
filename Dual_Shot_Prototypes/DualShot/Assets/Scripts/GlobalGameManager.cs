using UnityEngine;
using System.Collections;

public class GlobalGameManager : MonoBehaviour {

	private string mCurrentLevel = "Menu";  //  

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}

	// 
	public void SetCurrentLevel(string level) {
		mCurrentLevel = level;
	}

	public void PrintCurrentLevel()
	{
		Debug.Log("Current Level is: " + mCurrentLevel);
	}

	public string getLevel { get { return mCurrentLevel; } }
}
