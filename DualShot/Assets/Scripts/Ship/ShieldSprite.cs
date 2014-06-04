﻿using UnityEngine;
using System.Collections;

public class ShieldSprite : MonoBehaviour {

	private SpriteManager spriteMan = null;
	private float mFlashTime = 0.1f;
	public float mImpactTime = 0f;

	// Use this for initialization
	void Start () {
	
		if (spriteMan == null) {
			spriteMan = GetComponent<SpriteManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.realtimeSinceStartup - mImpactTime < mFlashTime && spriteMan.SpriteNum < 2) {
			spriteMan.SpriteNum = 2;
			spriteMan.setSprite();
		} else if ( Time.realtimeSinceStartup - mImpactTime < mFlashTime * 1.5f && spriteMan.SpriteNum != 1) {
			spriteMan.SpriteNum = 1;
			spriteMan.setSprite();
		} else if (Time.realtimeSinceStartup - mImpactTime < mFlashTime * 2f && spriteMan.SpriteNum > 0) {
			spriteMan.SpriteNum = 0;
			spriteMan.setSprite();
		}
	}
}