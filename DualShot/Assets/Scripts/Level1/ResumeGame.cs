using UnityEngine;
using System.Collections;

public class ResumeGame : MonoBehaviour {

	RespawnBehavior resume;
	private AudioClip mBaseHit;

	void Start () {
		resume = GameObject.Find("GameManager").GetComponent<RespawnBehavior>();
		mBaseHit = (AudioClip)Resources.Load("Sounds/BaseHit");
	}
	
	void Update () {
		
	}

	void OnMouseUp() {
		resume.Pause();
	}

	void OnMouseEnter() {
		Play(mBaseHit, 1f, 1);
	}

	void OnMouseOver() {
        renderer.material.color = Color.green;
    }

	void OnMouseExit()
    {
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
