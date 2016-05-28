using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject city = null;
	public GameObject camera = null;
	public GameObject craft = null;

	public static int health = 80;
	public static int speedValue = 100;

	public Slider healthBar;
	public Slider speedBar;
	public Slider transformerBar;


	public Image healthBarFillImage;
	public Image speedBarFillImage;

	public Text transitionText;
	public Text craftLevelText;
	public Text healthText;
	public Text speedText;
	public Text scoreText;
	public Image[] icons = null;
	public static float transformNum = 100;
	private Color fullHealthColor = Color.green;
	private Color lowHealthColor = Color.red;

	public bool showIcons = false;


	public static int collecteditems = 0; 

	void Start () {

		//InvokeRepeating ("ReduceHealth", 1, 1);

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
		transitionText.color = new Color(transitionText.color.r,transitionText.color.g,transitionText.color.b, flashing(1.0f));

		scoreText.text = collecteditems + " Items Collected";

		craftLevelText.text = " Craft Level "+ craft.GetComponent<CharacterMeshComplete> ().animateCount+".";

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
