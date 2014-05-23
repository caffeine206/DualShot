using UnityEngine;
using System.Collections;

public class MoveMainCamera : MonoBehaviour {

    // Instantiate sound clips
    private AudioClip mHover;       
    //private AudioClip mStartScreenMusic;
    
    // Instantiate text mesh object for text on buttons
    private TextMesh tm;     

    // Camera movement positions for each "scene"
    private Vector3 Instructions = new Vector3(0, 60, -9);
    private Vector3 Credits =      new Vector3(0, 40, -9);
    private Vector3 Controls =     new Vector3(0, 20, -9);
    private Vector3 MainMenu =     new Vector3(0, 0, -9);


	// Use this for initialization
	void Start () {

        // Associates sound clips
        mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");
        //mStartScreenMusic = (AudioClip)Resources.Load("Sounds/StartScreen");

        // Plays the music for Main Menu/Controls/Credits/Instructions
        //Play(mStartScreenMusic, 0.5f, 1);
        
        // Associates text mesh component 
        tm = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("escape"))  //When Esc key is pressed down, sends camera to Main Menu
        {
            Camera.main.transform.position = MainMenu;
        }
	
	}

    // To highlight button text when mouse is over collider
    void OnMouseEnter()
    {
        tm.fontSize = 55;
        Play(mHover, 0.5f, 1);
    }
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        tm.fontSize = 50;
    }
    

    void OnMouseUp()
    {
        //Application.LoadLevel(6);  // Instructions
        if(GetComponent<TextMesh>().text == "Controls")
        {
            Camera.main.transform.position = Controls;
        }
        if (GetComponent<TextMesh>().text == "Credits")
        {
            Camera.main.transform.position = Credits;
        }
        if (GetComponent<TextMesh>().text == "Instructions")
        {
            Camera.main.transform.position = Instructions;
        }
        if (GetComponent<TextMesh>().text == "Main Menu")
        {
            Camera.main.transform.position = MainMenu;
        }
    }

    // Audio clip player
    public void Play(AudioClip clip, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
    }
}
