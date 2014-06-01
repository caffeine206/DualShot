using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {

	private string mCurrentLevel = "MenuLevel";
	private int mOrWins = 0, // OrangeWins
				mBlWins = 0, // BlueWins
				mBestOf = 3,	// Rounds total
				mRoundNum = 1;	// Cur Round starting at one
				
	private string[] joysticks;
	private int keyboard;

    public int DeadBase = 0; // For deciding which base is destroyed

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		joysticks = new string[4] {"P1","P2","P3","P4"};
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
	public void setKeyboard(int player) {
		int pNum = 1;
		keyboard = player;
		for ( int i = 1; i < 5; ++i ) {
			if ( player == i ) {
				Debug.Log("Keyboard: " + keyboard);
				joysticks[i - 1] = "P4";
				GameObject.Find("Setup-Joystick" + i).GetComponent<ControlButton>().SetKeyboard();
			} else {
				joysticks[i - 1] = "P" + pNum;
				GameObject.Find("Setup-Joystick" + i).GetComponent<ControlButton>().SetJoystick(pNum);
				pNum++;
			}
		}
		
	}
	public void disableKeyboard() {
		keyboard = 0;
		for ( int i = 1; i <= 4; ++i ) {
				joysticks[i - 1] = "P" + i;
				GameObject.Find("Setup-Joystick" + i).GetComponent<ControlButton>().SetJoystick(i);;
			
		}
	}
	public void setupShip(Ship ship) {
		ship.controller = joysticks[ship.player - 1];
		if ( ship.player == keyboard ) {
			ship.isController = false;
		}
	}
}
