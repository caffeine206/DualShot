/*
 * Controls the ship with a keyboard.
*/

using UnityEngine;	
using System.Collections;

public class KeyboardControl : MonoBehaviour {

	Ship theShip;
	public float kHeroSpeed = 140f;
	private Vector3 mClampedPosition;

	private PlaySound playme;       // For initiation of playing sounds
    private AudioClip mBackground;  // "music by audionautix.com"
	
	void Start () {
        // Audio Files setup
        mBackground = (AudioClip)Resources.Load("Sounds/DeepSpaceY");
        //playme.Play(mBackground, 1f, 1);
        Play(mBackground, 1f, 1);
		theShip = gameObject.GetComponent<Ship>();
	}

	void Update () {
		// New Movement
		if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f ||
			Input.GetAxis ("Vertical") > 0.1f || Input.GetAxis ("Vertical") < -0.1f) {
			Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			rigidbody2D.AddForce ( move.normalized * kHeroSpeed);
			}

        // Boundary control
		BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
		boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
		
        // Ship mouse aim
		Vector2 mousedir;
		mousedir = boundsControl.mMainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		mousedir.Normalize();
		transform.up = mousedir;
	} // End of update()

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
