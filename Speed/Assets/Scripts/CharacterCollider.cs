﻿using UnityEngine;
using System.Collections;


public class CharacterCollider : MonoBehaviour {


	[Range(1,10)] public int healthDamage = 2;


	void OnCollisionEnter(Collision col){


		//float impact = Vector3.Dot (col.contacts [0].normal, col.relativeVelocity) * GetComponent<Rigidbody>().mass;
		//float impact = healthDamage * col.relativeVelocity.magnitude; 
		float impact = 	Vector3.Magnitude(GetComponent<Rigidbody>().velocity);
		//print (gameObject.name + "  has collided with " + col.gameObject.name +" reletive velocity: "
		//	+col.relativeVelocity +"   impact: "+impact);

		//		var vFinal = col.rigidbody.mass * col.relativeVelocity / (rigidbody.mass + col.rigidbody.mass);
		//		var impulse = vFinal * rigidbody.mass;



		if (col.gameObject.name == "building") {
			GameManager.health -= (int)impact / healthDamage;
		}

		if (col.gameObject.name == "ground") {
			//GameManager.health -= healthDamage;
		}

		if (col.gameObject.name == "circularGround") {
			///GameManager.health -= healthDamage;
		}

		if (col.gameObject.name == "Pyramid") {
			///GameManager.health -= healthDamage;
		}

		if (col.gameObject.name == "TorusKnot") 
		{
			GameManager.health -= (int)impact / healthDamage;
		}
		if (col.gameObject.name == "Earth") 
		{
			GameManager.health -= (int)impact / healthDamage;
		}



	}



	void OnTriggerEnter (Collider other)
	{

		//print (gameObject.name + "  has triggered with " + other.gameObject.name);

	}

	void OnTriggerExit ( Collider other )
	{


	}



}
