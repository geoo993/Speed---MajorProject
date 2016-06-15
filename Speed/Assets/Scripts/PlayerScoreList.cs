using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScoreList : MonoBehaviour {


	public GameObject playerScoreEntry = null;

	private ScoreLeaderboard scoreLeaderboard;

	private int lastChangeCounter = 0;

	void Start () {

		scoreLeaderboard = GameObject.FindObjectOfType<ScoreLeaderboard> ();
		lastChangeCounter = scoreLeaderboard.GetChangeCounter ();
	}

	void Update () {

		if(scoreLeaderboard == null){
			Debug.LogError (" no score leaderboard");
			return;
		}

		if (scoreLeaderboard.GetChangeCounter () == lastChangeCounter) {

			//no change is needed
			return;
		}

		lastChangeCounter = scoreLeaderboard.GetChangeCounter ();

		while (this.transform.childCount > 0) {
			Transform child = this.transform.GetChild (0);

			child.SetParent (null);
			Destroy (child.gameObject);
		}

		string[] names = scoreLeaderboard.GetPlayersNames ("score");

		foreach(string name in names) {

			GameObject addEntry =  (GameObject)Instantiate (playerScoreEntry);
			addEntry.name = "PlayerScoreEntry";
			addEntry.transform.SetParent (this.transform);

			addEntry.transform.Find("Name").GetComponent<Text>().text = name;
			addEntry.transform.Find("Score").GetComponent<Text>().text = scoreLeaderboard.GetScore(name, "score").ToString();
			addEntry.transform.Find("Time").GetComponent<Text>().text = (scoreLeaderboard.GetScore(name, "minutes")+" : "+ scoreLeaderboard.GetScore(name, "seconds")).ToString();



		}

	}
}
