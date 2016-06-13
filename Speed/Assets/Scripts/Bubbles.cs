using UnityEngine;
using System.Collections;

public class Bubbles : MonoBehaviour {


	void Start () 
	{
		this.name = "FloatingBubbles";
	}

	void Update () {
		GetComponent<ParticleSystem>().startColor = GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor;
	}
}
