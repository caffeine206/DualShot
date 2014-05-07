using UnityEngine;
using System.Collections;

public class EnergyOrbBehavior : MonoBehaviour {

	public const float mTowardsCenter = 0.0f; 
	// what is the change of enemy flying towards the world center after colliding with world bound
	// 0: no control
	// 1: always towards the world center, no randomness

	private float mSpeed;
	private int hitCount;
	private float kReferenceSpeed;
	private EnergyOrbState mEnergyOrbState;
		
	protected enum EnergyOrbState
	{
		Normal,
		Run,
		Stunned
	}

	// Use this for initialization
	void Start () {
		//transform.position = RandomPointInWorld();
		transform.position = new Vector3(0,0,0);
		NewDirection();	

		kReferenceSpeed = 10.0f;
		mSpeed = kReferenceSpeed;

		mEnergyOrbState = EnergyOrbState.Normal;
		hitCount = 0;
	}

	private Vector3 RandomPointInWorld()
	{
		LocalGameBehavior localGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		float x = Random.Range(localGameBehavior.WorldMin.x, localGameBehavior.WorldMax.x);
		float y = Random.Range(localGameBehavior.WorldMin.y, localGameBehavior.WorldMax.y);
		return new Vector3(x, y, 0f);
	}

	// Update is called once per frame
	void Update () {
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
		LocalGameBehavior localGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		
		LocalGameBehavior.WorldBoundStatus status =
			localGameBehavior.CollideWithWorld(GetComponent<Renderer>().bounds);
		
		if (status != LocalGameBehavior.WorldBoundStatus.Inside) {
			// Debug.Log("collided position: " + this.transform.position);
			/*transform.position = new Vector3(localGameBehavior.WorldMin.x + kReferenceSpeed, 
			                                 localGameBehavior.WorldMin.y + kReferenceSpeed, 0f);*/
			NewDirection();
		}
	}

	private void NewDirection() {
		LocalGameBehavior LocalGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		
		// we want to move towards the center of the world
		Vector2 v = LocalGameBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
		// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direction that is perpendicular to V
		
		float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );
		
		float randomX = Random.Range(0f, 1f);
		float yRange = tanSpread * randomX;
		float randomY = Random.Range (-yRange, yRange);
		
		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		// Wave Blast collision
		if (other.gameObject.name == "WaveBlast(Clone)") {
			Vector3 v =  transform.position - other.gameObject.transform.position;
			v.Normalize();
			Vector3 newDir = v + transform.up;
			newDir.Normalize();
			transform.up = newDir;
			//Destroy(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{

	}

	/*
	public void Play(AudioClip clip, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject ("Audio: " +  clip.name);
		LocalGameBehavior LocalGameBehavior2 = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		go.transform.position = LocalGameBehavior2.transform.position;
		go.transform.parent = LocalGameBehavior2.transform;

		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
		Destroy (go, clip.length);
	}
	*/
}
