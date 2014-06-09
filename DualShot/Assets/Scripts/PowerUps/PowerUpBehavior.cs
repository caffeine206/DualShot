using UnityEngine;
using System.Collections;

public class PowerUpBehavior : MonoBehaviour {

	private GameObject powerUpText = null;

	void Start () {
		if (powerUpText == null) {
			powerUpText = Resources.Load("Prefabs/PowerUpText") as GameObject;
		}
	}
	
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "OrangeShip" || other.gameObject.name == "OrangeRedShip" ||
				other.gameObject.name == "BlueShip" || other.gameObject.name == "ChartreuseShip" ||
				other.gameObject.name == "PeriwinkleShip") {
			GameObject text = Instantiate(powerUpText) as GameObject;
			text.transform.position = other.transform.position;
			
			if (gameObject.name == "PowerUp" || gameObject.name == "PowerUp(Clone)") {
				text.transform.GetComponent<TextMesh>().text = "POWER";
			} else if (gameObject.name == "SpeedUp" || gameObject.name == "SpeedUp(Clone)") {
				text.transform.GetComponent<TextMesh>().text = "SPEED";
			} else if (gameObject.name == "GrowUp" || gameObject.name == "GrowUp(Clone)") {
				text.transform.GetComponent<TextMesh>().text = "GROW";
			} else if (gameObject.name == "SpikeUp" || gameObject.name == "SpikeUp(Clone)") {
				text.transform.GetComponent<TextMesh>().text = "SPIKE";
			}

			Destroy(this.gameObject);
		}
	}
}
