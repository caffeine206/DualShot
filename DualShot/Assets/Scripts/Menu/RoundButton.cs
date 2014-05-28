using UnityEngine;
using System.Collections;

public class RoundButton : MonoBehaviour
{
	// Instantiate sound clips
	private AudioClip mHover;
	//private AudioClip mStartScreenMusic;
	
	// Instantiate text mesh object for text on buttons
	private TextMesh tm;
	private char buttonNumber;
	
	private WorldBehavior world = null;
	private bool Active = false;
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
		buttonNumber = tm.text[0];
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
		if (!Active) {
			tm.fontSize = 50;
		}
	}
	
	
	void OnMouseUp()
	{
		tm.fontSize = 70;
		world.setRounds(buttonNumber);
		isActive = true;
	}
	
	public bool isActive {
		set { Active = value;}
		get { return Active; }
	}
	public void Deactivate () {
		isActive = false;
		tm.fontSize = 50;
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
