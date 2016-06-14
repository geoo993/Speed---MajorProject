using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ScoreLeaderboard : MonoBehaviour {


	private Dictionary < string, Dictionary<string, int> > playerScores;

	public static int bestScore = 0;

	private string playerName = "";
	private int playerScore = 0;

	private int changeManager = 0;

	public string newPlayer = "";

	void Start () {

		SetScore ("George", "score", 5400);
		SetScore ("George", "minutes", 1);
		SetScore ("George", "seconds", 40);

		SetScore ("Abudl", "score", 105400);
		SetScore ("Abudl", "minutes", 3);
		SetScore ("Abudl", "seconds", 20);

		SetScore ("Jenny", "score", 24004);
		SetScore ("Jenny", "minutes", 2);			
		SetScore ("Jenny", "seconds", 56);

	}

	void Update () {
	
//		if (Input.GetKeyDown ("space")) {
//
//			playerScore = Random.Range (0, 10000);
//
//			SetScore ("Jenny", "score", playerScore);
//			SetScore ("Jenny", "minutes", 2);
//			SetScore ("Jenny", "seconds", 56);
//
//			print ("Best Score: " + bestScore + "   score: " + playerScore);
//		}

//		if (Input.GetKeyDown ("p")) {
//
//			SetScore (newPlayer, "score", 0);
//		}

	}
	void ScoreInit(){

		if (playerScores != null)
			return;
			playerScores = new Dictionary< string, Dictionary<string, int> > ();

	}


	public int GetScore(string name, string scoreType)
	{
		ScoreInit ();
		if (playerScores.ContainsKey (name) == false) {
			//we have no name record of this 
			return 0;
		}
		if (playerScores[name].ContainsKey (scoreType) == false) {
			//we have no score record of this name
			return 0;
		}

		return playerScores [name] [scoreType];
	}

	public void SetScore(string name, string scoreType, int value){

		ScoreInit ();

		changeManager++;

		if (playerScores.ContainsKey (name) == false) {
			playerScores [name] = new Dictionary<string, int> ();
		}

		playerScores [name] [scoreType] = value;
	}

	public void ChangeScore(string name, string scoreType, int amount){

		ScoreInit ();

		int currentScore = GetScore (name, scoreType);
		SetScore (name, scoreType, currentScore + amount);
	}

//	public string[] GetPlayersNames(){
//
//		ScoreInit ();
//
//		return playerScores.Keys.ToArray();
//
//	}

	public string[] GetPlayersNames(string sortingScoreType){

		ScoreInit ();

		return playerScores.Keys.OrderByDescending (n => GetScore (n, sortingScoreType)).ToArray();
	}


	public int  GetChangeCounter(){

		return changeManager;
	}
}
