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
		if (lvlNum == 1) {
			Active = true;
		}
		
	}
	
	void OnMouseEnter() {
		//tm.fontStyle = FontStyle.Bold;
		tm.fontSize = 55;
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
	}
	public void Deactivate () {
		Active = false;
		tm.fontSize = 50;
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