using UnityEngine;
using System.Collections;

public class ShieldSprite : MonoBehaviour {

	private SpriteManager spriteMan = null;
	// spikey powerup animation
	private bool spikey = false, growingSpikes = false, shrinkingSpikes = false;
	private float mSpikeFrameTime = .02f;
	private float mSpikeTime = -20f;
	private float mPowerupTime = -20f;
	private float mPowerupLength = 0f;
	
	// flashing shield animation
	private float mFlashTime = 0.2f;
	public float mImpactTime = 0f;

	// Use this for initialization
	void Start () {
	
		if (spriteMan == null) {
			spriteMan = GetComponent<SpriteManager>();
		}
		mPowerupLength = Ship.kSpikeEnd;
		
	}
	
	// Update is called once per frame
	void Update () {
		// ignores flashing if spikes are up
		if ( spikey ) {
			if (growingSpikes) {
				// grows spikes
				if (Time.realtimeSinceStartup - mSpikeTime > mSpikeFrameTime) {
					spriteMan.nextSprite();
					mSpikeTime = Time.realtimeSinceStartup;
					if (spriteMan.SpriteNum == 6)
						// when done growing, turn off growing animation
						growingSpikes = false;
				}
			} else if(shrinkingSpikes) {
				// shrinks spikes
				if (Time.realtimeSinceStartup - mSpikeTime > mSpikeFrameTime) {
				spriteMan.prevSprite();
				mSpikeTime = Time.realtimeSinceStartup;
					if (spriteMan.SpriteNum == 1) {
						// when done shrinking, turn of spikes
						shrinkingSpikes = false;
						spikey = false;
					}
				}	
			} else {
				// rotate the shield
				transform.Rotate(0f, 0f, 180f * Time.smoothDeltaTime);
				// start shrinking when you have just enough time left to shrink before the powerup
				//  wears off
				if (Time.realtimeSinceStartup - mPowerupTime > mPowerupLength - 0.01f) {
					shrinkingSpikes = true;
				}
			}
		} else {
			// Sprite flash animation
			if (Time.realtimeSinceStartup - mImpactTime > mFlashTime && spriteMan.SpriteNum != 1) {
				spriteMan.nextSprite();
			}
		}
		
	}
	// Starts a shield flash
	public void shieldFlash() {
		if (!spikey) {
			mImpactTime = Time.realtimeSinceStartup;
			spriteMan.setSprite(0);
		}
	}
	
	// cancels shield flashes and starts the shield spikes
	public void spikesUp() {
		mSpikeTime = Time.realtimeSinceStartup;
		mPowerupTime = Time.realtimeSinceStartup;
		spriteMan.setSprite(1);
		spikey = true;
		growingSpikes = true;
        shrinkingSpikes = false;
	}
}
