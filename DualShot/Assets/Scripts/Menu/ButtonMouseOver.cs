﻿using UnityEngine;
using System.Collections;

public class ButtonMouseOver : MonoBehaviour {

	private AudioClip mHover;
    private TextMesh tm;

	void Start () {
		mHover = (AudioClip)Resources.Load("Sounds/BaseHit");
        tm = GetComponent<TextMesh>();
	}
	
	void Update () {
	
	}

	void OnMouseEnter() {
        tm.fontSize = 55;
		Play(mHover, 0.25f, 1);
	}
	
    // To de-highlight button text when mouse is over collider
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
