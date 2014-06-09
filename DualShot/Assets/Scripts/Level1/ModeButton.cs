using UnityEngine;
using System.Collections;

public class ModeButton : MonoBehaviour {
	
	private AudioClip mHover;
	private TextMesh tm;
	public int lvlNum;
	private bool Active = false;
	
	void Start () {
		mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");
		tm = GetComponent<TextMesh>();

        // Set Mode: 1 vs 1 & Best of: 1 Round to active
        if(gameObject.name == "Setup-Mode1")
        {
            Active = true;
            tm.fontSize = 70;
            renderer.material.color = new Color(95, 153, 207, 255);
        }
	}
	
	void OnMouseEnter() {
		//tm.fontStyle = FontStyle.Bold;
        if (!Active)
        { tm.fontSize = 55; }
        Play(mHover, 0.5f, 1);
	}
	
	// To de-highlight button text when mouse is over collider
	void OnMouseExit()
	{
		//renderer.material.color = Color.white;
		tm.fontStyle = FontStyle.Normal;
		if (!Active) {
			tm.fontSize = 50;
		} else  {
			tm.fontSize = 70;
		}
	}
	
	void OnMouseUp()
	{
		//GlobalBehavior glob = GameObject.Find ("GameStateManager").GetComponent<GlobalBehavior>();
		//glob.SetCurrentLevel("Dual Level");
		WorldBehavior world = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
		world.mode = lvlNum;
		Active = true;
		tm.fontSize = 70;
		renderer.material.color = new Color(95, 153, 207, 255);
	}

	public bool isActive {
		set {
			Active = value;
		}
		get { return Active; }
	}

	public void Deactivate () {
		isActive = false;
		tm.fontSize = 50;
		renderer.material.color = Color.white;
	}
	
	// Audio clip player
	public void Play(AudioClip clip, float volume, float pitch) {
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