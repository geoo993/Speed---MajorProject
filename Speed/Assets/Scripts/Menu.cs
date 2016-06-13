using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Text restartPress = null;

	void Start () {
		this.name = "MenuBoard";
		this.transform.parent = GameObject.Find ("Canvas").transform;
	}

	void Update(){

		restartPress.color =	new Color(
			restartPress.color.r, 
			restartPress.color.g,
			restartPress.color.b, flashing(1.5f));
	}

	public float flashing( float duration)
	{
		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

		return amplitude;
	}
}
