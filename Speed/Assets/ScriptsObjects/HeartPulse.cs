using UnityEngine;
using System.Collections;

public class HeartPulse : MonoBehaviour {

	public GameObject city = null;

	void Start () {

		transform.position = new Vector3 (city.GetComponent<GenerateCity> ().transform.position.x + city.GetComponent<GenerateCity> ().mapWidth/2, 
			-city.GetComponent<GenerateCity> ().mapHeight/4, 
			city.GetComponent<GenerateCity> ().transform.position.z + city.GetComponent<GenerateCity> ().mapWidth/2);

	}

}
