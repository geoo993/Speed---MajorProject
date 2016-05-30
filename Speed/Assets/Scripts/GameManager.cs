using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject city = null;
	public GameObject craft = null;
	private CharacterMeshComplete craftScript;

	private int healthIconsAmount = 0;
	public static int health = 80;
	public static int speedValue = 100;

	public Slider[] sliderBars;
	public Image[] fillBarsImages;

	public Image[] gameIcons = null;
	public Image[] radarLocatorsIcons = null;
	public Image[] craftIcons = null;
	public Image[] craftSliderIcons = null;

	public Text[] radarIconsTexts = null;
	public Text[] mainTexts = null;
	public Image[] scoreIcons = null;

	public Color interfaceColor = Color.cyan;

	public static float inTransitionNum = 100;
	public static int collecteditems = 0; 

	private string iconState = "idle";

	void Start () {

		craftScript = craft.GetComponent<CharacterMeshComplete>();
	}

	void Update () {
	
		UpdateHealthSlider ();
		UpdateInTransitionSlider ();
		UpdateSpeedSlider ();

		UpdateFillBars ();
		UpdateTexts ();
		UpdateIcons ();

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

		sliderBars[1].value = health;

		if (health <= 0) {
			health = 0;
		}
		if (health >= 100) {

			health = 100;
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
		mainTexts[1].text = "Health ( "+health+" )";
		mainTexts[2].text = "Speed ( "+Mathf.Round(CharacterMovement.speed)+" )";
		mainTexts[3].text = "Points/Score ";

		for (int i = 1; i < mainTexts.Length; i++) {
			mainTexts [i].color = interfaceColor;
		}

		radarIconsTexts [0].text = ""+CharacterMeshComplete.tranformNum  + " /" + city.GetComponent<Items>().transformPickUps;
		radarIconsTexts [1].text = ""+Items.healthItems.Count + " /" + city.GetComponent<Items>().healthPickUps;
		radarIconsTexts [2].text = ""+Items.resetItems.Count + " /" + city.GetComponent<Items>().resetPickUps;
		radarIconsTexts [3].text = ""+collecteditems;

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

	public void ShowHideRadarLocatorIcon(int i){

		foreach (Text textIcon in radarIconsTexts) {
			textIcon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
		}
		foreach (Image icon in gameIcons) 
		{
			icon.transform.localScale = Vector3.one;
			icon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
		}

		foreach (Image icon in radarLocatorsIcons) 
		{
			icon.color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 0.2f);
		}

		gameIcons [i].transform.localScale = Vector3.one * 1.4f;
		gameIcons[i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b,  1.0f);
		radarIconsTexts[i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 1.0f);
		radarLocatorsIcons [i].color = new Color (interfaceColor.r, interfaceColor.g, interfaceColor.b, 1f);
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


	public static void CreateSwirl(){

	}

}
