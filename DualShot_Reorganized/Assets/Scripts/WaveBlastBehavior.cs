using UnityEngine;
using System.Collections;

public class WaveBlastBehavior : MonoBehaviour {
	
	public float mSpeed = 100f;

	void Start()
	{
	}

	// Update is called once per frame
	void Update () {
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        string level = MenuState.TheGameState.getLevel;
		LocalGameBehavior localGameBehavior = GameObject.Find ("GameManager").GetComponent<LocalGameBehavior>();
		LocalGameBehavior.WorldBoundStatus status =
            localGameBehavior.CollideWithWorld(GetComponent<Renderer>().bounds);
		if (status != LocalGameBehavior.WorldBoundStatus.Inside) {
			Destroy(this.gameObject);
		}
	}
	
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}
}
