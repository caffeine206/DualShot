using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour
{
    // Instantiate sound clips
    private AudioClip mHover;
    //private AudioClip mStartScreenMusic;

    // Instantiate text mesh object for text on buttons
    private TextMesh tm;

    // Camera movement positions for each "scene"
    private Vector3 Setup = new Vector3(0, 80, -9);
    private Vector3 Instructions = new Vector3(0, 60, -9);
    private Vector3 Credits = new Vector3(0, 40, -9);
    private Vector3 Controls = new Vector3(0, 20, -9);
    private Vector3 MainMenu = new Vector3(0, 0, -9);


    // Use this for initialization
    void Start()
    {
        // Associates sound clips
        mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");

        // Associates text mesh component 
        tm = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // To highlight button text when mouse is over collider
    void OnMouseEnter()
    {
        tm.fontSize = 55;
        renderer.material.color = new Color(95, 153, 207, 255);
        //renderer.material.color = Color.red;
        Play(mHover, 0.25f, 1);
    }
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        tm.fontSize = 50;
        //renderer.material.color = new Color(0.8f, 0.282f, 0f, 1);
        renderer.material.color = Color.white;

        //renderer.material.color = Color.blue;
    }


    void OnMouseUp()
    {
        // For button up controls based on the "text" of the button

        // Main menu - Controls - Credits - Instructions buttons
        if (GetComponent<TextMesh>().text == "Controls")
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
        if (GetComponent<TextMesh>().text == "Exit")
        {
            Application.Quit();
        }
        if (GetComponent<TextMesh>().text == "Start")
        {
            //Camera.main.transform.position = Setup;
            Application.LoadLevel(1);   // Level 1
        }

        // Pause menu buttons
        if (GetComponent<TextMesh>().text == "Resume")
        {
            //resume.Pause();
        }
        if (GetComponent<TextMesh>().text == "Reset")
        {
            Application.LoadLevel(1);   // Level 1
        }
        if (GetComponent<TextMesh>().text == "Quit")
        {
            Time.timeScale = 1f;
            Application.LoadLevel(0);   // Menu
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
