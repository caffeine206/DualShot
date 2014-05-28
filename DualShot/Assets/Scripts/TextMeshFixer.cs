using UnityEngine;
using System.Collections;

public class TextMeshFixer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<TextMesh>();
		string temp = transform.GetComponent<TextMesh>().text;
		transform.GetComponent<TextMesh>().text = "";
		transform.GetComponent<TextMesh>().text = temp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
