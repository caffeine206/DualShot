using UnityEngine;
using System.Collections;

public class ControlButton : MonoBehaviour
{
	// Instantiate sound clips
	private AudioClip mHover;
	//private AudioClip mStartScreenMusic;
	
	// Instantiate text mesh object for text on buttons
	private TextMesh tm;
	public int player;
	private bool isKeyboard = false;
	
	private WorldBehavior world = null;
	
	// Use this for initialization
	void Start()
	{	
		if (world == null) {
			world = GameObject.Find ( "GameManager").GetComponent<WorldBehavior>();
		}
		// Associates sound clips
		mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");
		
		// Associates text mesh component 
		tm = GetComponent<TextMesh>();
		if (world.isKeyboard(player)) {
			tm.text = "Keyboard";
		}
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
		if (isKeyboard) {
			world.setupControls(0);
		} else {
			world.setupControls(player);
		}
	}
	
	public void SetKeyboard() {
		tm.text = "Keyboard";
		isKeyboard = true;
	}
	
	public void SetJoystick(int joyNum) {
		tm.text = "Joystick" + joyNum;
		isKeyboard = false;
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
