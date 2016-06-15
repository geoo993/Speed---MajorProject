using UnityEngine;
using System.Collections;

public class HeartPulse : MonoBehaviour {


	void Start () {

		this.name = "Heart";

		transform.position = new Vector3 (
			GameObject.Find("City").GetComponent<GenerateCity> ().transform.position.x + 500f, 
			GameObject.Find("City").GetComponent<GenerateCity> ().mapHeight * 2.5f, 
			GameObject.Find("City").GetComponent<GenerateCity> ().transform.position.z + 500f);

	}


}
