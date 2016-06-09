using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject city = null;
	public GameObject craft = null;
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

	private int healthIconsAmount = 0;
	public static float health = 80;
	public static int speedValue = 100;
	public static int flashCount = 120;

	public static float inTransitionNum = 100;

	public static int radarIcon = 0; 

	public static int collectedItems = 0; 
	public static int healthCollectableItems = 0; 
	//public static int resetCollectableItems = 0; 
	public static int coinCollectableItems = 0; 
	public static int coinItemsAtStart = 0; 

	public static bool disableSun = false;
	private string iconState = "idle";
	public bool showRadar = false;

	public enum ControlsType { Keyboard, Controller };
	public ControlsType controlsType = ControlsType.Keyboard;


	void Start () {

		craftScript = craft.GetComponent<CharacterMeshComplete>();
		coinItemsAtStart = Items.coinItems.Count;


	}

	void Update () {
	
		UpdateRadar ();
		UpdateHealthSlider ();
		UpdateInTransitionSlider ();
		UpdateSpeedSlider ();

		UpdateFillBars ();
		UpdateTexts ();
		UpdateIcons ();
		PS4Controls ();
		FlashHealth ();
		LerpScore ();

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


	void PS4Controls()
	{

		if (Input.GetKeyDown ("7") || Input.GetButtonDown ("PS4_X")) {

//			if (healthCollectableItems > 0 && radarIcon == 1) {
//				health += 20;
//				healthCollectableItems -= 1;
//			}

//			if (healthCollectableItems > 0 ) {
//				health += 20;
//				healthCollectableItems -= 1;
//			}


//			if (resetCollectableItems > 0 && radarIcon == 2) {
//				
//				resetCollectableItems -= 1;
//			}

		}

		////Axis
//		print("Right Analog Horizontal: "+ Input.GetAxis ("PS4_RightAnalogHorizontal") +
//				"    Right Analog Vertical: "+ Input.GetAxis ("PS4_RightAnalogVertical")+
//				"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//				"    Vertical: "+ Input.GetAxis ("Vertical")
//		);
//
//
//		print("D-Pad Horizontal: "+ Input.GetAxis ("PS4_DirectionalPadHorizontal") +
//			"    D-Pad Vertical: "+ Input.GetAxis ("PS4_DirectionalPadVertical")+
//			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//			"    Vertical: "+ Input.GetAxis ("Vertical")
//		);

//		print("L2: "+ Input.GetAxis ("PS4_L2") +
//			"    R2: "+ Input.GetAxis ("PS4_R2")+
//			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//			"    Vertical: "+ Input.GetAxis ("Vertical")
//		);

//		print("Left Analog Horizontal: "+ Input.GetAxis ("PS4_LeftAnalogHorizontal") +
//			"    Left Analog Vertical: "+ Input.GetAxis ("PS4_LeftAnalogVertical")+
//			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//			"    Vertical: "+ Input.GetAxis ("Vertical")
//		);


		////buttons
//		if (Input.GetButton ("PS4_X")) 
//		{
//			print("Pressed X");
//		}
//		if (Input.GetButton ("PS4_O")) {
//			print("Pressed O");
//		}
//		if (Input.GetButton ("PS4_Triangle")) {
//			print("Pressed Triangle");
//		}
//		if (Input.GetButton ("PS4_Square")) {
//			print("Pressed Square");
//		}
//		if (Input.GetButton ("PS4_Share")) {
//			print("Pressed Share");
//		}
//		if (Input.GetButton ("PS4_Options")) {
//			print("Pressed Options");
//		}
//		if (Input.GetButton ("PS4_R1")) {
//			print("Pressed R1");
//		}
//		if (Input.GetButton ("PS4_R3")) {
//			print("Pressed R3");
//		}
//		if (Input.GetButton ("PS4_L1")) {
//			print("Pressed L1");
//		}
//		if (Input.GetButton ("PS4_L3")) {
//			print("Pressed L3");
//		}
//		if (Input.GetButton ("PS4_Touch")) {
//			print("Pressed Touch");
//		}
//		if (Input.GetButton ("PS4_PSN")) {
//			print("Pressed PSN");
//		}

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
			
	}

	void UpdateSpeedSlider()
	{
		speedValue = (int)percentageValue (CharacterMovement.speed, 0.0f, 2000f);
		sliderBars[2].value = speedValue;

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
		mainTexts[3].text = "Points: "+(int)currentScoreCount;
		mainTexts[4].text = Timer();

		for (int i = 1; i < mainTexts.Length; i++) {
			mainTexts [i].color = interfaceColor;
		}

		radarIconsTexts [0].text = ""+CharacterMeshComplete.tranformNum  + " /" + city.GetComponent<Items>().transformPickUps;
		radarIconsTexts [1].text = ""+healthCollectableItems + " /" + city.GetComponent<Items>().healthPickUps;
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

//	public void ShowHideRadarLocatorIcon(int i){
//
//		foreach (Text textIcon in radarIconsTexts) {
//			textIcon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
//		}
//		foreach (Image icon in gameIcons) 
//		{
//			icon.transform.localScale = Vector3.one;
//			icon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
//		}
//

//		gameIcons [i].transform.localScale = Vector3.one * 1.4f;
//		gameIcons[i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b,  1.0f);
//		radarIconsTexts[i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 1.0f);
//	}

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

		timerCount += Time.deltaTime;
		string minutes = Mathf.Floor(timerCount / 60).ToString("00");
		string seconds = (timerCount % 60).ToString("00");

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
