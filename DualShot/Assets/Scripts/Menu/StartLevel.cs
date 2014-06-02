using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {

    private AudioClip mHover;
    private TextMesh tm;
    // indicates level to load on start
    // 1 = 1v1
    // 4 = 2v2
    // 5 = Versus

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
        //tm.fontStyle = FontStyle.Bold;
        tm.fontSize = 55;
		Play(mHover, 0.5f, 1);
	}
	
    // To de-highlight button text when mouse is over collider
    void OnMouseExit()
    {
        //renderer.material.color = Color.white;
        tm.fontStyle = FontStyle.Normal;
        tm.fontSize = 50;
    }

    void OnMouseUp()
    {
    	//GlobalBehavior glob = GameObject.Find ("GameStateManager").GetComponent<GlobalBehavior>();
    	//glob.SetCurrentLevel("Dual Level");
		WorldBehavior world = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
		world.resetScore();
		int mode = world.mode;
        Application.LoadLevel(mode);
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
