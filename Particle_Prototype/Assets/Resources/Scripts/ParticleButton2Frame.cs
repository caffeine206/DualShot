using UnityEngine;
using System.Collections;

public class ParticleButton2Frame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(Move());
	}

	IEnumerator Move() {
		while (true) {
			yield return StartCoroutine(MoveObject(transform, new Vector3(15f, 35f, 0f), new Vector3(15f, 25f, 0f), 3.0f));
			yield return StartCoroutine(MoveObject(transform, new Vector3(15f, 25f, 0f), new Vector3(-15f, 25f, 0f), 6.0f));
			yield return StartCoroutine(MoveObject(transform, new Vector3(-15f, 25f, 0f), new Vector3(-15f, 35f, 0f), 3.0f));
			yield return StartCoroutine(MoveObject(transform, new Vector3(-15f, 35f, 0f), new Vector3(15f, 35f, 0f), 6.0f));
		}
	}

	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
		float i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null;
		}
	}
}
