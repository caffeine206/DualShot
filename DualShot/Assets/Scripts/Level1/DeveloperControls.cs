using UnityEngine;
using System.Collections;

public class DeveloperControls : MonoBehaviour {

	private Ship periwinkle;
	private Ship orangeRed;
	private BaseBehavior blueCity;
	private BaseBehavior orangeCity;
	private GUIText status;
	private GUIText controls;
	private bool shipsInvulnerable = true;
	private bool citiesInvulnerable = false;

	void Start () {
		periwinkle = GameObject.Find("PeriwinkleShip").GetComponent<Ship>();
		orangeRed = GameObject.Find("OrangeRedShip").GetComponent<Ship>();
		blueCity = GameObject.Find("BlueCity").GetComponent<BaseBehavior>();
		orangeCity = GameObject.Find("OrangeCity").GetComponent<BaseBehavior>();
		status = GameObject.Find("Status").GetComponent<GUIText>();
		controls = GameObject.Find("Controls").GetComponent<GUIText>();

		controls.text = 
			"1: Destroy Periwinkle ship"
			+ "\n2: Destroy OrangeRed ship"
			+ "\n3: Destroy Periwinkle city"
			+ "\n4: Destroy OrangeRed city"
			+ "\n5: Turn on ship invincibility"
			+ "\n6: Turn on city invincibility"
			+ "\nF1: Destroy all orbs"
			+ "\nF2: Spawn more orbs"
			+ "\nEnter: Reset scene"
			+ "\nEscape: Return to main menu";
	}
	
	void Update () {
		KillShip();
		KillCity();
		ShipInvincibility();
		CityInvulnerable();
		UpdateStatus();
	}

	private void UpdateStatus() {
		status.text = 
			"Periwinkle health: " + periwinkle.GetCurrentHealth()
			+ "\nOrangeRed health: " + orangeRed.GetCurrentHealth()
			+ "\nPeriwinkle city health: " + blueCity.currentHealth / 20f
			+ "\nOrangeRed city health: " + orangeCity.currentHealth
			+ "\nShip Invincibility: " + shipsInvulnerable
			+ "\nCity Invincibility: " + citiesInvulnerable;
	}

	#region Destroy the ships
	//For testing purposes.
	private void KillShip() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			periwinkle.Suicide();
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			orangeRed.Suicide();
		}
	}
	#endregion

	#region Destroy the cities
	private void KillCity() {
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			blueCity.Suicide();
		} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
			orangeCity.Suicide();
		}
	}
	#endregion

	#region Turn on ship invincibility
	private void ShipInvincibility() {
		if (Input.GetKeyDown(KeyCode.Alpha5) && !shipsInvulnerable) {
			periwinkle.isInvulnerable = true;
			orangeRed.isInvulnerable = true;
			shipsInvulnerable = true;
		} else if (Input.GetKeyDown(KeyCode.Alpha5) && shipsInvulnerable) {
			periwinkle.isInvulnerable = false;
			orangeRed.isInvulnerable = false;
			shipsInvulnerable = false;
		}
	}
	#endregion

	#region Turn on base invincibility
	private void CityInvulnerable() {
		if (Input.GetKeyDown(KeyCode.Alpha6) && !citiesInvulnerable) {
			blueCity.isInvulnerable = true;
			orangeCity.isInvulnerable = true;
			citiesInvulnerable = true;
		} else if(Input.GetKeyDown(KeyCode.Alpha6) && citiesInvulnerable) {
			blueCity.isInvulnerable = false;
			orangeCity.isInvulnerable = false;
			citiesInvulnerable = false;
		}
	}
	#endregion
}
