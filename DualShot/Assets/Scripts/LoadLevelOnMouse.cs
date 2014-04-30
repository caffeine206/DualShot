using UnityEngine;
using System.Collections;

public class LoadLevelOnMouse : MonoBehaviour {

	public string LevelName = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver() {
		Debug.Log("Mouse over us!");
	}

	void OnMouseUp() {
		Application.LoadLevel(LevelName);
		MenuState.TheGameState.SetCurrentLevel(LevelName);
		MenuState.TheGameState.PrintCurrentLevel();
	}
}