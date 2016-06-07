using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Items : MonoBehaviour {


	private GenerateCity city;
	public GameObject craft = null;
	public GameObject[] collectors = null; 

	[Range(1, 50)] public int healthPickUps = 5;
	//[Range(1, 10)] public int resetPickUps = 5;
	[Range(1, 50)] public int coinPickUps = 10;
	[Range(5, 50)] public int transformPickUps = 5;

	[Range(0, 5)] public int transformStartingAmoungt = 5;
	[HideInInspector] public static int numberCollected = 0;
	private bool ifOver = true;


	[HideInInspector] public static List<GameObject> healthItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> transformerItems = new List<GameObject> ();
	[HideInInspector] public static List<Vector3> collectablesItemsPositions = new List<Vector3> ();
	[HideInInspector] public static List<GameObject> collectablesItems = new List<GameObject> ();
	//[HideInInspector] public static List<GameObject> resetItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> coinItems = new List<GameObject> ();

	void Start () {

		city = GetComponent<GenerateCity> ();

		CharacterMeshComplete.tranformNum = transformStartingAmoungt;

		//createResetItems ();
		createCoinItems ();
		CreateHealthAndTransformItems ();
		StartCoroutine(CreateCollectable ());

	}
	void Update()
	{

		if (numberCollected > 10 && numberCollected < 12) 
		{
			if ( ifOver){
				
				AddNewCollectableItemPositon ();
				print ("over 10");
				ifOver = false;
			}


		}

	}

	private void AddNewCollectableItemPositon(){

		Vector3 pos = new Vector3 (Random.Range(200f,800f), 100f, Random.Range(200f,800f));
		GameObject c = (GameObject) Instantiate(collectors [1], pos, Quaternion.identity) as GameObject;

		Vector3 scale = new Vector3 (Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f));
		c.transform.localScale = scale;
		c.transform.parent = this.transform;

		c.GetComponent<Rigidbody> ().useGravity = true;

		c.name = "CollectableItem";
		c.tag = "RadarCollectable";

		collectablesItems.Add (c);
	}


//	void createResetItems(){
//		
//		//resetItems objects // cube
//		for (int r = 0; r < resetPickUps; r++) 
//		{
//			Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight ), Random.Range (0, city.mapWidth));
//			GameObject b = (GameObject) Instantiate(collectors [0], pos, Quaternion.identity);
//
//			Vector3 scale = new Vector3 (Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f));
//			b.transform.localScale = scale;
//			b.transform.parent = this.transform;
//
//			b.GetComponent<Rigidbody> ().useGravity = false;
//
//			b.name = "resetItem" + r;
//			resetItems.Add (b);
//		}
//
//	}

	void createCoinItems(){

		for (int r = 0; r < coinPickUps; r++) 
		{
			Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (5.0f, city.mapHeight ), Random.Range (0, city.mapWidth));
			GameObject b = (GameObject) Instantiate(collectors [0], pos, Quaternion.identity);

			//Vector3 scale = new Vector3 (Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f));
			//b.transform.localScale = scale;
			b.transform.localRotation = Quaternion.Euler(90, 0f, 0f);
			b.transform.parent = this.transform;

			b.GetComponent<Rigidbody> ().useGravity = (Random.Range (0, 2) == 0);

			b.name = "coinItem" + r;
			coinItems.Add (b);
		}

	}

	private IEnumerator CreateCollectable()
	{
		WaitForSeconds wait = new WaitForSeconds (2.0f);


		//print (collectablesItemsPositions.Count);
		yield return wait;

		for (int col = 0; col < collectablesItemsPositions.Count; col++) {
			//Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (100.0f, city.mapHeight),Random.Range (0, city.mapWidth));
			GameObject c = (GameObject) Instantiate(collectors [1], collectablesItemsPositions[col], Quaternion.identity) as GameObject;

			Vector3 scale = new Vector3 (Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f));
			c.transform.localScale = scale;
			c.transform.parent = this.transform;

			c.GetComponent<Rigidbody> ().useGravity = false;

			c.name = "CollectableItem"+ col;
			c.tag = "RadarCollectable";

			collectablesItems.Add (c);
		}

		collectablesItems [2].transform.parent = GameObject.Find("Earth").transform;
	}


	void CreateHealthAndTransformItems()
	{

		//transformers objects // sphere
		for (int t = 0; t < transformPickUps - transformStartingAmoungt; t++) {

			Vector3 pos = new Vector3 (Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight), Random.Range (0, city.mapWidth));
			GameObject a = (GameObject)Instantiate (collectors [2], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (10.0f, 18.0f), Random.Range (10.0f, 18.0f), Random.Range (10.0f, 18.0f));
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

			Vector3 scale = new Vector3 (Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f));
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
