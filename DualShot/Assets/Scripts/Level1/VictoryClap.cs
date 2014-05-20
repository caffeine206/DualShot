using UnityEngine;
using System.Collections;

public class VictoryClap : MonoBehaviour {

    private AudioClip mClap;
    private AudioClip mCheer;

	// Use this for initialization
	void Start () {
        mClap = (AudioClip)Resources.Load("Sounds/SmallCrowdClapping");
        mCheer = (AudioClip)Resources.Load("Sounds/Applause");

        int random = Random.Range(0, 9);
        if (random % 2 == 0)
        { Play(mClap, 1f, 1); }
        else
        { Play(mCheer, 1f, 1); }
	
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
