using UnityEngine;
using System.Collections;

public class VictoryClap : MonoBehaviour {

    // Instantiate sound clip
    private AudioClip mVictory;

	// Use this for initialization
	void Start () {
        mVictory = (AudioClip)Resources.Load("Sounds/TaikoWinScreen");

        Play(mVictory, 1f, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
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
