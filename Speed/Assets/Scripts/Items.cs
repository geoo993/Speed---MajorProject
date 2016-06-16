using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Items : MonoBehaviour {


	public GameObject[] collectors = null; 

	[Range(10,50)] public int curvesItemsFrequency = 10;
	[Range(1, 50)] public int healthPickUps = 5;
	//[Range(1, 10)] public int resetPickUps = 5;
	[Range(1, 100)] public int coinPickUps = 10;
	[Range(0, 50)] public int transformPickUps = 5;
	public static int collectablePickUps = 20;

	public static int numberCollected = 0;

	public static List<Vector3> easyPipeCollectablesItemsPositions = new List<Vector3>();
	public static List<Vector3> hardPipeCollectablesItemsPositions = new List<Vector3>();

	[HideInInspector] public static List<GameObject> healthItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> transformerItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> collectablesItems = new List<GameObject> ();
	[HideInInspector] public static List<GameObject> coinItems = new List<GameObject> ();

	public static List<GameObject> finishItems = new List<GameObject> ();

	[HideInInspector] public static List<Vector3> fourPoints1 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints2 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints3 = new List<Vector3>();
	[HideInInspector] public static List<Vector3> fourPoints4 = new List<Vector3>();


	public IEnumerator CreateCollectable()
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
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3(Random.Range(100f, 900f), 100.0f, Random.Range(100f, 900f));

				c.GetComponent<Rigidbody> ().useGravity = true;
			}else if (col == 1)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3(Random.Range(100f, 900f), 100.0f, Random.Range(100f, 900f));

				c.GetComponent<Rigidbody> ().useGravity = true;
			}else if (col == 2)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3 (Random.Range(400f,800f), 100f, Random.Range(400f,800f));;
				c.GetComponent<Rigidbody> ().useGravity = true;

			}else if (col == 3)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = AlignObjects(GameObject.Find ("Heart").transform.position, new Vector3(500f, 0.0f, 500f), 5 );

				c.GetComponent<Rigidbody> ().useGravity = true;
			}else if (col == 4)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = AlignObjects(GameObject.Find ("Heart").transform.position, new Vector3(500f, 0.0f, 500f), 4 );

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 5)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = AlignObjects(GameObject.Find ("Heart").transform.position, new Vector3(500f, 0.0f, 500f), 3 );

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 6)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = AlignObjects(GameObject.Find ("Heart").transform.position, new Vector3(500f, 0.0f, 500f), 1.5f );

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 7) 
			{
				c.transform.parent = GameObject.Find ("Heart").transform;
				c.GetComponent<BoxCollider> ().enabled = false;
				c.transform.localPosition = Vector3.zero;
				c.GetComponent<Rigidbody> ().isKinematic = true;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 8) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/hardPipeParent1").transform;
				c.transform.localPosition = hardPipeCollectablesItemsPositions[0];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 9) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/hardPipeParent2").transform;
				c.transform.localPosition = hardPipeCollectablesItemsPositions[1];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 10) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/hardPipeParent3").transform;
				c.transform.localPosition = hardPipeCollectablesItemsPositions[2];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 11) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemHard/hardPipeParent4").transform;
				c.transform.localPosition = hardPipeCollectablesItemsPositions[3];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}
			else if (col == 12){
				c.transform.parent = GameObject.Find ("Sun").transform;
				c.transform.localPosition = Vector3.zero;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 13) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemEasy/easyPipeParent1").transform;
				c.transform.localPosition = easyPipeCollectablesItemsPositions[0];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 14) 
			{
				c.transform.parent = GameObject.Find ("SwirlPipeSystemEasy/easyPipeParent2").transform;
				c.transform.localPosition = easyPipeCollectablesItemsPositions[1];

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 15 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(-0.6f, 0.0f, 0.0f);

				c.GetComponent<Rigidbody> ().useGravity = false;
			}else if (col == 16 ) 
			{
				c.transform.parent = GameObject.Find ("Earth").transform;
				c.transform.localPosition = new Vector3(0.6f, 0.0f, 0.0f);

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 17) 
			{
				c.transform.parent = GameObject.Find ("TorusArc").transform;
				c.transform.localPosition = Vector3.zero;

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 18)
			{
				c.transform.parent = this.transform;
				c.transform.localPosition = new Vector3(0, 40, 1000);

				c.GetComponent<Rigidbody> ().useGravity = false;

			}else if (col == 19)
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
		collectablesItems [7].GetComponent<BoxCollider> ().enabled = true;

		CreateHealthItems ();
		createCoinItems ();

		fourPoints1.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints1.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints1.Add (new Vector3(0, 10, 1000));
		AddCurves (fourPoints1, collectors [0],1);

		fourPoints2.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints2.Add (new Vector3(1000, 60, 1000));
		fourPoints2.Add (new Vector3(0, 10, 1000));
		AddCurves (fourPoints2, collectors [0],2);

		fourPoints3.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints3.Add (new Vector3(1000, 60, 1000));
		fourPoints3.Add (new Vector3(1000, 80, 0));
		AddCurves (fourPoints3, collectors [0],3);

		fourPoints4.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints4.Add (new Vector3(Random.Range(0, 1000), Random.Range(0, 1000),Random.Range(0, 1000)));
		fourPoints4.Add (new Vector3(1000, 40, 1000));
		AddCurves (fourPoints4, collectors [0],4);


//		print ("coins: "+ coinItems.Count);
//		print ("health: "+ healthItems.Count);
//		print ("collectables: "+ collectablesItems.Count);
//		print ("fourPoints1: "+ fourPoints1.Count);
//		print ("fourPoints2: "+ fourPoints2.Count);
//		print ("fourPoints3: "+ fourPoints3.Count);
//		print ("fourPoints4: "+ fourPoints4.Count);

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
			Vector3 pos = new Vector3(Random.Range (0, 1000f), Random.Range (5.0f, 1000f ), Random.Range (0, 1000f));
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

			coinItems[ii].transform.position = AlignObjects(collectablesItems[13].transform.position, collectablesItems[12].transform.position, (v1 * 0.25f) );
			coinItems[ii].GetComponent<Rigidbody> ().useGravity = true;
			v1+= 1f;
		}

	}
		

	Vector3 AlignObjects(Vector3 pos1, Vector3 pos2, float step)
	{
		return (pos1 + pos2) / step ;
	}

	public void CreateTransformItems()
	{

		//transformers objects // sphere
		//for (int t = 0; t < transformPickUps ; t++) {

//			Vector3 pos = new Vector3 (Random.Range (200.0f, 800.0f), Random.Range (100.0f, 200.0f), Random.Range (200f, 800f));
//			GameObject a = (GameObject)Instantiate (collectors [2], pos, Quaternion.identity);
//
//			Vector3 scale = new Vector3 (Random.Range (20.0f, 35.0f), Random.Range (20.0f, 25.0f), Random.Range (20.0f, 25.0f));
//			a.transform.localScale = scale;
//			a.transform.parent = this.transform;
//
//			a.GetComponent<Rigidbody> ().useGravity = true;
//
//			a.name = "transformers" ;
//			transformerItems.Add (a);
//
		//}

	}

	void CreateHealthItems()
	{
		//health objects // capsule
		for (int h = 0; h < healthPickUps; h++) {

			Vector3 pos = new Vector3 (Random.Range (0, 1000f), Random.Range (0.0f, 1000f), Random.Range (0, 1000f));
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

	private void AddCurves( List<Vector3> points, GameObject obj, int id)
	{

		List<GameObject> items = new List<GameObject>();
		const float directionScale = 0.5f;


		GameObject newObj = new GameObject ();
		newObj.name = "Curve"+id;
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
