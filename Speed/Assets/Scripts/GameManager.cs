using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject craft = null;
	public CharacterMeshComplete craftScript;

	public static int health = 80;
	public static int speedValue = 100;

	public Slider healthBar;
	public Slider speedBar;
	public Slider transformerBar;

	public Image healthBarFillImage;
	public Image speedBarFillImage;
	public Image transitionBarFillImage;
	public Image[] icons = null;
	public Image[] craftIcons = null;
	public Image[] craftSliderIcons = null;

	public Text transitionText;
	public Text healthText;
	public Text speedText;

	private Color fullHealthColor = Color.green;
	private Color lowHealthColor = Color.red;
	public Color interfaceColor = Color.cyan;
	public bool showIcons = false;

	public static float transformNum = 100;
	public static int collecteditems = 0; 

	void Start () {

		//InvokeRepeating ("ReduceHealth", 1, 1);
		craftScript = craft.GetComponent<CharacterMeshComplete>();
	}

	void Update () {
	
		HealthSlider ();
		TransitionSlider ();
		SpeedSlider ();

		UpdateSlider ();
		UpdateTexts ();
		UpdateIcons ();


	}

	void HealthSlider(){

		healthBar.value = health;

		if (health <= 0) {
			health = 0;
		}
		if (health >= 100) {

			health = 100;
		}

	}

	void TransitionSlider(){

		transformerBar.value = transformNum;
		if (transformNum > 0 && transformNum < 100) {
			transitionText.enabled = true;
		} else {

			transitionText.enabled = false;
		}

	}

	void SpeedSlider()
	{
		speedValue = (int)percentageValue (CharacterMovement.speed, 0.0f, 2000f);
		speedBar.value = speedValue;

	}

	void UpdateTexts()
	{

		transitionText.text = "In Transition.";
		transitionText.color = new Color(interfaceColor.r,interfaceColor.g,interfaceColor.b, flashing(1.0f));
		healthText.color = interfaceColor;
		speedText.color = interfaceColor;
		transitionBarFillImage.color = interfaceColor;
		//scoreText.text = collecteditems + " Items Collected";

		//craftLevelText.text = " Craft Level "+ craft.GetComponent<CharacterMeshComplete> ().animateCount+".";

	}

	void UpdateSlider(){
		
		healthBarFillImage.color = Color.Lerp (lowHealthColor, fullHealthColor, health / 100f);

		if (health < 20) 
		{
			healthBarFillImage.color  = new Color(healthBarFillImage.color.r, healthBarFillImage.color.g, healthBarFillImage.color.b, flashing(1.0f));
		}

		speedBarFillImage.color =  Color.Lerp (lowHealthColor, fullHealthColor, speedValue / 100f);


	}

	void UpdateIcons(){


		foreach (Image icon in icons) 
		{
			if (showIcons) {
				icon.enabled = true;

				icon.color = new Color (icon.color.r, icon.color.g, icon.color.b, flashing (1.0f));
			} else {

				icon.enabled = false;
			}
		}

		if ((craftScript.moveState == "ball" && craftScript.animateCount == 0) || (craftScript.moveState == "car" && craftScript.animateCount == 2)) {

			ShowHideCraftIcon (0);
			ShowHideCraftSliderIcon (0);
		}else if ((craftScript.moveState == "car" && craftScript.animateCount == 1) || (craftScript.moveState == "airplane" && craftScript.animateCount == 3)) {
			ShowHideCraftIcon (1);
			ShowHideCraftSliderIcon (1);

		} else if((craftScript.moveState == "airplane" && craftScript.animateCount == 2) || (craftScript.moveState == "jet" && craftScript.animateCount == 4))
		{
			ShowHideCraftIcon (2);
			ShowHideCraftSliderIcon (2);
		} else if ((craftScript.moveState == "jet" && craftScript.animateCount == 3) || (craftScript.moveState == "nasa" && craftScript.animateCount == 5)){
			
			ShowHideCraftIcon (3);
			ShowHideCraftSliderIcon (3);
		}else if (craftScript.moveState == "nasa" && craftScript.animateCount == 4)
		{
			ShowHideCraftIcon (4);
			ShowHideCraftSliderIcon (4);
		} 


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
