using UnityEngine;
using System.Collections;

public class ResumeGame : MonoBehaviour {

	RespawnBehavior resume;
    private AudioClip mHover;
    private TextMesh tm;

	void Start () {
		resume = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
        mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");
        tm = GetComponent<TextMesh>();
	}
	
	void Update () {
		
	}

	void OnMouseUp() {
		resume.Pause();
	}

	void OnMouseEnter() {
        tm.fontSize = 55;   // Changes font size
		Play(mHover, 1f, 1);
	}

	void OnMouseOver() {

    }

	void OnMouseExit()
    {
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
