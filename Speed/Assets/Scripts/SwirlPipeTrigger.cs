using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {


	public GameObject city = null;

	void Awake(){

	}

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

	}



	void OnTriggerEnter (Collider other)
	{

		//print (gameObject.name + "  has triggered with " + other.gameObject.name);


	}

	void OnTriggerExit ( Collider other )
	{


	}



}
