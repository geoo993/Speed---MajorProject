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
	[Range(1, 100)] public int coinPickUps = 10;
	[Range(0, 50)] public int transformPickUps = 5;
	public static int collectablePickUps = 10;

	[Range(0, 5)] public int transformStartingAmoungt = 5;
	public static int numberCollected = 0;
	private bool ifOver = true;

	public static List<Vector3> pipeCollectablesItemsPositions = new List<Vector3>();

	[HideInInspector] public static List<GameObject> healthItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> transformerItems = new List<GameObject> ();
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


//		int whenToGenerateBuildingsCollectable1 = GenerateCity.buildingsIndex.Count/2;
//		int whenToGenerateBuildingsCollectable2 = whenToGenerateBuildingsCollectable1 + 2;
//
//		if (numberCollected > whenToGenerateBuildingsCollectable1 && numberCollected < whenToGenerateBuildingsCollectable2) 
//		{
//			if ( ifOver){
//				AddNewCollectableItemPositon ();
//
//				ifOver = false;
//			}
//
//		}

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

	Vector3 CircularPoint(Vector3 center, float angle, float radius) { 
		float x = Mathf.Sin(angle) * radius;
		float z = Mathf.Cos(angle) * radius;
		Vector3 pos = new Vector3(x, 0, z) + center;
		return pos; 
	}
		
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


		Vector3 center = craft.transform.position;
		int numberOfItemsInCircles = 10; 

		for (int i = (coinItems.Count - numberOfItemsInCircles); i < coinItems.Count; i++) {
			
			float chunk = (i * 1.0f) / numberOfItemsInCircles;
			float angle = chunk * Mathf.PI * 2;

			coinItems[i].transform.position = CircularPoint (center, angle, 50.0f);
			coinItems[i].GetComponent<Rigidbody> ().useGravity = true;
		}

	}

	private IEnumerator CreateCollectable()
	{
		WaitForSeconds wait = new WaitForSeconds (2.0f);

		//print (collectablesItemsPositions.Count);
		yield return wait;

		for (int col = 0; col < 10; col++) {
			//Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (100.0f, city.mapHeight),Random.Range (0, city.mapWidth));
			GameObject c = (GameObject) Instantiate(collectors [1], Vector3.zero, Quaternion.identity) as GameObject;

			Vector3 scale = new Vector3 (Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f), Random.Range (8.0f, 14.0f));
			c.transform.localScale = scale;

			if (col == 0) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemEasy/collectablePipeParent").transform;
				c.transform.localPosition = pipeCollectablesItemsPositions[0];

			}else if (col == 1) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/collectablePipeParent").transform;
				c.transform.localPosition = pipeCollectablesItemsPositions[1];

			}else if (col == 2) 
			{
				c.transform.parent = GameObject.Find ("Heart").transform;
				c.GetComponent<BoxCollider> ().enabled = false;
				c.transform.localPosition = Vector3.zero;

			}else if (col == 3){
				c.transform.parent = GameObject.Find ("Sun").transform;
				c.transform.localPosition = Vector3.zero;

			}else if (col == 4) 
			{
				c.transform.parent = GameObject.Find ("TorusArc").transform;
				c.transform.localPosition = Vector3.zero;

			}else if (col == 5 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(-0.6f, 0.0f, 0.0f);
			}else if (col == 6 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(0.6f, 0.0f, 0.0f);
			}



			c.GetComponent<Rigidbody> ().useGravity = false;

			c.name = "CollectableItem"+ col;
			c.tag = "RadarCollectable";

			collectablesItems.Add (c);
		}

		yield return wait;
		//collectablesItems [2].transform.parent = GameObject.Find("Earth").transform;
		collectablesItems [2].GetComponent<BoxCollider> ().enabled = true;
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

			Vector3 pos = new Vector3 (Random.Range (0, city.mapWidth), Random.Range (0.0f, city.mapHeight), Random.Range (0, city.mapWidth));
			GameObject a = (GameObject)Instantiate (collectors [3], pos, Quaternion.identity);

			Vector3 scale = new Vector3 (Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f), Random.Range (10.0f, 15.0f));
			a.transform.localScale = scale;
			a.transform.parent = this.transform;

			a.GetComponent<Rigidbody> ().isKinematic = (Random.Range (0, 2) == 0);

			a.name = "Health" + h;
			healthItems.Add (a);
		}
			
	}


	public static void RemoveObjectFromList(GameObject obj, List <GameObject> objectList)
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
