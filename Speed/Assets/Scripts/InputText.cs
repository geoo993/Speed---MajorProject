using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputText : MonoBehaviour {

	public InputField newPlayerInput;
	private ScoreLeaderboard scoreLeaderboard;

	void Awake () {
		
		GetComponent<Button> ().onClick.AddListener( () => {myFunctionForOnClickEvent("stringValue", 4.5f);} ); 

		scoreLeaderboard = GameObject.FindObjectOfType<ScoreLeaderboard> ();
	}


	void myFunctionForOnClickEvent(string argument1, float argument2)
	{
		if(scoreLeaderboard == null){
			Debug.LogError (" no score leaderboard");
			return;
		}

		if (newPlayerInput.text.Length > 1) {

			scoreLeaderboard.SetScore(newPlayerInput.text, "score", 0);
			print(newPlayerInput.text);
		}

	}
}
