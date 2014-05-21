using UnityEngine;
using System.Collections;

public class BoundsControl : MonoBehaviour {

	// Vars for Asteroid Spawning
	private float mSpawnTime = 10f;
	private float mMinSpawnTime = 3f;
	private int mMaxOrbs = 20;
	public int mSpawnNum = 6;
	public int mSpawnMinSize = 1;
	public int mSpawnMaxSize = 15;
	public float mSpawnSpread = 20f;
	public float mSpawnSpeed = 30f;
	public float mSpawnStagger = 10f;
	
	private float mRampInterval = 15f;
	private float mlastRamp;
	private float mTimeInterval = 0.25f;
	private float mMassInterval = 5f;
	
	public GameObject mOrb = null;
	//public GUIText mEcho = null;
	protected float mMass = 1f;
	protected float mVelocity = 50f;
	
	protected int mCurOrbs;
	protected float mLastSpawn = 0f; // The last time that orbs were spawned


	#region World Bound support
	// Top, Bot, Left, and Right are built to provide
	// walls with collisions on them.
	protected BoxCollider2D Top, Bot, Left, Right;
	protected Bounds mWorldBound;  // this is the world bound
    protected Vector2 mWorldMin;	// Better support 2D interactions
    protected Vector2 mWorldMax;
    protected Vector2 mWorldCenter;
   	public Camera mMainCamera;
	#endregion
	
	// Global Manager
    protected static GlobalBehavior sTheGameState = null;
    private RespawnBehavior pause = null;
	
	// Use this for initialization
	void Start () {
		if (mOrb == null) {
			mOrb = (GameObject) Resources.Load ("Prefabs/Orb");                
		}
		if (pause == null) {
			pause = GetComponent<RespawnBehavior>();
		}
		mlastRamp = Time.realtimeSinceStartup;
		/*
		if (mEcho == null) {
			mEcho = GameObject.Find ("GUI Text").GetComponent<GUIText>();
		}
		mCurOrbs = 0;
		*/
		#region World Bounds
		mMainCamera = Camera.main;
		mWorldBound = new Bounds (Vector3.zero, Vector3.one);
		UpdateWorldBound ();
		#endregion	
		
		SpawnOrbs();
	}
	
	// Update is called once per frame
	void Update () {
		#region Orb Spawner Logic
		//Disabled for demo.
		/*
		if ( Input.GetKeyDown(KeyCode.F2)) { // Manual Spawner
			SpawnOrbs();
		}
		*/

		// Automated spawner. Uses the metrics of time since last spawn and limits the number of orbs on the screen.
		 
		if ( Time.realtimeSinceStartup - mlastRamp > mRampInterval ) {
			mlastRamp = Time.realtimeSinceStartup;
			/*if ( mSpawnTime > mMinSpawnTime ) { 
				mSpawnTime -= mTimeInterval;
				}*/
			mSpawnMinSize += mMassInterval;
		}
		
		if ( Time.realtimeSinceStartup - mLastSpawn > mSpawnTime && mCurOrbs < mMaxOrbs && !pause.GameIsPaused() ) {
			SpawnOrbs();
			mLastSpawn = Time.realtimeSinceStartup;
			//Added this to increase wave frequency.
			//mSpawnTime = mSpawnTime - 2f;

			//if (mSpawnTime < 0f) {
			//	mSpawnTime = 0f;
			//}
		}
		#endregion
		/*
		mMass += Input.GetAxis ("Vertical");
		if (mMass < 1) {
			mMass = 1;
		}

		mVelocity += Input.GetAxis ("Horizontal");
		if (mVelocity < 1) {
			mVelocity = 1;
		}

		if (Input.GetButtonDown ("Fire1")) {
			GameObject e = (GameObject) Instantiate(mOrb);
			e.transform.position = mWorldMin + Vector2.one * 5f;
			e.transform.up = new Vector2(Random.Range (0f,1f), Random.Range(0f, 1f));
			e.rigidbody2D.mass = mMass;
			e.rigidbody2D.AddForce(e.transform.up * mVelocity * mMass);
		}*/

		//mEcho.text = "Total Orbs: " + mCurOrbs + "\ncur < max: " + (mCurOrbs < mMaxOrbs);
		
		//reset();
		
		//Added
		Debug.Log("Current Orbs: " + mCurOrbs + " Spawn Time: " + mSpawnTime);
	}

	/*
	private void reset() {
		if (Input.GetKey(KeyCode.Return)) {
			mCurOrbs = 0;
		}
	}
	*/

	void Awake() {
		if (null == sTheGameState) { // not here yet
			CreateGlobalManager();
		}
	}
	
	#region WorldBounds

    public enum WorldBoundStatus
    {
		CollideTop,
		CollideLeft,
		CollideRight,
		CollideBottom,
		Outside,
		Inside
	};

	
	public void UpdateWorldBound() 
	{
		if (mMainCamera != null) {
			float maxY = mMainCamera.orthographicSize;
			float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs (mMainCamera.farClipPlane - mMainCamera.nearClipPlane);
			
			// set z to zero
			Vector3 center = mMainCamera.transform.position;
			center.z = 0.0f;
			
			// set bounds
			mWorldBound.center = center;
			mWorldBound.size = new Vector3 (sizeX, sizeY, sizeZ);
			
			mWorldCenter = new Vector2 (center.x, center.y);
			mWorldMin = new Vector2 (mWorldBound.min.x, mWorldBound.min.y);
			mWorldMax = new Vector2 (mWorldBound.max.x, mWorldBound.max.y);

			Top = GameObject.Find ("Top").GetComponent<BoxCollider2D>();
			Top.center = new Vector2(mWorldCenter.x, mWorldMax.y);
			Top.size = new Vector2(sizeX,10);

			Bot = GameObject.Find ("Bot").GetComponent<BoxCollider2D>();
			Bot.center = new Vector2(mWorldCenter.x, mWorldMin.y);
			Bot.size = new Vector2(sizeX,10);

			Right = GameObject.Find ("Right").GetComponent<BoxCollider2D>();
			Right.center = new Vector2(mWorldMax.x, mWorldCenter.y);
			Right.size = new Vector2(10, sizeY);

			Left = GameObject.Find ("Left").GetComponent<BoxCollider2D>();
			Left.center = new Vector2(mWorldMin.x, mWorldCenter.y);
			Left.size = new Vector2(10, sizeY);
			
		}
	}
	
	public Vector2 WorldCenter { get { return mWorldCenter; } }
	public Vector2 WorldMin { get { return mWorldMin; }} 
	public Vector2 WorldMax { get { return mWorldMax; }}
	
	public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
	{
		WorldBoundStatus status = WorldBoundStatus.Inside;
		
		if (mWorldBound.Intersects(objBound)) {
			if (objBound.max.x > mWorldBound.max.x)
				status = WorldBoundStatus.CollideRight;
			else if (objBound.min.x < mWorldBound.min.x)
				status = WorldBoundStatus.CollideLeft;
			else if (objBound.max.y > mWorldBound.max.y)
				status = WorldBoundStatus.CollideTop;
			else if (objBound.min.y < mWorldBound.min.y)
				status = WorldBoundStatus.CollideBottom;
			else if ( (objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
				status = WorldBoundStatus.Outside;
		} else 
			status = WorldBoundStatus.Outside;
		
		return status;
		
	}
	public void ClampAtWorldBounds(GameObject obj, Bounds objBound)
	{
		
		{
			Vector3 pos = obj.transform.position;
			Vector2 objMax = WorldMax - (Vector2)objBound.extents;
			Vector2 objMin = WorldMin + (Vector2)objBound.extents;
	
			if (pos.x > objMax.x)
				pos.x = objMax.x;
			if (pos.x < objMin.x)
				pos.x = objMin.x;
			if (pos.y > objMax.y)
				pos.y = objMax.y;
			if (pos.y < objMin.y)
				pos.y = objMin.y;
			
			obj.transform.position = pos;
		}
	}
	#endregion
	
	#region Global
	
	private static void CreateGlobalManager()
	{
		GameObject newGameState = new GameObject();
		newGameState.name = "GlobalStateManager";
		newGameState.AddComponent<GlobalBehavior>();
		sTheGameState = newGameState.GetComponent<GlobalBehavior>();
	}

	public static GlobalBehavior TheGameState { get {
		if (null == sTheGameState)
			CreateGlobalManager();
			return sTheGameState;
		} }
	
	#endregion

	#region Asteroid Spawner
	
	public int Orbs {
		get { return mCurOrbs; }
		set { mCurOrbs = value; }
	}
	
	private void SpawnOrbs() {
		int topNum, botNum; // Number spawning from top/bottom
		topNum = mSpawnNum/2;
		botNum = mSpawnNum - topNum;
		
		for(int i = 0; i < topNum; ++i) {
			Vector2 spawnPoint = new Vector2( Random.Range(-mSpawnSpread, mSpawnSpread), mWorldMax.y + (i + 1) * mSpawnStagger);
			Vector2 spawnDir = new Vector2( Random.Range (-mSpawnSpread / 2f, mSpawnSpread / 2f), -mSpawnSpeed);
			float spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}
		
		for(int i = 0; i < botNum; ++i) {
			Vector2 spawnPoint = new Vector2( Random.Range(-mSpawnSpread, mSpawnSpread), mWorldMin.y - (i + 1) * mSpawnStagger);
			Vector2 spawnDir = new Vector2( Random.Range (-mSpawnSpread / 2f, mSpawnSpread / 2f), mSpawnSpeed);
			float spawnMass = Random.Range (mSpawnMinSize, mSpawnMaxSize);
			ThrowOrb(spawnPoint, spawnDir, spawnMass);
		}

		//Added
	}
	
	private void ThrowOrb(Vector2 pos, Vector2 dir, float mass) {
		GameObject e = (GameObject) Instantiate(mOrb);
		e.rigidbody2D.mass = mass;
		e.transform.position = pos;
		e.rigidbody2D.velocity = dir;
		e.transform.up = dir.normalized;
		e.transform.localScale = mass * Vector2.one;
		e.collider2D.isTrigger = true;
	}

	#endregion

}

