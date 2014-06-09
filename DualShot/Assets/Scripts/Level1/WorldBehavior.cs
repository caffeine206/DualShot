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
    protected RespawnBehavior pause = null;
    protected GameObject blueRoundWin, orangeRoundWin;
	
	// Targets for the Round Counters
	protected Vector3	BluePoint1, BluePoint2, BluePoint3,
	OrangePoint1, OrangePoint2, OrangePoint3;
						
	// Use this for initialization
	void Start () {
	//sTheGameState.
		if (pause == null) {
			pause = GetComponent<RespawnBehavior>();
		}
		if (blueRoundWin == null) {
			blueRoundWin = (GameObject)Resources.Load ("Prefabs/BlueRoundCounter");
		}
		if (orangeRoundWin == null) {
			orangeRoundWin = (GameObject)Resources.Load ("Prefabs/OrangeRoundCounter");
		}
		
		// setup round counters
		
		float aspectSize = mMainCamera.aspect * mMainCamera.orthographicSize;
		BluePoint1 = new Vector3( aspectSize * .95f, 92f); 
		BluePoint2 = new Vector3( aspectSize * .885f, 92f);
		BluePoint3 = new Vector3( aspectSize * .95f, 80f);
		OrangePoint1 = new Vector3( -aspectSize * .95f, 92f);
		OrangePoint2 = new Vector3( -aspectSize * .885f, 92f);
		OrangePoint3 = new Vector3( -aspectSize * .95f, 80f);
		
		RoundCounterBehavior counter;	
	
		// Orange counter logic
		counter = spawnOrangeCounter(OrangePoint1);
		if (sTheGameState.OrangeWins <= 0) {
			counter.makeFrame();
		}
		if (sTheGameState.BestOf > 1) {
			counter = spawnOrangeCounter(OrangePoint2);
		}
		if (sTheGameState.OrangeWins <= 1) {
			counter.makeFrame();
		}
		if (sTheGameState.BestOf > 3) {
			counter = spawnOrangeCounter(OrangePoint3);
		}
		counter.makeFrame();

		// Blue counter logic
		counter = spawnBlueCounter(BluePoint1);
		if (sTheGameState.BlueWins <= 0) {
			counter.makeFrame();
		}
		if (sTheGameState.BestOf > 1) {
			counter = spawnBlueCounter(BluePoint2);
		}
		if (sTheGameState.BlueWins <= 1) {
			counter.makeFrame();
		}
		if (sTheGameState.BestOf > 3) {
			counter = spawnBlueCounter(BluePoint3);
		}
		counter.makeFrame();
		
		
		
		/*
		if (mEcho == null) {
			mEcho = GameObject.Find ("GUI Text").GetComponent<GUIText>();
		}
		*/		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.F7)) {
			sTheGameState.BlueWins++;
		}
		if (Input.GetKeyDown (KeyCode.F8)) {
			sTheGameState.OrangeWins++;
		}
		*/
		
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

	void Awake() {
		if (null == sTheGameState) { // not here yet
			CreateGlobalManager();
		}
		
		mMainCamera = Camera.main;
		mWorldBound = new Bounds (Vector3.zero, Vector3.one);
		UpdateWorldBound ();
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

	public virtual void UpdateWorldBound() 
	{
		if (mMainCamera != null) {
			float maxY = mMainCamera.orthographicSize;
			float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs (mMainCamera.farClipPlane - mMainCamera.nearClipPlane);
            float cornerSize = 60f;
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
			Top.size = new Vector2(sizeX,10f);

			Bot = GameObject.Find ("Bot").GetComponent<BoxCollider2D>();
			Bot.center = new Vector2(mWorldCenter.x, mWorldMin.y);
			Bot.size = new Vector2(sizeX,10f);

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
            TopLeftCollider.size = new Vector2(cornerSize, cornerSize);
			
			TopRight = GameObject.Find ("TopRight");
			TopRight.transform.position = new Vector2(mWorldMax.x, mWorldMax.y);
			TopRight.transform.Rotate(Vector3.forward * 45.0f);
			TopRightCollider = GameObject.Find ("TopRight").GetComponent<BoxCollider2D>();
            TopRightCollider.size = new Vector2(cornerSize, cornerSize);
			
			BotLeft = GameObject.Find ("BotLeft");
			BotLeft.transform.position = new Vector2(mWorldMin.x, mWorldMin.y);
			BotLeft.transform.Rotate(Vector3.forward * 45.0f);
			BotLeftCollider = GameObject.Find ("BotLeft").GetComponent<BoxCollider2D>();
            BotLeftCollider.size = new Vector2(cornerSize, cornerSize);

			BotRight = GameObject.Find ("BotRight");
			BotRight.transform.position = new Vector2(mWorldMax.x, mWorldMin.y);
			BotRight.transform.Rotate(Vector3.forward * 45.0f);
			BotRightCollider = GameObject.Find ("BotRight").GetComponent<BoxCollider2D>();
            BotRightCollider.size = new Vector2(cornerSize, cornerSize);
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
	
	protected static void CreateGlobalManager()
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
	public virtual void RoundEnd(int winner) {
		sTheGameState.RoundNum++;
		if ( winner == 2 ) {
			sTheGameState.BlueWins++;
            // Wins X out of Y rounds (1 out of 1, 2 out of 3, 3 out of 5)
            if (sTheGameState.BlueWins >= 1 && sTheGameState.BestOf == 1 ||
                sTheGameState.BlueWins >= 2 && sTheGameState.BestOf == 3 ||
                sTheGameState.BlueWins >= 3 && sTheGameState.BestOf == 5 )
            { Application.LoadLevel (5); }
            else
            { Application.LoadLevel (mode); }
		}
        else {
			sTheGameState.OrangeWins++;
            // Wins X out of Y rounds (1 out of 1, 2 out of 3, 3 out of 5)
            if (sTheGameState.OrangeWins >= 1 && sTheGameState.BestOf == 1 ||
                sTheGameState.OrangeWins >= 2 && sTheGameState.BestOf == 3 ||
                sTheGameState.OrangeWins >= 3 && sTheGameState.BestOf == 5 )
            { Application.LoadLevel(4); }
            else
            { Application.LoadLevel(mode); }
		}
	}
	public void setRounds( char value ) {
		RoundButton lastActive = GameObject.Find ("Setup-Rounds" + sTheGameState.BestOf + "Button" ).GetComponent<RoundButton>();
		lastActive.Deactivate();
		sTheGameState.BestOf = (int)(value) - 48;
	}
	public int blueScore() {
		return sTheGameState.BlueWins;
	}
	public int orangeScore() {
		return sTheGameState.OrangeWins;
	}
	public void resetScore() {
		sTheGameState.resetScores();
	}
	protected RoundCounterBehavior spawnBlueCounter(Vector3 target) {
		GameObject e = (GameObject)	Instantiate(blueRoundWin);
		e.transform.position = target;
		e.GetComponent<RoundCounterBehavior>().mTargetPos = target;
		return e.GetComponent<RoundCounterBehavior>();
	}
	protected RoundCounterBehavior spawnOrangeCounter(Vector3 target) {
		GameObject e = (GameObject)	Instantiate(orangeRoundWin);
		e.transform.position = target;
		e.GetComponent<RoundCounterBehavior>().mTargetPos = target;
		return e.GetComponent<RoundCounterBehavior>();
	}
	public void setupShip (Ship ship) {
		sTheGameState.setupShip(ship);
	}
	public void setupControls(int keyboardNum) {
		if (keyboardNum == 0) {
			sTheGameState.disableKeyboard();
		} else {
			sTheGameState.setKeyboard(keyboardNum);	
		}
	}
	public int mode {
		get {return sTheGameState.mode;}
		set {
			ModeButton lastActive = GameObject.Find ("Setup-Mode" + sTheGameState.mode).GetComponent<ModeButton>();
			lastActive.Deactivate();
			sTheGameState.mode = value;
		}	
	}
	public bool isKeyboard( int player ) {
		if (sTheGameState.keyboard == player) {
			return true;
		}
		else
			return false;
	}

	#endregion

}

