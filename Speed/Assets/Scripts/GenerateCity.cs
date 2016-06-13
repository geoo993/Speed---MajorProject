using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class GenerateCity : MonoBehaviour {


	public Color buildingsTopColor = Color.white;
	private Color buildingsBottomColor = Color.black;
	public Color buildingsAreaColor = Color.white;

	public Image builingsImage = null;
	public GameObject spotlight = null;

	private float stretcher = 10f;
	[Range(10,50)] public int minSize = 10;
	[Range(100,1000)] public int mapWidth = 1000;
	[Range(100,1000)] public int mapHeight = 1000;

	[Range(1,20)] public int buildingsFrequency = 10;
	[Range(50f,600f)] public float buildingsLimit = 600.0f;

	[Range(0, 50)] public int buildingsMinHeight = 10;
	[Range(10, 100)] public int buildingsMaxHeight = 60;

	[HideInInspector] public static List<GameObject> buildings = new List<GameObject>();
	[HideInInspector] public static List<int> buildingsIndex = new List<int>();
	[HideInInspector] public static List<int> buildingsRemovedAreaIndex = new List<int>();
	[HideInInspector] public static int buildingsCurrentIndex = 0;
	[HideInInspector] public static bool addOneBuilding = false;
	private float lerpId = 0;


	private List<GameObject> areas = new List<GameObject>();
	private List<GameObject> areasIndexDelete = new List<GameObject>();
	private List<Vector3> edgePoints = new List<Vector3>();
	private List<Vector3> mapEdgePoints = new List<Vector3>();

	private int xSize, ySize, zSize, gameObjectCount ;
	public int roundness = 6;
	private bool roundTop = false;
	public bool roundFront, roundBack, roundSides = false;

	[Range(0, 16)] public int textureIndex = 2;
	[Range(0.02f, 0.98f)] public float colorHeight = 0.3f;

	public bool addTexture = false;
	private Texture[] stripes;


	private List <Vector3> verticesCopy = new List<Vector3> ();
	private List<int[]> controlPoints = new List<int[]>();
	private List<int> listOfVerticesIndexes = new List<int>();
	private List<int> pivotControlPoint= new List<int>();
	private List<int> topControlPointIndexes = new List<int>();
	private List<int> frontControlPointIndexes = new List<int>();
	private List<int> backControlPointIndexes = new List<int>();
	private List<int> sidesControlPointIndexes = new List<int>();
	private List<Vector3> vertices = new List<Vector3>();
	private List<Vector3> normals = new List<Vector3>();
	private List<Vector2> uv = new List<Vector2>();
	private int[] triangles; 

	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {

		triangles[i] = v00;
		triangles[i + 4] = v01;
		triangles[i + 1] = triangles[i + 4];
		triangles[i + 3] = v10;
		triangles[i + 2] = triangles[i + 3];
		triangles[i + 5] = v11;

		return i + 6;
	}



	private void Awake () {

		this.transform.name = "City";
		//StartCoroutine(GenerateCityBuildings ());
		GenerateCityBuildings();

	}
	private void Update(){

		GrowBuilding ();
		UpdateAreasColor ();
		UpdateBuildingsColor ();
	}



	//private IEnumerator GenerateCityBuildings () 
	private void GenerateCityBuildings () 
	{
		//WaitForSeconds wait = new WaitForSeconds (0.01f);

		GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		startCube.transform.localScale = new Vector3 (mapWidth,1,mapHeight);
		startCube.transform.position = new Vector3(transform.position.x + mapWidth/2, transform.position.y,transform.position.z + mapHeight/2);
		areas.Add (startCube);
		addMapEdges ();

		for (int e = 0; e < mapEdgePoints.Count; e++) {
			createSpotlights (mapEdgePoints [e] + this.transform.localPosition);
		}

		for (int i = 0; i < areas.Count; i++) {

			float choice = Random.Range(0.0f,1.0f);

			if (choice <= 0.5f){
				splitX ( areas [i] );
			}else{

				splitZ ( areas [i] );
			}
			//yield return wait;
		}

		for (int j = 0; j< areas.Count; j++) {

			for (int a = 0; a < areasIndexDelete.Count; a++) {

				areas.Remove (areasIndexDelete [a]);
			}
			if (areas [j] == null) {

				areas.RemoveAt(j);
			}
		}

		//yield return wait;

		for (int i = 0; i < areas.Count; i++) {

			GetVectors (areas [i]);
		}

		//yield return wait;

		for (int i = 0; i < areas.Count; i++) {

			buildingsIndex.Add (i);
		}


		//print ("buildings index amount: "+buildingsIndex.Count+"   areas: "+areas.Count);

	}

	public static void RemoveIntFromList(int num, List <int> numList)
	{
		//print ("list of items: "+numList.Count+" in and index: "+ num);

		List <int> newList = new List<int> ();

		for (int i = 0; i < numList.Count; i++) {

			if (numList [i] == num) {

				//print (numList [i]);

				//numList.Remove(numList [i]);
				//continue;

			} else {

				newList.Add (numList [i]);
			}
		}

		numList.RemoveRange (0, numList.Count);
		numList.AddRange (newList);

		//print (numList.Count);
	}
		

	private void AddBuilding(string name, Vector3 position, int x, int y, int z){

		GameObject building = GetComponent<Building>().CreateBuilding (position, x, y, z) as GameObject;
		building.transform.parent = this.transform;
		building.name = name;
		building.transform.localScale = new Vector3 (Random.Range (0.5f, 0.8f), 0f, Random.Range (0.5f, 0.8f));
		buildings.Add (building);

		//building.AddComponent<MakeRadarObject> ();

		//builingsImage.color = buildingsTopColor;
		//building.GetComponent<MakeRadarObject> ().image = builingsImage;

		lerpId = 0;
	}


	public void MakeBuilding(int i)
	{

		//for (int i = 0; i < areas.Count; i++) 
		//{

			xSize = (int)areas[i].GetComponent<MeshRenderer> ().bounds.size.x;
			zSize = (int)areas[i].GetComponent<MeshRenderer> ().bounds.size.z;

			float distanceToCenter = Vector3.Distance (new Vector3(mapWidth/2, 0, mapHeight/2), areas [i].transform.localPosition);
			float distanceToMapEdge = Vector3.Distance (GetClosestEdge (areas [i].transform.localPosition, mapEdgePoints), areas [i].transform.localPosition);

			//move from center
			float xx = areas[i].transform.position.x - ((float)xSize/2.0f);
			float zz = areas[i].transform.position.z - ((float)zSize/2.0f);

			Vector3 pivotPoint = new Vector3 (xx,areas[i].transform.position.y, zz);

			roundTop = (Random.Range (0, 2) == 0);

			if (distanceToCenter < buildingsLimit) {

				buildingsRemovedAreaIndex.Add(i);

				int splitSize = 100;//(int)buildingsLimit / buildingsFrequency;

				if (xSize > splitSize || zSize > splitSize) {

					int xCount = 1;
					while (xSize / xCount > splitSize) {

						xCount++;
					}
					float xOffset = xSize / xCount;

					int zCount = 1;
					while (zSize / zCount > splitSize) {
						zCount++;
					}
					float zOffset = zSize / zCount;

					List<Vector3> pointsInArea = new List<Vector3> ();
					for (int s = 0; s < xCount; s++) {

						float xP = pivotPoint.x + (s * xOffset);

						for (int z = 0; z < zCount; z++) {

							float zP = pivotPoint.z + (z * zOffset);

							Vector3 finalP = new Vector3 (xP, pivotPoint.y, zP);
							pointsInArea.Add (finalP);
						}

					}
					xSize = (int)xOffset - (int)stretcher - 5;
					zSize = (int)zOffset - (int)stretcher - 5;


					for (int o = 0; o < pointsInArea.Count; o++) {
						ySize = Random.Range (buildingsMinHeight, buildingsMaxHeight) + ((int)distanceToMapEdge / 2);
						Vector3 buildingPos1 = new Vector3 (pointsInArea [o].x + (stretcher / 2), transform.localPosition.y, pointsInArea [o].z + (stretcher / 2));
						AddBuilding ("building", buildingPos1, xSize, ySize, zSize);

					}


				} else {

					xSize -= (int)stretcher - 5;
					ySize = Random.Range (buildingsMinHeight, buildingsMaxHeight) + ((int)distanceToMapEdge / 2);
					zSize -= (int)stretcher - 5;
					Vector3 buildingPos2 = new Vector3 (pivotPoint.x + (stretcher / 2), this.transform.position.y, pivotPoint.z + (stretcher / 2));

					AddBuilding ("building", buildingPos2,xSize, ySize, zSize);
				}

			//}
			//yield return wait;

		}

	}

	void UpdateAreasColor (){

		buildingsBottomColor = Camera.main.gameObject.GetComponent<Skybox> ().bc;

		//for (int i = 0; i < areas.Count; i++) {
		for (int i = 0; i < buildingsIndex.Count; i++) {	
			areas [i].GetComponent<MeshRenderer> ().material.color = buildingsBottomColor;//Color.clear;//Color.black;

		}

		for (int b = 0; b < buildingsRemovedAreaIndex.Count; b++) {	
			areas [buildingsRemovedAreaIndex [b]].GetComponent<MeshRenderer> ().material.color = GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor;//buildingsAreaColor;
		}

	}

	private void UpdateBuildingsColor(){


		foreach (GameObject building in buildings) {

			building.GetComponent<MeshRenderer> ().material.SetColor ("_ColorTop", buildingsTopColor);
			building.GetComponent<MeshRenderer> ().material.SetColor ("_ColorMid", buildingsTopColor);
			building.GetComponent<MeshRenderer> ().material.SetColor ("_ColorBot", GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor);//buildingsAreaColor);//buildingsBottomColor);
			building.GetComponent<MeshRenderer> ().material.SetFloat ("_Middle", colorHeight);

			builingsImage.color = buildingsTopColor;
		}
	}

	private void GrowBuilding(){


		if (addOneBuilding) {

			MakeBuilding (buildingsCurrentIndex);

			addOneBuilding = false;
		}

		if (buildings.Count > 0) {

			if (lerpId < 1.0f) {

				lerpId += Time.deltaTime * (1.0f / 10.0f);
			} 
			foreach (GameObject b in buildings) 
			{
				b.transform.localScale = new Vector3 (
					b.transform.localScale.x,
					Mathf.Lerp (b.transform.localScale.y, 1.0f, Mathf.SmoothStep (0.0f, 1.0f, lerpId)),
					b.transform.localScale.z);
			}
		}

	}


////-------------------------------------------------------------------------------------------------------------- map area

	private void addMapEdges()
	{
		mapEdgePoints.Add( new Vector3(0, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, 0)); //center
		mapEdgePoints.Add( new Vector3(mapWidth, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight/2));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight/2));


		//mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight/2)); //center
	}

	Vector3 GetClosestEdge(Vector3 currentPosition, List<Vector3> targets)
	{
		Vector3 bestTarget = new Vector3();
		float closestDistanceSqr = Mathf.Infinity;
		//Vector3 currentPosition = transform.position;

		foreach(Vector3 potentialTarget in targets)
		{
			Vector3 directionToTarget = potentialTarget - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		return bestTarget;
	}

	private void GetDistinctArrayList(List<Vector3> arr, int idx)
	{

		int count = 0;

		if (idx >= arr.Count) return;

		Vector3 val = arr[idx];
		foreach (Vector3 v in arr)
		{
			if (v.Equals(arr[idx]))
			{
				count++;
			}
		}

		if (count > 1)
		{
			arr.Remove(val);
			GetDistinctArrayList(arr, idx);
		}
		else
		{
			idx += 1;
			GetDistinctArrayList(arr, idx);
		}
	}
		
	void GetVectors ( GameObject cube) 
	{
		Vector3[] v = new Vector3[4];

		Vector3 bMin = cube.GetComponent<BoxCollider>().bounds.min;
		Vector3 bMax = cube.GetComponent<BoxCollider>().bounds.max;

		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));

		GetDistinctArrayList (edgePoints, 0);

	}

	void splitX(GameObject splitMe){

		float xSplit =  Random.Range(minSize,splitMe.transform.localScale.x - minSize);
		float split1 = splitMe.transform.localScale.x - xSplit;

		float x1 = splitMe.transform.position.x - ((xSplit - splitMe.transform.localScale.x) / 2);
		float x2 = splitMe.transform.position.x + ((split1 - splitMe.transform.localScale.x) / 2);

		if (xSplit > minSize){

			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (xSplit, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c1.transform.position = new Vector3(x1,splitMe.transform.position.y,splitMe.transform.position.z);
			c1.GetComponent<Renderer> ().material.color = Color.black;//new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground";//"ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (split1, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c2.transform.position = new Vector3(x2,splitMe.transform.position.y,splitMe.transform.position.z);
			c2.GetComponent<Renderer>().material.color = Color.black;//new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground";//"ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);
		}		
	}

	void splitZ(GameObject splitMe){
		
		float zSplit = Random.Range(minSize, splitMe.transform.localScale.z - minSize);
		float zSplit1 = splitMe.transform.localScale.z - zSplit;

		float z1 = splitMe.transform.position.z - ((zSplit - splitMe.transform.localScale.z) / 2);
		float z2 = splitMe.transform.position.z+ ((zSplit1 - splitMe.transform.localScale.z) / 2);


		if (zSplit > minSize){
			
			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit);
			c1.transform.position = new Vector3( splitMe.transform.position.x, splitMe.transform.position.y, z1);
			c1.GetComponent<Renderer>().material.color = Color.black; //new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground";//"ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit1);
			c2.transform.position = new Vector3(splitMe.transform.position.x, splitMe.transform.position.y, z2);
			c2.GetComponent<Renderer>().material.color = Color.black;//new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground";//"ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);

		}
	}

	private GameObject createSpotlights(Vector3 pos ){

		GameObject a = (GameObject) Instantiate(spotlight, pos, Quaternion.identity);
		//a.transform.localScale = new Vector3 (4f, 4f, 4f);
		//a.GetComponent<Renderer> ().material.color = Color.red;
		a.transform.parent = this.transform;

		return a;
	}


	
}
