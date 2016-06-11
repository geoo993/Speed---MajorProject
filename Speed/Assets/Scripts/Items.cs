using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Items : MonoBehaviour {


	private GenerateCity city;
	public GameObject craft = null;
	public GameObject[] collectors = null; 

	[Range(10,50)] public int curvesItemsFrequency = 10;
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

	[HideInInspector] public static List<Vector3> fourPoints1 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints2 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints3 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints4 = new List<Vector3>();

	void Start () {

		city = GetComponent<GenerateCity> ();

		CharacterMeshComplete.tranformNum = transformStartingAmoungt;

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


		Vector3 center = new Vector3(0,10,0);
		int numberOfItemsInCircles = 10; 
		for (int i = (coinItems.Count - numberOfItemsInCircles); i < coinItems.Count; i++) {
			
			float chunk = (i * 1.0f) / numberOfItemsInCircles;
			float angle = chunk * Mathf.PI * 2;

			coinItems[i].transform.position = CircularPoint (center, angle, 50.0f);
			coinItems[i].GetComponent<Rigidbody> ().useGravity = true;
		}

		int fromm1 = (coinItems.Count - 20);
		int too1 = coinItems.Count - 10;
		float v1 = 1;
		for (int ii = fromm1; ii < too1; ii++) {

			coinItems[ii].transform.position = AlignObjects(collectablesItems[7].transform.position, collectablesItems[8].transform.position, (v1 * 0.5f) );
			coinItems[ii].GetComponent<Rigidbody> ().useGravity = true;
			v1+= 1f;
		}

		int fromm2 = (coinItems.Count - 30);
		int too2 = coinItems.Count - 20;
		float v2 = 1;
		for (int iii = fromm2; iii < too2; iii++) {

			coinItems[iii].transform.position = AlignObjects(collectablesItems[8].transform.position, collectablesItems[9].transform.position, (v2 * 0.5f) );
			coinItems[iii].GetComponent<Rigidbody> ().useGravity = true;
			v2+= 1f;
		}


	}

	Vector3 AlignObjects(Vector3 pos1, Vector3 pos2, float step)
	{
		return (pos1 + pos2) / step ;
	}

	private IEnumerator CreateCollectable()
	{
		WaitForSeconds wait = new WaitForSeconds (2.0f);

		//print (collectablesItemsPositions.Count);
		yield return wait;

		for (int col = 0; col < collectablePickUps; col++) {
			//Vector3 pos = new Vector3(Random.Range (0, city.mapWidth), Random.Range (100.0f, city.mapHeight),Random.Range (0, city.mapWidth));
			GameObject c = (GameObject) Instantiate(collectors [1], Vector3.zero, Quaternion.identity) as GameObject;

			Vector3 scale = Vector3.one * 10;
			c.transform.localScale = scale;

			if (col == 0) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemEasy/collectablePipeParent").transform;
				c.transform.localPosition = pipeCollectablesItemsPositions[0];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 1) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/collectablePipeParent").transform;
				c.transform.localPosition = pipeCollectablesItemsPositions[1];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 2) 
			{
				c.transform.parent = GameObject.Find ("Heart").transform;
				c.GetComponent<BoxCollider> ().enabled = false;
				c.transform.localPosition = Vector3.zero;
				c.GetComponent<Rigidbody> ().isKinematic = true;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 3){
				c.transform.parent = GameObject.Find ("Sun").transform;
				c.transform.localPosition = Vector3.zero;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 4) 
			{
				c.transform.parent = GameObject.Find ("TorusArc").transform;
				c.transform.localPosition = Vector3.zero;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 5 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(-0.6f, 0.0f, 0.0f);

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 6 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(0.6f, 0.0f, 0.0f);

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 7)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3 (Random.Range(200f,800f), 100f, Random.Range(200f,800f));;
				c.GetComponent<Rigidbody> ().useGravity = true;

			}else if (col == 8)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3(0, 40, 1000);

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 9)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3(0, 25, 0);

				c.GetComponent<Rigidbody> ().useGravity = false;
			}


			c.name = "CollectableItem"+ col;
			c.tag = "RadarCollectable";

			collectablesItems.Add (c);
		}

		yield return wait;
		//collectablesItems [2].transform.parent = GameObject.Find("Earth").transform;
		collectablesItems [2].GetComponent<BoxCollider> ().enabled = true;

		createCoinItems ();

		fourPoints1.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints1.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints1.Add (new Vector3(0, 10, 1000));
		AddCurves (fourPoints1, collectors [0]);

		fourPoints2.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints2.Add (new Vector3(1000, 60, 1000));
		fourPoints2.Add (new Vector3(0, 10, 1000));
		AddCurves (fourPoints2, collectors [0]);

		fourPoints3.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints3.Add (new Vector3(1000, 60, 1000));
		fourPoints3.Add (new Vector3(1000, 80, 0));
		AddCurves (fourPoints3, collectors [0]);

		fourPoints4.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints4.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints4.Add (new Vector3(1000, 40, 1000));
		AddCurves (fourPoints4, collectors [0]);
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

			a.GetComponent<Rigidbody> ().useGravity = (Random.Range (0, 2) == 0);

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




//	................. curves......................

	private void AddCurves( List<Vector3> points, GameObject obj)
	{

		List<GameObject> items = new List<GameObject>();
		const float directionScale = 0.5f;


		GameObject newObj = new GameObject ();
		newObj.name = "curve items";
		newObj.AddComponent<LineRenderer>();
		LineRenderer lineRenderer = newObj.GetComponent<LineRenderer> ();

		Material mat = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.material = mat;
		lineRenderer.SetColors(Color.red, Color.green);
		lineRenderer.SetWidth(1f, 1f);

		int curveSteps = 10 ;
		lineRenderer.SetVertexCount(curveSteps + 1);
		Vector3 lineStart = GetPoint(0f, points);
		lineRenderer.SetPosition(0, lineStart + GetDirection(0f, points) * directionScale);

		for (int i = 1; i <= curveSteps; i++) {
			Vector3 lineEnd = GetPoint(i / (float)curveSteps, points);
			lineRenderer.SetPosition (i  , lineEnd + GetDirection(i / (float)curveSteps, points) * directionScale);
		}
			
	
		items.Add (obj);

		if (curvesItemsFrequency <= 0 || items == null || items.Count == 0) {
			return;
		}
		float stepSize = curvesItemsFrequency * items.Count;
		stepSize = 1f / (stepSize - 1);

		for (int p = 0, f = 0; f < curvesItemsFrequency; f++) {
			for (int i = 0; i < items.Count; i++, p++) {

				GameObject item = Instantiate(items[i]) as GameObject;
				Vector3 position = GetPoint(p * stepSize, points);
				item.transform.localPosition = position;

				item.transform.LookAt(position + GetDirection(p * stepSize, points));
				item.transform.parent = newObj.transform;

				item.GetComponent<Rigidbody> ().isKinematic = true;
				coinItems.Add (item);
			}
		}

		lineRenderer.enabled = false;

	}


	public Vector3 GetPoint (float t,List<Vector3> curvePoints) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = curvePoints.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) ;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint (
			GetPointForCubicCurve (
				curvePoints [i], 
				curvePoints [i + 1], 
				curvePoints [i + 2],
				curvePoints [i + 3], t));

	}

	public Vector3 GetVelocity (float t, List<Vector3> curvePoints) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = curvePoints.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) ;
			i = (int)t;
			t -= i;
			i *= 3;
		}

		return transform.TransformPoint(
			GetFirstDerivativeForCubicCurve(
				curvePoints[i], 
				curvePoints[i + 1], 
				curvePoints[i + 2], 
				curvePoints[i + 3], t)) - 
			transform.position;


	}

	////reduces the velocity line 
	public Vector3 GetDirection (float t, List<Vector3> curvePoints) {
		return GetVelocity(t, curvePoints).normalized;
	}

	public static Vector3 GetPointForCubicCurve (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}
	public static Vector3 GetFirstDerivativeForCubicCurve  (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}


}
