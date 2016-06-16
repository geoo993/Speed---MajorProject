using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	private CharacterMeshComplete craftScript;

	public Image healthDamageImages;
	public Image healthFlashImage;
	public Slider[] sliderBars;
	public Image[] fillBarsImages;

	public Image[] gameIcons = null;
	public Image[] craftIcons = null;
	public Image[] craftSliderIcons = null;

	public Text[] radarIconsTexts = null;
	public Text[] mainTexts = null;
	public Image[] scoreIcons = null;
	public GameObject[] radarImages = null;
	public Color interfaceColor = Color.cyan;

	public static int scoreNum = 0;
	public static float currentScoreCount = 0;
	public static float scoreCountDuration = 0;
	public static float timerCount = 0;

	public static float health = 100;
	public static int speedValue = 100;
	public static int flashCount = 120;

	public static float inTransitionNum = 100;
	private float minutesTime = 0;
	private float secondsTime = 0;
	private float lerpInfotext = 0;

	public static int collectedItems = 0; 
	public static int healthCollectableItems = 0; 
	public static int coinCollectableItems = 0; 
	public static int coinItemsAtStart = 0; 

	public static bool disableSun = false;
	private string iconState = "idle";
	public bool showRadar = false;
	public bool showHealth = false;
	public bool showSpeed = false;

	public bool switchAnalogStick = false;

	public enum ControlsType { Keyboard, PS4_Controller, Xbox_Controller };
	public ControlsType controlsType = ControlsType.Keyboard;

	private bool startGame = false;
	public static bool resetGame = false;
	public static string gameOver = "idle";
	public static string currentPlayer;

	public GameObject ScoreBoardPanel = null;

	void Start () {

		Instantiate( Resources.Load ("CircularGround") );
		Instantiate( Resources.Load ("CityObject") );

		StartCoroutine (StartGame ());

	}
	void Update () {


		if (startGame) {
			Game ();
		} 
		if(!startGame)
		{
			
			UpdateTexts ();
			UpdateIcons ();

			collectedItems = 0; 
			healthCollectableItems = 0; 
			coinCollectableItems = 0; 
			coinItemsAtStart = 0;

			disableSun = false;
			minutesTime = 0;
			secondsTime = 0;
			scoreNum = 0;
			currentScoreCount = 0;
			timerCount = 0;
			health = 100; 
			gameOver = "idle";

			ScoreBoardPanel.SetActive(true);

			if (GameObject.Find("Canvas").GetComponent<ScoreLeaderboard>().currentPlayerIcon != null && (Input.GetButton ("PS4_Options") || Input.GetButton ("360_Start") || Input.GetKeyDown ("space"))) {

				currentPlayer = GameObject.Find ("Canvas/ScoreBoardPanel/SelectedPlayer").GetComponent<Text> ().text;

				ScoreBoardPanel.SetActive(false);
				GameObject.Find("Craft").GetComponent<CharacterMovement>().enabled = true;

				startGame = true;

				//print ("select object is active");
			}

		}

		if (Input.GetKeyDown ("3") || Input.GetButton ("PS4_Square") || Input.GetButton ("360_X")) {

			Color rand = ExtensionMethods.RandomColor ();
			interfaceColor = rand;
		}
		if (Input.GetKeyDown ("4") || Input.GetButton ("PS4_Triangle") || Input.GetButton ("360_Y")) {

			Color skyRand = ExtensionMethods.RandomColor ();

			if (Camera.main.gameObject != null) {
				Camera.main.gameObject.GetComponent<Skybox> ().topColor = skyRand;
				Camera.main.gameObject.GetComponent<Skybox> ().midColor = skyRand;
			}

		}
		if (Input.GetKeyDown ("5") || Input.GetButton ("PS4_O") || Input.GetButton ("360_B")) {

			Color buildingsRand = ExtensionMethods.RandomColor ();
			GameObject.Find ("City").GetComponent<GenerateCity> ().buildingsTopColor = buildingsRand;
		}
		if (Input.GetKeyDown ("6") || Input.GetButton ("PS4_X") || Input.GetButton ("360_A")) {

			Color bottomRand = ExtensionMethods.RandomColor ();
			Camera.main.gameObject.GetComponent<Skybox> ().bottomColor = bottomRand;

		}


		//print (startGame);
	}


	private IEnumerator StartGame () 
	{
		WaitForSeconds wait = new WaitForSeconds (0.01f);

		GameObject.Find("City").GetComponent<Items> ().enabled = true;

		Items.fourPoints1.Clear ();
		Items.fourPoints2.Clear ();
		Items.fourPoints3.Clear ();
		Items.fourPoints4.Clear ();

		foreach (GameObject health in Items.healthItems) {
			Destroy (health);
		}
		Items.healthItems.Clear ();

		foreach (GameObject col in Items.collectablesItems) {
			Destroy (col);
		}
		Items.collectablesItems.Clear ();

		foreach (GameObject coin in Items.coinItems) {
			Destroy (coin);
		}
		Items.coinItems.Clear ();

		Items.easyPipeCollectablesItemsPositions.Clear ();
		Items.hardPipeCollectablesItemsPositions.Clear ();

		yield return wait;

		Instantiate( Resources.Load ("MainCamera") );
		Instantiate( Resources.Load ("CraftObject") );
		Instantiate( Resources.Load ("SunObject") );
		Instantiate( Resources.Load ("TorusKnotObject") );
		Instantiate( Resources.Load ("HeartObject") );
		Instantiate( Resources.Load ("EarthBallObject") );
		Instantiate( Resources.Load ("Bubbles") );
		Instantiate( Resources.Load ("EasyPipe") );
		Instantiate( Resources.Load ("HardPipe") );
		Instantiate( Resources.Load ("ArrowLocator") );
	
		Camera.main.gameObject.GetComponent<CameraTracker> ().enabled = true;

		yield return wait;

		craftScript = GameObject.Find("Craft").GetComponent<CharacterMeshComplete>();

		coinItemsAtStart = Items.coinItems.Count;

		GameObject.Find("Craft").GetComponent<CharacterMovement>().enabled = false;

		yield return wait;

		StartCoroutine (GameObject.Find ("City").GetComponent<Items> ().CreateCollectable ());
		GameObject.Find ("City").GetComponent<GenerateCity> ().ResetCity ();

		yield return wait;

		startGame = false;
	}

	void Game(){

		UpdateHealthSlider ();
		UpdateInTransitionSlider ();
		UpdateSpeedSlider ();

		UpdateFillBars ();
		UpdateTexts ();
		UpdateIcons ();
		FlashHealth ();
		LerpScore ();

		if(Items.collectablesItems.Count <= 0){
			GameOver ();
		}


	}


	public void GameOver(){

		if (gameOver == "idle") {

			GameObject.Find ("Canvas").GetComponent<ScoreLeaderboard> ().SetScore (currentPlayer, "score", (int)currentScoreCount);
			GameObject.Find ("Canvas").GetComponent<ScoreLeaderboard> ().SetScore (currentPlayer, "minutes", (int)minutesTime);
			GameObject.Find ("Canvas").GetComponent<ScoreLeaderboard> ().SetScore (currentPlayer, "seconds", (int)secondsTime);

			Instantiate( Resources.Load ("Menu") );
			GameObject.Find ("Canvas/MenuBoard").GetComponent<Animator> ().SetTrigger ("GameOver");
			GameObject.Find ("Canvas/MenuBoard").GetComponent<RectTransform> ().localPosition = Vector3.zero;
			gameOver = "stop game";
		}

		GameObject.Find ("Craft").GetComponent<CharacterMeshComplete> ().enabled = false;
		GameObject.Find ("Craft").GetComponent<CharacterMovement> ().enabled = false;

		if (Input.GetKeyDown ("space") || Input.GetButton ("PS4_Options")) 
		{
			Restart ();
		}

	}
	void Restart(){

		resetGame = true;

		if (resetGame){

			Camera.main.gameObject.GetComponent<CameraTracker> ().enabled = false;
			GameObject.Find("City").GetComponent<Items> ().enabled = false;
			startGame = false;

			Destroy (GameObject.Find ("Craft"));
			Destroy (GameObject.Find ("Sun"));
			Destroy (GameObject.Find ("TorusArc"));
			Destroy (GameObject.Find ("Heart"));
			Destroy (GameObject.Find ("Earth"));
			Destroy (GameObject.Find ("FloatingBubbles"));
			Destroy (GameObject.Find ("SwirlPipeSystemEasy"));
			Destroy (GameObject.Find ("SwirlPipeSystemHard"));
			Destroy (GameObject.Find ("Locator"));
			Destroy (GameObject.Find ("Curve1"));
			Destroy (GameObject.Find ("Curve2"));
			Destroy (GameObject.Find ("Curve3"));
			Destroy (GameObject.Find ("Curve4"));
			Destroy (GameObject.Find ("MenuBoard"));
			Destroy (GameObject.Find ("Main Camera"));

			StartCoroutine (StartGame ());

			resetGame = false;
		}

	}

		
	void UpdateRadar(){

		foreach (GameObject im in radarImages) {
			if (showRadar) {
				im.SetActive(true);

			}else{
				im.SetActive(false);
			}
		}

	}

	void UpdateInTransitionSlider(){

		sliderBars[0].value = inTransitionNum;
		if (inTransitionNum > 0 && inTransitionNum < 100) {
			mainTexts[0].enabled = true;
		} else {

			mainTexts[0].enabled = false;
		}
	}

	void UpdateHealthSlider(){

		sliderBars[1].value = (int)health;

		if (health <= 0) {

			GameOver ();
			health = 0;
		} 
		if (health >= 100f) {

			health = 100f;
		}

		if (health >= 40f) {

			healthDamageImages.color = new Color (healthDamageImages.color.r, healthDamageImages.color.g, healthDamageImages.color.b, 0.0f);

		}else if (health < 40f && health > 25f) {
			healthDamageImages.color = new Color (healthDamageImages.color.r, healthDamageImages.color.g, healthDamageImages.color.b, 0.3f);
		} else if (health < 25f && health > 10f) {
			healthDamageImages.color = new Color (healthDamageImages.color.r, healthDamageImages.color.g, healthDamageImages.color.b, 0.5f);
		} else if (health < 10f) {
			healthDamageImages.color = new Color (healthDamageImages.color.r, healthDamageImages.color.g, healthDamageImages.color.b, flashing (1.0f));
		} 

		if (showHealth) {
			sliderBars [1].gameObject.SetActive(true);
			mainTexts [1].gameObject.SetActive(true);
		} else {
			sliderBars [1].gameObject.SetActive(false);
			mainTexts [1].gameObject.SetActive(false);
		}

	}

	void UpdateSpeedSlider()
	{
		speedValue = (int)percentageValue (CharacterMovement.speed, 0.0f, 2000f);
		sliderBars[2].value = speedValue;

		if (showSpeed) {
			sliderBars [2].gameObject.SetActive(true);
			mainTexts [2].gameObject.SetActive(true);
		} else {
			sliderBars [2].gameObject.SetActive(false);
			mainTexts [2].gameObject.SetActive(false);
		}
	}
		
	void UpdateFillBars(){

		//in transition
		fillBarsImages[0].color = interfaceColor;

		//health
		fillBarsImages[1].color = Color.Lerp (Color.red, Color.green, health / 100f);
		if (health < 20) 
		{
			fillBarsImages[1].color  = new Color(fillBarsImages[1].color.r, fillBarsImages[1].color.g, fillBarsImages[1].color.b, flashing(1.0f));
		}
		fillBarsImages[2].color =  Color.Lerp (Color.red, Color.green, speedValue / 100f);

	}

	void UpdateTexts()
	{

		mainTexts[0].text = "In Transition.";
		mainTexts[0].color = new Color(interfaceColor.r,interfaceColor.g,interfaceColor.b, flashing(1.0f));

		mainTexts[1].text = "Health ( "+(int)health+" )";
		mainTexts[2].text = "Speed ( "+Mathf.Round(CharacterMovement.speed)+" )";
		mainTexts[3].text = " "+(int)currentScoreCount;
		mainTexts[4].text = Timer();
		mainTexts[5].text = "COLLECT  THE  DIAMONDS";
		mainTexts[6].color =	new Color(interfaceColor.r,interfaceColor.g,interfaceColor.b, flashing(1.5f));

		if (controlsType == ControlsType.PS4_Controller) {

			mainTexts[6].text =	"Press OPTIONS To Start";
		}else if (controlsType == ControlsType.Xbox_Controller) {

			mainTexts[6].text =	"Press START To Start";
		}else if (controlsType == ControlsType.Keyboard) {

			mainTexts[6].text =	"Press SPACE To Start";
		}

		if (secondsTime >= 0 && secondsTime <= 10.0f) 
		{
			mainTexts [5].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, flashing (1.5f));
			lerpInfotext = 0f;

		} else {
			
			if (lerpInfotext < 1.0f) {
				lerpInfotext += Time.deltaTime * (1 / 5.0f); //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
			}
			mainTexts [5].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, Mathf.Lerp(mainTexts [5].color.a, 0.0f,  lerpInfotext));
		}

		for (int i = 1; i < mainTexts.Length - 2; i++) 
		{
			mainTexts [i].color = interfaceColor;
		}

		radarIconsTexts [0].text = ""+CharacterMeshComplete.tranformNum  + " /" + GameObject.Find("City").GetComponent<Items>().transformPickUps;
		radarIconsTexts [1].text = ""+healthCollectableItems + " /" + GameObject.Find("City").GetComponent<Items>().healthPickUps;
		//radarIconsTexts [2].text = ""+resetCollectableItems + " /" + city.GetComponent<Items>().resetPickUps;
		radarIconsTexts [2].text = ""+coinCollectableItems + " /" + coinItemsAtStart;
		radarIconsTexts [3].text = ""+collectedItems + " /" +Items.collectablePickUps;

		foreach (Text textIcon in radarIconsTexts) {
			textIcon.color = interfaceColor;
		}
	}

	void UpdateIcons(){

		foreach (Image icons in scoreIcons) {

			icons.color = interfaceColor;
		}

		if ((craftScript.moveState == "ball" && craftScript.animateCount == 0) || (craftScript.moveState == "car" && craftScript.animateCount == 2)) {

			iconState = "level1";

		}else if ((craftScript.moveState == "car" && craftScript.animateCount == 1) || (craftScript.moveState == "airplane" && craftScript.animateCount == 3)) {

			iconState = "level2";

		} else if((craftScript.moveState == "airplane" && craftScript.animateCount == 2) || (craftScript.moveState == "jet" && craftScript.animateCount == 4))
		{
			iconState = "level3";

		} else if ((craftScript.moveState == "jet" && craftScript.animateCount == 3) || (craftScript.moveState == "nasa" && craftScript.animateCount == 5)){

			iconState = "level4";

		}else if (craftScript.moveState == "nasa" && craftScript.animateCount == 4)
		{
			iconState = "level5";
		} 

		UpdateIconsState ();

	}

	private void UpdateIconsState(){

		if (iconState == "level1") {
			
			ShowHideCraftIcon (0);
			ShowHideCraftSliderIcon (0);
			
		} else if (iconState == "level2") {
			
			ShowHideCraftIcon (1);
			ShowHideCraftSliderIcon (1);

		} else if (iconState == "level3") {
			
			ShowHideCraftIcon (2);
			ShowHideCraftSliderIcon (2);

		} else if (iconState == "level4") {
			
			ShowHideCraftIcon (3);
			ShowHideCraftSliderIcon (3);

		} else if (iconState == "level5") {

			ShowHideCraftIcon (4);
			ShowHideCraftSliderIcon (4);
		}
	}

	private void LerpScore()
	{
		
		if (currentScoreCount < (float)scoreNum) {
			
			currentScoreCount += scoreCountDuration * Time.deltaTime;
		}
		//print ((int)currentScoreCount);

	}
	private void FlashHealth(){

		if (flashCount < 100) {

			healthFlashImage.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, flashing (0.5f));

			flashCount += 1;
		} else {
			healthFlashImage.color = Color.clear;
		}

	}

	private string Timer (){

		if(gameOver == "idle")
		{
			timerCount += Time.deltaTime;
		}

		minutesTime = Mathf.Floor (timerCount / 60);
		secondsTime = timerCount % 60;
		string minutes = (minutesTime).ToString("00");
		string seconds = (secondsTime).ToString("00");

		return minutes +" : "+seconds;
	}
	private void ShowHideCraftIcon(int i){

		foreach (Image icon in craftIcons) {
			icon.color = interfaceColor;
			icon.enabled = false;
		}

		craftIcons [i].enabled = true;
	}

	private void ShowHideCraftSliderIcon(int i){

		foreach (Image icon in craftSliderIcons) {
			icon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
		}
		craftSliderIcons [i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 1f);
	}

	public float percentageValue( float value, float min, float max) 
	{
		float difference = max - min;
		float myPercent = ((value - min) / difference);
		return Mathf.Round(100.0f * myPercent);
	}

	public float flashing( float duration)
	{
		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;
			
		return amplitude;
	}


}
