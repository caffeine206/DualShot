using UnityEngine;
using System.Collections;
/// <summary>
/// Credit: Simple Sprite Sheet was created by:
/// 	Jebediah Pavleas, August 2011 while he was a student at:
///		Computing and Software Systems, 
///		University of Washington, Bothell
/// 
/// ManyObject Sprite Sheet was from:
/// 	Sam Cook, Spring 2010 while he was a student at:
///		Computing and Software Systems, 
///		University of Washington, Bothell
/// 
/// Use with permission from both.
/// 
/// </summary>

#region a simple class to capture a sprite sequection
//
/// <summary>
/// Input parametesr describe one sprite animation action
/// </summary>
/// 
public class SpriteActionDefinition {
	public int mRow;
	public int mBeginColumn;		// if Begin > End, the sequence will go backwards
	public int mEndColumn;
	public float mUpdatePeriod;
	public bool mShouldLoop;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="SpriteActionDefinition"/> class.
	/// </summary>
	/// <param name='rowIndex'>
	/// The row index to the animation
	/// </param>
	/// <param name='beginColumn'>
	/// Begin column index of the animation
	/// </param>
	/// <param name='endColumn'>
	/// End column index of the animation
	/// </param>
	/// <param name='period'>
	/// Period (in seconds) to show each sprite element
	/// </param>
	/// <param name='shouldLoop'>
	/// loop the animation: when looping, beginColumn must be less than endColumn.
	/// </param>
	public SpriteActionDefinition(int rowIndex, 
	                              int beginColumn, int endColumn, 
	                              float period, bool shouldLoop) 
	{
		mRow = rowIndex;
		mBeginColumn = beginColumn;
		mEndColumn = endColumn;
		mUpdatePeriod = period;
		mShouldLoop = shouldLoop;
	}
	public SpriteActionDefinition(SpriteActionDefinition copy) {
		mRow = copy.mRow;
		mBeginColumn = copy.mBeginColumn;
		mEndColumn = copy.mEndColumn;
		mUpdatePeriod = copy.mUpdatePeriod;
		mShouldLoop = copy.mShouldLoop;
	}
};
#endregion
public class SpriteManager : MonoBehaviour {	
		
		// the user specify sprite sheet
		private Sprite[][] mSpriteElements = null;	// The actual computed sprite elements
		public Sprite[] mSpriteLine = null;
		/// <summary>
		/// When not null, this refers to the current sprite animation we are performing
		/// </summary>
		public SpriteActionDefinition mCurrentSpriteAction = null;	
		
		#region definition of what is one sprite element
		/// <summary>
		/// Input parameters describing the sprite sheet
		/// </summary> 
		public int mRowInSheet = 0;         // number of rows
		public int mColumnInSheet = 0;      // number of columns
		public int mPixelWidth = 0;
		public int mPixelHeight = 0;
		public int mBlankPixelsToLeft = 0;  // Number of blank pixels to the left/right of the sprite element
		public int mBlankPixelsToRight = 0; // 
		public int mBlankPixelsAbove = 0;   // Number of blank pixels above/below the sprite element
		public int mBlankPixelsBelow = 0;   // 
		#endregion 
		
		#region to support current sprite action
		/// <summary>
		/// Parameter for supporting current on-going animation
		/// </summary>
		private int mCurrentActionNum;       // mCurrentActionRow is simply mSpriteRow
		private int mCurrentActionColumnDelta;
		private float mCurrentActionLastUpdateTime;
		private SpriteRenderer mMyRenderer = null;
		#endregion
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SpriteSheetManager"/> class.
		/// This class expects itself to be attached to a GameObject with a SpriteRenderer
		/// defined and expects user to properly specifies the values that define
		/// one sprite element.
		/// </summary>
		void Start()
		{
			
		}
		
		void Awake() 
		{
			if ((mRowInSheet == 0) || (mColumnInSheet == 0))
				return;  // not properly defined, cannot do anything!
			
			mMyRenderer = GetComponent<SpriteRenderer>();
			if (null == mMyRenderer)
				return; // does not have a SpriteRenderer, cannot do anything!
			
			// this is the texture resolution
			Rect totalTexResolution = mMyRenderer.sprite.textureRect;
			
			// this is the padded sprite element sizes
			
			Rect spriteRect = new Rect();
			spriteRect.x = 0;
			spriteRect.y = 0;  // (0, 0) is the lower left corner
			spriteRect.width = mPixelWidth;
			spriteRect.height = mPixelHeight;
			//spriteRect.width = elmWidthWithBlank - mBlankPixelsToLeft - mBlankPixelsToRight;
			//spriteRect.height = elmHeightWithBlank - mBlankPixelsAbove - mBlankPixelsBelow;
			
			// now, lets define the row and column sprite elements
			mSpriteElements = new Sprite[mRowInSheet][];
			mSpriteLine = new Sprite[mRowInSheet * mColumnInSheet];
			for (int r = 0; r < mRowInSheet; r++) {
				mSpriteElements[r] = new Sprite[mColumnInSheet];
				spriteRect.y = (r * mPixelHeight);
				for (int c = 0; c < mColumnInSheet; c++) {
					spriteRect.x = (c * mPixelWidth);
					mSpriteElements[r][c] = Sprite.Create (
						mMyRenderer.sprite.texture,
						spriteRect, new Vector2(0.5f, 0.5f));
					
					mSpriteLine[c * mRowInSheet + r] = Sprite.Create (
						mMyRenderer.sprite.texture,
						spriteRect, new Vector2(0.5f, 0.5f));
				}
		}
		
		// now comptue the position and scale offset
		// compute the proper scale factor
		float sx = totalTexResolution.width / spriteRect.width;
		float sy = totalTexResolution.height / spriteRect.height;
		transform.localScale = new Vector3(sx * transform.localScale.x, 
		                                   sy * transform.localScale.y, 1f);
		
		// finally 
		if (null != mCurrentSpriteAction)
			mMyRenderer.sprite = mSpriteLine[mCurrentActionNum];	
		// create a new collder for this GameObject
		else
			mMyRenderer.sprite = mSpriteElements[0][0];	
		//RecreateCollider();
		}
		
		/// <summary>
		/// Updates the sprite animation. Should be called per update.
		/// </summary>
		void Update() {
			if ((mCurrentSpriteAction == null) || (mCurrentSpriteAction.mBeginColumn == mCurrentSpriteAction.mEndColumn))
				return;  // no action defined or required
			
			/* if ((Time.realtimeSinceStartup - mCurrentActionLastUpdateTime) > mCurrentSpriteAction.mUpdatePeriod) {
				mCurrentActionLastUpdateTime = Time.realtimeSinceStartup;
				ComputeNextAndContinue();					
				mMyRenderer.sprite = mSpriteLine[mCurrentSpriteAction.mRow];	
			}*/
		}
		private void RecreateCollider()
		{
			// create a new collder for this GameObject
			Collider2D collider = GetComponent<Collider2D>();
			if (null != collider) {
				bool shouldTrigger = collider.isTrigger;
				Destroy(collider2D);
				collider = gameObject.AddComponent("BoxCollider2D") as Collider2D;
				collider.isTrigger = shouldTrigger;
			}
		}
		public void nextSprite() 
		{
			if ( mCurrentActionNum < mRowInSheet * mColumnInSheet - 1)
				mCurrentActionNum ++;
			if (mMyRenderer == null) {
				mMyRenderer = GetComponent<SpriteRenderer>();
			}
				gameObject.GetComponent<SpriteRenderer>().sprite = mSpriteLine[mCurrentActionNum];
		}
		public void setSprite() 
		{
			mMyRenderer.sprite = mSpriteLine[mCurrentActionNum];
		}
		public void setSprite( int spriteNum) {
			mCurrentActionNum = spriteNum;
			mMyRenderer.sprite = mSpriteLine[mCurrentActionNum];
		}
		public int SpriteNum { 
			get { return mCurrentActionNum;}
		 	set { mCurrentActionNum = value; }
		 }
		
}
