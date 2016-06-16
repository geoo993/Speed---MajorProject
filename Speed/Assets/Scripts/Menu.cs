using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Text gameOverText = null;
	public Text restartText = null;

	void Start () {
		this.name = "MenuBoard";
		this.transform.parent = GameObject.Find ("Canvas").transform;
	}

	void Update(){

		gameOverText.color = GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor;

		restartText.color =	new Color(
			GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor.r, 
			GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor.g,
			GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor.b, flashing(1.5f));

		if (GameObject.Find("GameManager").GetComponent<GameManager>().controlsType == GameManager.ControlsType.PS4_Controller) {

			restartText.text = "Press OPTIONS to check your score";
		}else if (GameObject.Find("GameManager").GetComponent<GameManager>().controlsType == GameManager.ControlsType.Xbox_Controller) {

			restartText.text = "Press START to check your score";
		}else if (GameObject.Find("GameManager").GetComponent<GameManager>().controlsType == GameManager.ControlsType.Keyboard) {
			restartText.text = "Press SPACE to check your score";
		}

	}

	public float flashing( float duration)
	{
		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

		return amplitude;
	}
}
