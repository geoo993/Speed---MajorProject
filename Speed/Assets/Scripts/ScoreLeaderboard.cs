using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ScoreLeaderboard : MonoBehaviour, IPointerClickHandler {


	private Dictionary < string, Dictionary<string, int> > playerScores = new Dictionary<string, Dictionary<string, int>>();

	private int changeManager = 0;
	public InputField newPlayerInput;
	public Button newPlayerButton;

	public Text currentPlayer = null;
	[HideInInspector] public GameObject currentPlayerIcon = null;

	private GameObject currentObject = null;

	//private int scoeee = 0;

	void Awake () {

		newPlayerButton.GetComponent<Button>().onClick.AddListener( () => {OnClickEvent();} ); 

	}

	void OnClickEvent()
	{

		if (newPlayerInput.text.Length > 1) {

			if (playerScores.Keys.Count > 0) {

				SetScore (newPlayerInput.text, "score", GetScore (newPlayerInput.text, "score"));

			} else {
				SetScore (newPlayerInput.text, "score", 0);
			}

		}

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		
		currentObject = eventData.pointerPressRaycast.gameObject;

		if (currentObject.transform.childCount > 0 && currentObject.name == "PlayerScoreEntry") 
		{

			//print("childCount: "+currentObject.transform.childCount+"   name: "+currentObject.name+"   pos: "+currentObject.transform.position);

			Destroy (currentPlayerIcon);
			currentPlayerIcon = (GameObject)Instantiate (Resources.Load ("SelectedPlayerIcon")); 
			currentPlayerIcon.transform.SetParent (currentObject.transform);

			currentPlayerIcon.transform.localPosition = new Vector3(
				currentObject.GetComponent<RectTransform>().rect.width/2, 0.0f, 0.0f);
				
			foreach (Transform child in currentObject.GetComponentsInChildren<Transform>()) {
				//print (child.name);

				if (child.name == "Name"){

					currentPlayer.text = child.GetComponent<Text> ().text;
					//print("name in text: "+child.GetComponent<Text> ().text);
				}
			}
		}

		if (currentObject.name == "InterfaceColorText") {

			Color rand = ExtensionMethods.RandomColor ();
			currentObject.GetComponentInParent<Image>().color = rand;
			GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor = rand;
		}
		//print (eventData.pointerPressRaycast.gameObject.name +"  children: "+currentObject.transform.childCount);

	}

	void Start () {

		//		SetScore ("George", "score", 5400);
		//		SetScore ("George", "minutes", 1);
		//		SetScore ("George", "seconds", 40);
		//
		//		SetScore ("Abudl", "score", 105400);
		//		SetScore ("Abudl", "minutes", 3);
		//		SetScore ("Abudl", "seconds", 20);
		//
		//		SetScore ("Jenny", "score", 24004);
		//		SetScore ("Jenny", "minutes", 2);			
		//		SetScore ("Jenny", "seconds", 56);
		//
	}

	void Update () {

		//print (playerScores.Keys.Count);

//		if (currentPlayerIcon == null) {
//			print ("select object null");
//		} else {
//			print ("select object is active");
//		}


		if (Input.GetKeyDown ("space")) {

//			scoeee = Random.Range (0, 10000);
//			SetScore ("Bob", "score", scoeee);
//			SetScore ("Bob", "minutes", 5);
//			SetScore ("Bob", "seconds", 13);
//
//
//			print (scoeee);

//			foreach (string key in playerScores.Keys)
//			{
//				print (key);
//			}
//

		}


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
