using UnityEngine;
using System.Collections;

public class StartLevel1 : MonoBehaviour {

    private AudioClip mBaseHit;

	void Start () {
		mBaseHit = (AudioClip)Resources.Load("Sounds/BaseHit");
	}
	
	void Update () {
	
	}

    // To highlight button text when mouse is over collider
    void OnMouseOver()
    {
        renderer.material.color = Color.green;
    }
	
	void OnMouseEnter() {
		Play(mBaseHit, 1f, 1);
	}
	
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }

    void OnMouseUp()
    {
        Application.LoadLevel(1);
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
