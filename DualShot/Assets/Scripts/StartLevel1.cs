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

    // To highlight button text when mouse is over collider
    void OnMouseOver()
    {
        renderer.material.color = Color.green;
    }
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    void OnMouseUp()
    {
        Application.LoadLevel(1);
    }
<<<<<<< HEAD
    
    void OnMouseOver() {
    	Debug.Log ("Here");
    }
=======
>>>>>>> 8903bcb7502521a2d25e8606cb55457b7fb4e110
}
