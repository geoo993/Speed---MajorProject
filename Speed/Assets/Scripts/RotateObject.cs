using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public float speed = 20f;


	void Start () {
	
	}

	void Update () {
	
		this.transform.Rotate (0.0f, speed * Time.deltaTime, 0.0f);
	}
}
