﻿using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{
			GameManager.health += 25;
			Destroy (this.gameObject);
		}

		//print ("health: " + GameManager.health);

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}