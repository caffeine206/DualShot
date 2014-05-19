/*
 * Controls the stick with a joystick.
*/

using UnityEngine;
using System.Collections;

public class JoyStickControl : MonoBehaviour {

	Ship theShip;
    public float kHeroSpeed = 140f;
    private Vector3 mClampedPosition;
    private Vector3 mNewDirection;
    private Vector3 mNewRotation;
    private Vector3 mLastDirection;
    private Vector3 mDefaultDirection;  // Only needed for right joystick control

    private PlaySound playme;       // For initiation of playing sounds
    private AudioClip mBackground;  // "music by audionautix.com"

	// Use this for initialization
    void Start() {
        // Audio Files setup
        mBackground = (AudioClip)Resources.Load("Sounds/DeepSpaceY");
		Play (mBackground, 1f, 1);
        theShip = gameObject.GetComponent<Ship>();
	}
	
	// Update is called once per frame
    void Update()
    {
        // Player movement
        Vector2 move = new Vector2(Input.GetAxis("P2Horizontal"), Input.GetAxis("P2Vertical"));
        rigidbody2D.AddForce(move.normalized * kHeroSpeed);

        // Right Stick Aim
        transform.up = new Vector3(Input.GetAxis("P2RHorz"), Input.GetAxis("P2RVert"), 0);
        mLastDirection = transform.up;
        if (Input.GetAxis("P2RHorz") < 0.3f && Input.GetAxis("P2RHorz") > -0.3f &&
            Input.GetAxis("P2RVert") < 0.3f && Input.GetAxis("P2RVert") > -0.3f)
        { transform.up = mLastDirection.normalized; }

        // Ship clamped to world
        BoundsControl boundsControl = GameObject.Find("GameManager").GetComponent<BoundsControl>();
        boundsControl.ClampAtWorldBounds(this.gameObject, this.renderer.bounds);
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
