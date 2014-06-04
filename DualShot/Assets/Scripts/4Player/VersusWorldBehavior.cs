﻿using UnityEngine;
using System.Collections;

public class VersusWorldBehavior : WorldBehavior {
	
	// Level Control
	protected int mPlayersAlive = 4;
	protected bool[] mIsAlive = new bool[4] {true, true, true, true};
	protected int winner = 0;
	

	private GameObject charRoundWin, PerRoundWin;
	
	// Targets for the Round Counters
	
	private Vector3	ChartreusePoint1, ChartreusePoint2, ChartreusePoint3,
	PeriwinklePoint1, PeriwinklePoint2, PeriwinklePoint3;
	
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
		if (charRoundWin == null) {
			charRoundWin = (GameObject)Resources.Load ("Prefabs/CharRoundCounter");
		}
		if (PerRoundWin == null) {
			PerRoundWin = (GameObject)Resources.Load ("Prefabs/PerRoundCounter");
		}
		
		// setup round counters
		
		float aspectSize = mMainCamera.aspect * mMainCamera.orthographicSize;
		BluePoint1 = new Vector3( 95f, -92f);
		BluePoint2 = new Vector3( 90f, -92f);
		BluePoint3 = new Vector3( 95f, -82f);
		OrangePoint1 = new Vector3( -95f, 92f);
		OrangePoint2 = new Vector3( -90f, 92f);
		OrangePoint3 = new Vector3( -95f, 82f);
		ChartreusePoint1 = new Vector3( 95f, 92f);
		ChartreusePoint2 = new Vector3( 90f, 92f);
		ChartreusePoint3 = new Vector3( 95f, 82f);
		PeriwinklePoint1 = new Vector3( -95f, -92f);
		PeriwinklePoint2 = new Vector3( -90f, -92f);
		PeriwinklePoint3 = new Vector3( -95f, -82f);
		if (sTheGameState.BlueWins > 0) {
			spawnBlueCounter(BluePoint1);
		} 
		if (sTheGameState.BlueWins > 1) {
			spawnBlueCounter(BluePoint2);
		}
		
		if (sTheGameState.OrangeWins > 0) {
			spawnOrangeCounter(OrangePoint1);
		}
		if (sTheGameState.OrangeWins > 1) {
			spawnOrangeCounter(OrangePoint2);
		}
		if (sTheGameState.OrangeWins > 0) {
			spawnOrangeCounter(OrangePoint1);
		}
		if (sTheGameState.OrangeWins > 1) {
			spawnOrangeCounter(OrangePoint2);
		}
		if (sTheGameState.OrangeWins > 0) {
			spawnOrangeCounter(OrangePoint1);
		}
		if (sTheGameState.OrangeWins > 1) {
			spawnOrangeCounter(OrangePoint2);
		}
	}
	public override void UpdateWorldBound() {
		if (mMainCamera != null) {
			float maxY = mMainCamera.orthographicSize;
			float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs (mMainCamera.farClipPlane - mMainCamera.nearClipPlane);
			float boxSize = 10;
			float cornerSize = 36;
			
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
			Top.size = new Vector2(sizeY,boxSize);
			
			Bot = GameObject.Find ("Bot").GetComponent<BoxCollider2D>();
			Bot.center = new Vector2(mWorldCenter.x, mWorldMin.y);
			Bot.size = new Vector2(sizeY,boxSize);
			
			Right = GameObject.Find ("Right").GetComponent<BoxCollider2D>();
			Right.center = new Vector2(mWorldMax.y, mWorldCenter.y);
			Right.size = new Vector2(boxSize, sizeY);
			
			Left = GameObject.Find ("Left").GetComponent<BoxCollider2D>();
			Left.center = new Vector2(mWorldMin.y, mWorldCenter.y);
			Left.size = new Vector2(boxSize, sizeY);
			
			TopLeft = GameObject.Find ("TopLeft");
			TopLeft.transform.position = new Vector2(mWorldMin.y, mWorldMax.y);
			TopLeft.transform.Rotate(Vector3.forward * 45.0f);
			TopLeftCollider = GameObject.Find ("TopLeft").GetComponent<BoxCollider2D>();
			TopLeftCollider.size = new Vector2(cornerSize,cornerSize);
			
			TopRight = GameObject.Find ("TopRight");
			TopRight.transform.position = new Vector2(mWorldMax.y, mWorldMax.y);
			TopRight.transform.Rotate(Vector3.forward * 45.0f);
			TopRightCollider = GameObject.Find ("TopRight").GetComponent<BoxCollider2D>();
			TopRightCollider.size = new Vector2(cornerSize,cornerSize);
			
			BotLeft = GameObject.Find ("BotLeft");
			BotLeft.transform.position = new Vector2(mWorldMin.y, mWorldMin.y);
			BotLeft.transform.Rotate(Vector3.forward * 45.0f);
			BotLeftCollider = GameObject.Find ("BotLeft").GetComponent<BoxCollider2D>();
			BotLeftCollider.size = new Vector2(cornerSize,cornerSize);
			
			BotRight = GameObject.Find ("BotRight");
			BotRight.transform.position = new Vector2(mWorldMax.y, mWorldMin.y);
			BotRight.transform.Rotate(Vector3.forward * 45.0f);
			BotRightCollider = GameObject.Find ("BotRight").GetComponent<BoxCollider2D>();
			BotRightCollider.size = new Vector2(cornerSize,cornerSize);
		}
}
	
	public void BaseDies(int dead) {
		if (mIsAlive[dead - 1]){
			mIsAlive[dead - 1] = false;
			mPlayersAlive--;
		}		
		if (mPlayersAlive <= 1) {
			for (int i = 0; i < 4; ++i) {
				if (mIsAlive[i]){
					RoundEnd(i + 1);
					break;
				}
			}
		}
	}
	
	
	public override void RoundEnd(int wins) {
		sTheGameState.RoundNum++;
		winner = wins;
		
		StartCoroutine("Ending");
		
	}
	IEnumerator Ending() {
		if ( winner == 1 ) {
			GameObject e = (GameObject)	Instantiate(orangeRoundWin);
			e.transform.position = new Vector3(0, -50);
			if (sTheGameState.OrangeWins == 0) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = OrangePoint1;
			} else if (sTheGameState.OrangeWins == 1) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = OrangePoint2;
			} else {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = OrangePoint3;
			}
			
			yield return new WaitForSeconds(4.5f);
			
			sTheGameState.OrangeWins++;
			
			if (sTheGameState.OrangeWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner + 3);
			} else {
				Application.LoadLevel (sTheGameState.mode);
			}
		} else if(winner == 2 ) {
			GameObject e = (GameObject)	Instantiate(blueRoundWin);
			e.transform.position = new Vector3(0, -50);
			if (sTheGameState.BlueWins == 0){
				e.GetComponent<RoundCounterBehavior>().mTargetPos = BluePoint1;
			} else if (sTheGameState.BlueWins == 1) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = BluePoint2;
			} else {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = BluePoint3;
			}
			
			yield return new WaitForSeconds(4.5f);
			
			sTheGameState.BlueWins++;
			if (sTheGameState.BlueWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner + 3);
			} else {
				Application.LoadLevel (sTheGameState.mode);
			}
		} else if(winner == 3 ) {
			GameObject e = (GameObject)	Instantiate(charRoundWin);
			e.transform.position = new Vector3(0, -50);
			if (sTheGameState.CharWins == 0) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = ChartreusePoint1;
			} else if (sTheGameState.CharWins == 1) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = ChartreusePoint2;
			} else {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = ChartreusePoint3;
			}
			
			yield return new WaitForSeconds(4.5f);
			
			sTheGameState.CharWins++;
			
			if (sTheGameState.CharWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner + 3);
			} else {
				Application.LoadLevel (sTheGameState.mode);
			}
		} else if(winner == 4 ) {
			GameObject e = (GameObject)	Instantiate(PerRoundWin);
			e.transform.position = new Vector3(0, -50);
			if (sTheGameState.PerWins == 0) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = PeriwinklePoint1;
			} else if (sTheGameState.PerWins == 1) {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = PeriwinklePoint2;
			} else {
				e.GetComponent<RoundCounterBehavior>().mTargetPos = PeriwinklePoint3;
			}
			
			yield return new WaitForSeconds(4.5f);
			
			sTheGameState.PerWins++;
			
			if (sTheGameState.PerWins >= sTheGameState.BestOf) {
				Application.LoadLevel (winner + 3);
			} else {
				Application.LoadLevel (sTheGameState.mode);
			}
		}
	}
	
	private void spawnCharCounter(Vector3 target) {
		GameObject e = (GameObject)	Instantiate(charRoundWin);
		e.transform.position = target;
		e.GetComponent<RoundCounterBehavior>().mTargetPos = target;
	}
	private void spawnPerCounter(Vector3 target) {
		GameObject e = (GameObject)	Instantiate(PerRoundWin);
		e.transform.position = target;
		e.GetComponent<RoundCounterBehavior>().mTargetPos = target;
	}
	public void setupShip (FourPlayerShip ship) {
		sTheGameState.setupShip(ship);
	}
	
}
