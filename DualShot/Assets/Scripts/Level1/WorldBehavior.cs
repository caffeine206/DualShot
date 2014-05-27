using UnityEngine;
using System.Collections;

public class WorldBehavior : MonoBehaviour {


	#region World Bound support
	// Top, Bot, Left, and Right are built to provide
	// walls with collisions on them.
	protected BoxCollider2D Top, Bot, Left, Right, BotLeftCollider, BotRightCollider, TopLeftCollider, TopRightCollider;
	protected GameObject BotRight, BotLeft, TopRight, TopLeft;
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
	//sTheGameState.
		
		if (pause == null) {
			pause = GetComponent<RespawnBehavior>();
		}
		
		/*
		if (mEcho == null) {
			mEcho = GameObject.Find ("GUI Text").GetComponent<GUIText>();
		}
		mCurOrbs = 0;
		*/
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.F7)) {
			sTheGameState.BlueWins++;
		}
		if (Input.GetKeyDown (KeyCode.F8)) {
			sTheGameState.OrangeWins++;
		}
	
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
		#region World Bounds
		mMainCamera = Camera.main;
		mWorldBound = new Bounds (Vector3.zero, Vector3.one);
		UpdateWorldBound ();
		#endregion	
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
			Top.size = new Vector2(sizeX,20);

			Bot = GameObject.Find ("Bot").GetComponent<BoxCollider2D>();
			Bot.center = new Vector2(mWorldCenter.x, mWorldMin.y);
			Bot.size = new Vector2(sizeX,20);

			Right = GameObject.Find ("Right").GetComponent<BoxCollider2D>();
			Right.center = new Vector2(mWorldMax.x, mWorldCenter.y);
			Right.size = new Vector2(15, sizeY);

			Left = GameObject.Find ("Left").GetComponent<BoxCollider2D>();
			Left.center = new Vector2(mWorldMin.x, mWorldCenter.y);
			Left.size = new Vector2(15, sizeY);

			TopLeft = GameObject.Find ("TopLeft");
			TopLeft.transform.position = new Vector2(mWorldMin.x, mWorldMax.y);
			TopLeft.transform.Rotate(Vector3.forward * 45.0f);
			TopLeftCollider = GameObject.Find ("TopLeft").GetComponent<BoxCollider2D>();
			TopLeftCollider.size = new Vector2(60,60);
			
			TopRight = GameObject.Find ("TopRight");
			TopRight.transform.position = new Vector2(mWorldMax.x, mWorldMax.y);
			TopRight.transform.Rotate(Vector3.forward * 45.0f);
			TopRightCollider = GameObject.Find ("TopRight").GetComponent<BoxCollider2D>();
			TopRightCollider.size = new Vector2(60,60);
			
			BotLeft = GameObject.Find ("BotLeft");
			BotLeft.transform.position = new Vector2(mWorldMin.x, mWorldMin.y);
			BotLeft.transform.Rotate(Vector3.forward * 45.0f);
			BotLeftCollider = GameObject.Find ("BotLeft").GetComponent<BoxCollider2D>();
			BotLeftCollider.size = new Vector2(60,60);

			BotRight = GameObject.Find ("BotRight");
			BotRight.transform.position = new Vector2(mWorldMax.x, mWorldMin.y);
			BotRight.transform.Rotate(Vector3.forward * 45.0f);
			BotRightCollider = GameObject.Find ("BotRight").GetComponent<BoxCollider2D>();
			BotRightCollider.size = new Vector2(60,60);
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
	public void RoundEnd(int winner) {
		sTheGameState.RoundNum++;
		
		if ( winner == 2 ) {
			sTheGameState.BlueWins++;
			if (sTheGameState.BlueWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner);
			} else {
				Application.LoadLevel (1);
			}
		} else {
			sTheGameState.OrangeWins++;
			if (sTheGameState.OrangeWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner);
			} else {
				Application.LoadLevel (1);
			}
		}
	}
	public void resetScore() {
		sTheGameState.BlueWins = 0;
		sTheGameState.OrangeWins = 0;
	}
	public void setRounds( string value ) {
		RoundButton lastActive = GameObject.Find ("BestOf" + sTheGameState.BestOf ).GetComponent<RoundButton>();
		lastActive.Deactivate();
		sTheGameState.BestOf = int.Parse(value);
	}
	
	#endregion


}

