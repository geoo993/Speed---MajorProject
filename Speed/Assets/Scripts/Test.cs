using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Instantiate( Resources.Load ("Menu") );
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown ("space")) {

			GameObject.Find("Canvas/MenuBoard").GetComponent<Animator>().SetTrigger ("GameOver");
		}
	}
}
