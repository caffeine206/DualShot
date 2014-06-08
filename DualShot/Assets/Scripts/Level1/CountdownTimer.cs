using UnityEngine;
using System.Collections;

public class CountdownTimer : MonoBehaviour {

	RespawnBehavior respawn;
	private const float interval = 1f;
	private float lastCount = 0f;
	private int maxCountdown = 3;
	private bool isCounting = true;

	AudioClip wave;

	void Start () {
		transform.GetComponent<TextMesh>().text = " ";
		transform.GetComponent<TextMesh>().text = maxCountdown.ToString();
		lastCount = Time.realtimeSinceStartup;
		wave = (AudioClip)Resources.Load("Sounds/DaikoSingle");
		Play(wave, 1f, 1);
	}

	void Update () {
		Countdown();
	}

	private void Countdown() {
		if (Time.realtimeSinceStartup >= lastCount + interval && isCounting) {
			lastCount = Time.realtimeSinceStartup;
			maxCountdown--;
			
			if (maxCountdown == 2) {
				wave = Resources.Load("Sounds/DaikoSingleLoud") as AudioClip;
				Play(wave, 1f, 1);
				transform.GetComponent<TextMesh>().text = maxCountdown.ToString();
			} else if (maxCountdown == 1) {
				wave = Resources.Load("Sounds/DaikoSingle") as AudioClip;
				Play(wave, 1f, 1);
				transform.GetComponent<TextMesh>().text = maxCountdown.ToString();
			} else if (maxCountdown == 0) {
				wave = Resources.Load("Sounds/DaikoDrumRoll") as AudioClip;
				Play(wave, 1f, 1);
				transform.GetComponent<TextMesh>().text = "Go!";
				Time.timeScale = 1f;
				StartCoroutine("BackgroundMusic");

				foreach (ParticleSystem flames in FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[]) {
					if (flames.name == "ShipFlames") {
						flames.particleSystem.enableEmission = true;
					}
				}
			}
		}
	}

	IEnumerator BackgroundMusic() {
		yield return new WaitForSeconds(1f);
		wave = Resources.Load("Sounds/DualShotGameplay") as AudioClip;
		Play(wave, 1f, 1);
		isCounting = false;
		gameObject.SetActive(false);
	}

	public bool GetIsCounting() {
		return isCounting;
	}

	// Audio clip player
	public void Play(AudioClip clip, float volume, float pitch) {
		//Create an empty game object
		GameObject go = new GameObject("Audio: " + clip.name);

		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play();
		Destroy(go, clip.length);
	}
}
