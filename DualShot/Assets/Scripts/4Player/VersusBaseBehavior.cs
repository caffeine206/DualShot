using UnityEngine;
using System.Collections;

public class VersusBaseBehavior : MonoBehaviour {
	private GameObject smallExplosion = null;
	private GameObject bigExplosion = null;
	private GameObject smallExplosion2 = null;
	private GameObject bigExplosion2 = null;
	private GameObject fire = null;
	private GameObject f = null;
	
	private const float HEALTH = 100f;
	private float currentHealth = HEALTH;
	
	public bool isInvulnerable = false;
	public bool alive;
	
	private SpriteManager spriteMan = null;
	private int numSprites = 24; // This is the total number of sprites, includes the final once which will is the dieing sprite
	private SpriteActionDefinition BaseAnimation = new SpriteActionDefinition( 0, 0, 1, 5f, false);
	
	public Camera mCamera = null;
	
	private AudioClip mBaseHit;
	private AudioClip mBaseDead;
	
	
	// Targets for the Round Counters
	
	private GameObject blueRoundWin, orangeRoundWin;
	
	void Start () {
		alive = true;
		Camera mcamera = Camera.main;
		float aspectSize = mcamera.aspect * mcamera.orthographicSize;
		if (mCamera == null) {
			mCamera = Camera.main;
		}
		if (spriteMan == null) {
			spriteMan = GetComponent<SpriteManager>();
		}
		
		spriteMan.mCurrentSpriteAction = BaseAnimation;
		
		float sizeY = mCamera.orthographicSize * 0.735f;
		
		if ( gameObject.name == "OrangeCity") {
			transform.position = new Vector3(-sizeY, sizeY );
		} else if (gameObject.name == "BlueCity"){
			transform.position = new Vector3(sizeY, -sizeY);
		} else if (gameObject.name == "ChartreuseCity"){
			transform.position = new Vector3(sizeY , sizeY);
		} else if (gameObject.name == "PeriwinkleCity"){
			transform.position = new Vector3(-sizeY, -sizeY);
		}
		
		if (smallExplosion == null) {
			smallExplosion = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
		
		if (bigExplosion == null) {
			bigExplosion = Resources.Load("Prefabs/BigExplosionParticle") as GameObject;
		}
		
		if (smallExplosion2 == null) {
			smallExplosion2 = Resources.Load("Prefabs/SmallExplosionParticle") as GameObject;
		}
		
		if (bigExplosion2 == null) {
			bigExplosion2 = Resources.Load("Prefabs/BigExplosionParticle") as GameObject;
		}
		
		// Audio clip
		mBaseHit = (AudioClip)Resources.Load("Sounds/OrbOnCity");
		mBaseDead = (AudioClip)Resources.Load("Sounds/city explosion");
		
		if (fire == null) {
			fire = Resources.Load("Prefabs/Fire") as GameObject;
		}
	}
	
	void Update () {
		if ( currentHealth / HEALTH < 1 - ((float)spriteMan.SpriteNum / ((float)numSprites - 1)) && spriteMan.SpriteNum < numSprites - 2)
		{
			spriteMan.nextSprite();
		}
		if (alive) {
			Win();
		}
	}
	
	void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Orb1(Clone)" || other.gameObject.name == "Orb(Clone)"
            || other.gameObject.name == "Orb2(Clone)" || other.gameObject.name == "Orb3(Clone)"
             || other.gameObject.name == "Orb4(Clone)" && !isInvulnerable){
			WorldBehavior bounds = GameObject.Find("GameManager").GetComponent<WorldBehavior>();
			currentHealth -= ((other.gameObject.rigidbody2D.velocity.magnitude * other.gameObject.rigidbody2D.mass) / 100.0f);
		
			// Experimental audio play
			//GameObject go = new GameObject("Audio: " + mBaseHit.name);
			//AudioSource source = go.AddComponent<AudioSource>();
			//source.volume = 1;
			//source.Play();
			//Destroy(go, mBaseHit.length);
			Play(mBaseHit, 1f, 1);
			Destroy(other.gameObject);
			AsteroidSpawner spawner = GameObject.Find ("GameManager").GetComponent<AsteroidSpawner>();
			spawner.Orbs--;
			
			FireControl();
		}
	}
	
	private void FireControl() {
		if (f == null) {
			f = Instantiate(fire) as GameObject;
			f.transform.position = transform.position;
			
			if (gameObject.name == "OrangeCity") {
				f.transform.position += new Vector3(-5f, 0f, -1f);
				f.transform.Rotate(Vector3.up * 90f);
			} else if (gameObject.name == "BlueCity") {
				f.transform.position += new Vector3(5f, 0f, -1f);
				f.transform.Rotate(Vector3.up * -90f);
			} else if (gameObject.name == "ChartreuseCity") {
				f.transform.position += new Vector3(0f, 5f, -1f);
				f.transform.Rotate(Vector3.up * 180f);
			} else if (gameObject.name == "PeriwinkleCity") {
				f.transform.position += new Vector3(0f, -5f, -1f);
			}
		}
		
		if (currentHealth < 90f && currentHealth >= 70f) {
			f.particleSystem.startSize = 10f;
		} else if (currentHealth < 70f && currentHealth >= 50f) {
			f.particleSystem.startSize = 13f;
		} else if (currentHealth < 50f && currentHealth >= 30f) {
			f.particleSystem.startSize = 22f;
			f.particleSystem.startLifetime = 0.6f;
		} else if (currentHealth < 30f && currentHealth >= 10f) {
			f.particleSystem.startSize = 28f;
			f.particleSystem.startLifetime = 0.4f;
		} else if (currentHealth < 10f) {
			f.particleSystem.startSize = 35f;
			f.particleSystem.startLifetime = 0.2f;
		}
	}
	
	private void Win() {
		if (currentHealth <= 0f) {
			alive = false;
			
			f.particleSystem.enableEmission = false; // disable fire
			spriteMan.nextSprite();
			StartCoroutine("EXPLOSIVE_VICTORY");
			
			
			/*
            Play(mBaseDead, 1f, 1);

            // Testing for win screen
            if (gameObject.name == "OrangeCity")
            {
                Application.LoadLevel(2);
            }

            // If Blue base destroyed, load Level 3
            if (gameObject.name == "BlueCity")
            {
                Application.LoadLevel(3);
            }

            */
			dies();
		}
	}
	
	private void dies() {
		VersusWorldBehavior world = GameObject.Find ("GameManager").GetComponent<VersusWorldBehavior>();
		if(gameObject.name == "OrangeCity") {
			world.BaseDies(1);
		} else if(gameObject.name == "BlueCity") {
			world.BaseDies(2);
		} else if(gameObject.name == "ChartreuseCity") {
			world.BaseDies(3);
		} else if(gameObject.name == "PeriwinkleCity") {
			world.BaseDies(4);
		}
		GetComponent<CircleCollider2D>().center += new Vector2(100f,100f);
		
	}
	
	//For testing purposes.
	IEnumerator EXPLOSIVE_VICTORY() {
		GameObject s = Instantiate(smallExplosion) as GameObject;
		
		Play(mBaseDead, 1f, 1);
		
		s.transform.position = transform.position;
		yield return new WaitForSeconds(0.5f);
		GameObject b = Instantiate(bigExplosion) as GameObject;
		b.transform.position = transform.position;
		yield return new WaitForSeconds(1f);
		GameObject b2 = Instantiate(bigExplosion2) as GameObject;
		b2.transform.position = transform.position;
		yield return new WaitForSeconds(2f);
		GameObject s2 = Instantiate(smallExplosion2) as GameObject;
		s2.transform.position = transform.position;
		
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
		for( int i = 1; i < sprites.Length; ++i) {
			sprites[i].enabled = false;
		}
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
	
	public void Suicide() {
		currentHealth = 0f;
		Win();
	}
}
