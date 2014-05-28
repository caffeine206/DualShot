using UnityEngine;
using System.Collections;

public class SetupButtonBehavior : MonoBehaviour
{
    // Instantiate sound clips
    private AudioClip mHover;

    // Instantiate text mesh object for text on buttons
    private TextMesh tm;

    // For Players total cursor selection
    private Vector3 Players2 = new Vector3(-1f, 82.5f, -1f);
    private Vector3 Players3 = new Vector3(2f, 82.5f, -1f);
    private Vector3 Players4 = new Vector3(5f, 82.5f, -1f);

    // For Player controller selector
    private Vector3 Player1J = new Vector3(-0.65f, 80.4f, -1f);
    private Vector3 Player1K = new Vector3(-0.65f, 79.4f, -1f);
    private Vector3 Player2J = new Vector3(1.3f, 80.4f, -1f);
    private Vector3 Player2K = new Vector3(1.3f, 79.4f, -1f);
    private Vector3 Player3J = new Vector3(3.25f, 80.4f, -1f);
    private Vector3 Player3K = new Vector3(3.25f, 79.4f, -1f);
    private Vector3 Player4J = new Vector3(5.2f, 80.4f, -1f);
    private Vector3 Player4K = new Vector3(5.2f, 79.4f, -1f);

    // For Number of rounds selector
    private Vector3 Rounds1 = new Vector3(-1f, 78f, -1f);
    private Vector3 Rounds3 = new Vector3(2f, 78f, -1f);
    private Vector3 Rounds5 = new Vector3(5f, 78f, -1f);

    // For Border selection instantiation
    //private GameObject PlayerSelect = GameObject.;
    private GameObject ControllerSelect = null;
    private GameObject RoundSelect = null;

    // Use this for initialization
    void Start()
    {
        // Associates sound clips
        mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");

        // Associates text mesh component 
        tm = GetComponent<TextMesh>();

        //Initialize the borders for selection

    }


    // Update is called once per frame
    void Update()
    {

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
        // For button up controls mainly based on the "text" of the button

        // Setup number of players select
        if (GetComponent<TextMesh>().text == "2")
        {
            // Moves Setup-PlayersBorder to number 2

            // Changes number of players count to 2
                        
        }
        if (GetComponent<TextMesh>().text == "3")
        {
            // Moves Setup-PlayersBorder to number 3
            

            // Changes number of players count to 3
            
        }
        if (GetComponent<TextMesh>().text == "4")
        {
            // Moves Setup-PlayersBorder to number 4

            // Changes number of players count to 4
            
        }

        // For Player controller selection/assignment
        if (gameObject.name == "Setup-Controller1")
        {
            // Moves Setup-Player1Select border to controller 1
        }
        if (gameObject.name == "Setup-Controller2")
        {
            // Moves Setup-Player2Select border to controller 2
        }
        if (gameObject.name == "Setup-Controller3")
        {
            // Moves Setup-Player3Select border to controller 3
        }
        if (gameObject.name == "Setup-Controller4")
        {
            // Moves Setup-Player4Select border to controller 4
        }
        if (gameObject.name == "Setup-KybdP1")
        {
            // Moves Setup-Player1Select border to keyboard 1

            // Moves all other borders to respective controllers 2,3,4

            // Updates ship controller input

        }
        if (gameObject.name == "Setup-KybdP2")
        {
            // Moves Setup-Player2Select border to keyboard 2

            // Moves all other borders to respective controllers 1,3,4

            // Updates ship controller input
        }
        if (gameObject.name == "Setup-KybdP3")
        {
            // Moves Setup-Player3Select border to keyboard 3

            // Moves all other borders to respective controllers 1,2,4

            // Updates ship controller input
        }
        if (gameObject.name == "Setup-KybdP4")
        {
            // Moves Setup-Player4Select border to keyboard 4

            // Moves all other borders to respective controllers 1,2,3

            // Updates ship controller input
        }
        

        // Sets Best of Rounds counter
        if (GetComponent<TextMesh>().text == "1 Round")
        {
            // Move Setup-RoundsBorder to vector3 Rounds1

            // Change global number of rounds to 1
        }
        if (GetComponent<TextMesh>().text == "3 Rounds")
        {
            // Move Setup-RoundsBorder to vector3 Rounds2

            // Change global number of rounds to 3
           
        }
        if (GetComponent<TextMesh>().text == "5 Rounds")
        {
            // Move Setup-RoundsBorder to vector3 Rounds3

            // Change global number of rounds to 5
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
