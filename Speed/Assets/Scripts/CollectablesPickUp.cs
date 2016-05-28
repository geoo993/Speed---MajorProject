﻿using UnityEngine;
using System.Collections;

public class CollectablesPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{
			//Destroy (this.gameObject);
			//this.gameObject.SetActive (false);

			Vector3 pos = new Vector3(Random.Range (50, 800), Random.Range (100.0f, 400),Random.Range (50, 800));
			this.gameObject.transform.localPosition = pos;

			Vector3 scale = new Vector3 (Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f));
			this.transform.localScale = scale;

			this.transform.eulerAngles = new Vector3(Random.Range (0, 360),Random.Range (0, 360),Random.Range (0, 360));
			//city.GetComponent<Track> ().UpdateCollectable();


			GameManager.collecteditems += 1;
			print ("hit on enter");

		}

	}

	void OnCollisionExit(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{
			
			print ("exiting");

			//city.GetComponent<Track> ().CreateCollectable();

		}

	}




}