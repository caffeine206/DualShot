using UnityEngine;
using System.Collections;

public class PowerUpText : MonoBehaviour {

	private float alpha = 1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		alpha -= 0.6f * Time.smoothDeltaTime;
		transform.position += new Vector3(0f, 25f, 0f) * Time.smoothDeltaTime;
		renderer.material.color = new Color(255f, 255f, 255f, alpha);
		Destroy(gameObject, 5);
	}
}
