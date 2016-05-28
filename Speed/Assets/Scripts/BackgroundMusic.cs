using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
	
	private static BackgroundMusic instance = null;

	public static BackgroundMusic Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			AudioSource music = GetComponent<AudioSource> ();

			if(music.isPlaying) {
				music.Pause();
			} else {
				music.Play();
			}
		}
	}
}