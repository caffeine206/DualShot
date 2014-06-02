using UnityEngine;
using System.Collections;

public class LoadMenu : MonoBehaviour {

	private AudioClip mHover;
    private TextMesh tm;

	void Start () {
        mHover = (AudioClip)Resources.Load("Sounds/DaikoSingle");
        tm = GetComponent<TextMesh>();
	}
	void Update () {
	
	}

    // To highlight button text when mouse is over collider
    void OnMouseOver()
    {
        
    }
	
	void OnMouseEnter() {
        tm.fontSize = 55;
		Play(mHover, 0.25f, 1);
		renderer.material.color = new Color(95, 153, 207, 255);
	}
	
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        tm.fontSize = 50;
        renderer.material.color = Color.white;
    }

    void OnMouseUp()
    {
		Time.timeScale = 1f;
        Application.LoadLevel(0);  // Menu
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
