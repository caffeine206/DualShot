using UnityEngine;
using System.Collections;

public class StartLevel1 : MonoBehaviour {

    public string LevelName = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        Application.LoadLevel(1);
        
    }
}
