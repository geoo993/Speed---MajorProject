﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Items : MonoBehaviour {


	private GenerateCity city;
	public GameObject craft = null;
	public GameObject[] collectors = null; 

	[Range(1, 50)] public int healthPickUps = 5;
	[Range(1, 10)] public int resetPickUps = 5;
	[Range(5, 50)] public int transformPickUps = 5;

	[Range(0, 5)] public int transformStartingAmoungt = 5;

	[HideInInspector] public static List<GameObject> healthItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> transformerItems = new List<GameObject> ();
	//[HideInInspector] public List<GameObject> collectablesItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> resetItems = new List<GameObject> ();
	[HideInInspector] public Vector3 nextItemPos = Vector3.zero;
	[HideInInspector] private GameObject c;

	void Start () {

		city = GetComponent<GenerateCity> ();

		CharacterMeshComplete.tranformNum = transformStartingAmoungt;

		createResetItems ();
		CreateHealthAndTransformItems ();
		CreateCollectable ();


	}
	void Update(){

		nextItemPos = c.transform.position;

	}

	void createResetItems(){
		
		//resetItems objects // cube
		for (int r = 0; r < resetPickUps; r++) 
		{
			Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight ), Random.Range (0, city.mapWidth));
			GameObject b = (GameObject) Instantiate(collectors [0], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f));
			b.transform.localScale = scale;
			b.transform.parent = this.transform;

			b.GetComponent<Rigidbody> ().useGravity = false;

			b.name = "resetItem" + r;
			resetItems.Add (b);
		}

	}


	public void CreateCollectable()
	{

		Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (100.0f, city.mapHeight),Random.Range (0, city.mapWidth));
		c = (GameObject) Instantiate(collectors [1], pos, Quaternion.identity) as GameObject;

		Vector3 scale = new Vector3 (Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f));
		c.transform.localScale = scale;
		c.transform.parent = this.transform;

		c.GetComponent<Rigidbody> ().useGravity = true;

		c.name = "CollectableItem";
		c.tag = "RadarCollectable";

	}



	void CreateHealthAndTransformItems()
	{

		//transformers objects // sphere
		for (int t = 0; t < transformPickUps - transformStartingAmoungt; t++) {

			Vector3 pos = new Vector3 (Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight), Random.Range (0, city.mapWidth));
			GameObject a = (GameObject)Instantiate (collectors [2], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f));
			a.transform.localScale = scale;
			a.transform.parent = this.transform;

			a.GetComponent<Rigidbody> ().useGravity = (Random.Range (0, 2) == 0);

			a.name = "transformers" + t;
			transformerItems.Add (a);


		}



		//health objects // capsule
		for (int h = 0; h < healthPickUps; h++) {

			Vector3 pos = new Vector3 (Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight/2), Random.Range (0, city.mapWidth));
			GameObject a = (GameObject)Instantiate (collectors [3], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f));
			a.transform.localScale = scale;
			a.transform.parent = this.transform;

			a.GetComponent<Rigidbody> ().isKinematic = false;

			a.name = "Health" + h;
			healthItems.Add (a);
		}


	}


	public static void RemoveObjectFromHealthList(GameObject obj, List <GameObject> objectList)
	{
		//print ("list of items: "+objectList.Count+" in and index: "+ objectList.IndexOf(obj));

		List <GameObject> newList = new List<GameObject> ();

		for (int i = 0; i < objectList.Count; i++) {

			if (objectList [i] == obj) {

				Destroy (objectList [i]);
				continue;
			} else {

				newList.Add (objectList [i]);
			}
		}

		objectList.RemoveRange (0, objectList.Count);
		objectList.AddRange (newList);

		//print (" count the list" + objectList.Count);
	}


}
