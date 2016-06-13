using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadGame : MonoBehaviour {

	public string levelToLoad;
	public GameObject loadingBackground = null;
	public GameObject loadingText = null;
	public GameObject progressBar = null;

	private int loadProgress = 0;

	void Start(){

		loadingBackground.SetActive (false);
		loadingText.SetActive (false);
		progressBar.SetActive (false);
	}

	void Update(){

		if (Input.GetKeyDown ("space")) {

			StartCoroutine (DisplayLoadingScreen(levelToLoad));
			//Application.LoadLevel (levelToLoad);
		}

	}
	IEnumerator DisplayLoadingScreen(string level){

		loadingBackground.SetActive (true);
		loadingText.SetActive (true);
		progressBar.SetActive (true);

		progressBar.transform.localScale = new Vector3 (loadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
		loadingText.GetComponent<GUIText>().text = " L o a d   P r o g r e s s " + loadProgress + "%";

		AsyncOperation async = Application.LoadLevelAsync (level);

		while (!async.isDone) {
			
			loadProgress = (int)(async.progress * 100);
			loadingText.GetComponent<GUIText>().text = " L o a d   P r o g r e s s " + loadProgress + "%";
			progressBar.transform.localScale = new Vector3 (async.progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

			print(async.progress);
			//print("scyncing");
			yield return null;

		}
	}

}
