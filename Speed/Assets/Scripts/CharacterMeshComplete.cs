using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]


public class CharacterMeshComplete : MonoBehaviour {

	private GameObject topParent = null;
	private GameObject bottomParent = null;
	private GameObject rightParent = null;
	private GameObject leftParent = null;
	private GameObject frontParent = null;
	private GameObject backParent = null;
	private GameObject frontStrechParent = null;
	private GameObject backStrechParent = null;

	public GameObject wingsTrailObject = null;
	public GameObject sphere;
	private List <GameObject> newSpheres = new List<GameObject>();

	[HideInInspector] public static int tranformNum = 0;
	[HideInInspector] public int animateCount = 0;//0
	[HideInInspector] public string moveState = "ball";
	[HideInInspector] public GameObject leftTrail;
	[HideInInspector] public GameObject rightTrail;
	public GameObject flame1;
	public GameObject flame2;
	public GameObject flame3;

	private bool disolveLerpState, disolveState, pickNewTexture = false;
	private float disolveLerp = 0.0f;
	private int turnCount = 0;

	private int xSize = 10; 
	private int ySize = 10; 
	private int zSize = 10;
	private int roundness = 5;

	List <int> top = new List<int>(); 
	List <int> bottom = new List<int>(); 

	private bool nextAnim = true;
	private bool prevAnim = false;
	private float ballId, carId, airplaneId, jetId, nasaId, rocketId = 0;
	private int[] left, right, back, front, frontStretch, backStretch = new int[]{};

	private Texture[] textures, stripesTexture;
	Material material;

	//private MeshCollider meshCollider; 
	private SphereCollider sphereCollider; 
	private float colliderCenterY, colliderCenterZ, colliderSizeX, colliderSizeY, colliderSizeZ = 0;
	private BoxCollider boxCollider;
	private Rigidbody meshRigidBody; 
	private MeshFilter meshFilter ;
	private Renderer meshRenderer;
	private Mesh mesh;

	private Vector3[] vertices = new Vector3[]{};
	private Vector3[] normals = new Vector3[]{};
	private Vector2[] uv = new Vector2[]{};
	private int[] triangles = new int[]{}; 
	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}

	private CharacterMovement craftMovement;

	[HideInInspector] public static Color textureSwitchColor = Color.black;
	private int textureIndex1 = 0;
	private int textureIndex2 = 0;

	public static Color[] textureColor = new Color[]{
		Color.green,
		new Color( 0.6f, 0.6f, 0.6f, 1.0f), //black
		Color.blue * 0.5f, //dark blue
		new Color( 0.95f, 0.6f, 0.01f, 1.0f),//orange
		new Color( 1f, 0.0f, 0.2f, 1.0f),//red
		new Color( 0.6f, 0.6f, 0.25f, 1.0f),//dark green
		Color.cyan,
		new Color( 0.8f, 0.6f, 0.6f, 1.0f),//whiter red 
		new Color( 0.4f, 0.2f, 0.6f, 1.0f),//purple 
		Color.gray,//gray 
		Color.green,
		Color.blue * 0.5f,
		new Color( 0.95f, 0.6f, 0.01f, 1.0f),//orange
		new Color( 1f, 0.0f, 0.2f, 1.0f),//red
		new Color( 0.6f, 0.6f, 0.25f, 1.0f)//dark green
	};

	private GameObject createSphere(Vector3 pos , List <GameObject> Arr, int Id){

		GameObject a = new GameObject ();
		a.transform.position = pos;
		//GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.name = "Cube" + Id;
		//a.transform.localScale = Vector3.zero; //new Vector3 (0.4f, 0.4f, 0.4f);
		//a.GetComponent<Renderer> ().material.color = Color.red;
		a.transform.parent = this.transform;
		Arr.Add (a);
		return a;
	}
	GameObject createParent(Vector3 pos, string name)
	{
		GameObject a = new GameObject ();
		a.name = name;
		a.transform.parent = this.transform;
		a.transform.localPosition = pos;
		return a;
	}


	void Awake ()
	{
		this.name = "Craft";

		craftMovement = GetComponent<CharacterMovement> ();


		CreateMesh ();
		AddSpheres ();
		GetSides ();
		SpheresID ();


	}

	void AddSpheres()
	{
		for (int i = 0; i < vertices.Length; i++) 
		{
			createSphere (this.transform.position + vertices[i], newSpheres, i);

		}
	}

	void GetSides()
	{
		//front
		front = new int[] {
			303, 304, 305, 306, 307,
			263, 264, 265, 266, 267,
			223, 224, 225, 226, 227,
			183, 184, 185, 186, 187,
			143, 144, 145, 146, 147
		};

		// back
		back = new int[] {
			283, 284, 285, 286, 287,
			243, 244, 245, 246, 247,
			203, 204, 205, 206, 207,
			163, 164, 165, 166, 167,
			123, 124, 125, 126, 127
		};


		//right
		right = new int[] {
			293, 294, 295, 296, 297,
			253, 254, 255, 256, 257,
			213, 214, 215, 216, 217,
			173, 174, 175, 176, 177,
			133, 134, 135, 136, 137
		};

		//left
		left = new int[] {
			313, 314, 315, 316, 317,
			273, 274, 275, 276, 277,
			233, 234, 235, 236, 237,
			193, 194, 195, 196, 197,
			153, 154, 155, 156, 157
		};


		///top
		for (int t = 360; t < 521; t++) {
			top.Add (t);
		}


		//bottom
		for (int b = 0; b < 80; b++) {
			bottom.Add (b);
		}
		for (int b2 = 521; b2 < 602; b2++) {
			bottom.Add (b2);
		}

		frontStretch = new int[] {
			96, 97, 98, 99, 100,
			101, 102, 103, 104, 105,
			106, 107, 108, 109, 110,
			111, 112, 113, 114, 115,

			138, 139, 140, 141, 142,
			148, 149, 150, 151, 152,

			178, 179, 180, 181, 182,
			188, 189, 190, 191, 192,

			218, 219, 220, 221, 222,
			228, 229, 230, 231, 232,

			258, 259, 260, 261, 262,
			268, 269, 270, 271, 272,

			298, 299, 300, 301, 302,
			308, 309, 310, 311, 312,

			336, 337, 338, 339, 340,
			341, 342, 343, 344, 345,
			346, 347, 348, 349, 350,
			351, 352, 353, 354, 355,

		};


		backStretch = new int[]{ 
		
			80,81,82,83,84,
			85,86,87,88,89,
			90,91,92,93,94,
			95,
			116,117,118,119,120,
			121,122,128,129,130,
			131,132,158,159,160,
			161,162,168,169,170,
			171,172,198,199,200,
			201,202,208,209,210,
			211,212,238,239,240,
			241,242,248,249,250,
			251,252,278,279,280,
			281,282,288,289,290,
			291,292,318,319,
			320,321,322,323,324,
			325,326,327,328,329,
			330,331,332,333,334,
			335,356,357,358,359
		
		};
	}


	void SpheresID ()
	{
		
		topParent = createParent(newSpheres [top [120]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "top");
		bottomParent = createParent(newSpheres [bottom [120]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "bottom");
		leftParent = createParent(newSpheres [left [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "left");
		rightParent = createParent(newSpheres [right [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "right");
		frontParent = createParent(newSpheres [front [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "front");
		backParent = createParent(newSpheres [back [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "back");
		frontStrechParent = createParent(newSpheres [front [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "front Strech");
		backStrechParent = createParent(newSpheres [back [12]].transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2), "back Strech");


		foreach (int fs in frontStretch) {
			newSpheres [fs].transform.parent = frontStrechParent.transform;
		}

		foreach (int bs in backStretch) {
			newSpheres [bs].transform.parent = backStrechParent.transform;
		}
		foreach (int t in top) 
		{
			newSpheres [t].transform.parent = topParent.transform;
		}

		foreach (int b in bottom) 
		{
			newSpheres [b].transform.parent = bottomParent.transform;

		}

		for (int i = 0; i < 25; i++) 
		{
			newSpheres [left [i]].transform.parent = leftParent.transform;
			newSpheres [right [i]].transform.parent = rightParent.transform;
			newSpheres [front [i]].transform.parent = frontParent.transform;
			newSpheres [back [i]].transform.parent = backParent.transform;
		}

		leftTrail = (GameObject) Instantiate(wingsTrailObject, new Vector3(leftParent.transform.position.x + 1.5f,leftParent.transform.position.y,leftParent.transform.position.z), Quaternion.identity);
		leftTrail.transform.parent = leftParent.transform;

		rightTrail = (GameObject) Instantiate(wingsTrailObject, new Vector3(rightParent.transform.position.x - 1.5f,rightParent.transform.position.y,rightParent.transform.position.z), Quaternion.identity);
		rightTrail.transform.parent = rightParent.transform;


	}



	void BallAnimation()
	{
		if (moveState == "ball") {
			GameManager.inTransitionNum = ballId * 100f;

			if (ballId < 1.0f) {

				ballId += Time.deltaTime * (1.0f / 10.0f);

			} else {

				if (nextAnim) 
				{
					print ("gone to 1");
					animateCount = 1;

					nextAnim = false;

				}
				if (prevAnim) 
				{
					print ("gone to 0");

					topParent.transform.localPosition = new Vector3 (0, 5.0f, 0);
					bottomParent.transform.localPosition = new Vector3 (0, -5.0f, 0);

					frontParent.transform.localPosition = new Vector3 (0,0, 5.0f);
					backParent.transform.localPosition = new Vector3 (0, 0, -5.0f);
					leftParent.transform.localPosition = new Vector3 (-5.0f, 0,0);
					rightParent.transform.localPosition = new Vector3 (5.0f, 0,0);
					frontStrechParent.transform.localPosition = new Vector3 (0, 0, 5.0f);
					backStrechParent.transform.localPosition = new Vector3 (0, 0, -5.0f);


					animateCount = 0;
					prevAnim = false;
				}
				moveState = "idle";

			} 

		} else 
		{
			ballId = 0;
		}

	}
	void CarAnimation()
	{
		
		if (moveState == "car") {

			GameManager.inTransitionNum = carId * 100f;

			if (carId < 1.0f) {

				carId += Time.deltaTime * (1.0f / 10.0f);

			} else {
				
				if (nextAnim) 
				{
					print ("gone to 2");
					animateCount = 2;
					nextAnim = false;
				}
				if (prevAnim) 
				{
					print ("gone to 1");
					animateCount = 1;
					prevAnim = false;
				}
				moveState = "idle";

			} 


			if (nextAnim) 
			{
		
				for (int i = 0; i < 25; i++) 
				{
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 3000.0f), 
						-((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 2000.0f), 
						0.0f);

				}

				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 5.0f, carId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -3.0f, carId);

				float frontZ = Mathf.Lerp (frontParent.transform.localPosition.z, 7.0f, carId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -6.0f, carId);

				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 4.5f, carId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -4.5f, carId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 6.0f, carId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -4.60f, carId);

				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZ);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);

			}
			if (prevAnim) {
				
				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 3000.0f), 
						((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 2000.0f), 
						0.0f);
				}
					
				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 5.0f, carId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -5.0f, carId);

				float frontZr = Mathf.Lerp (frontParent.transform.localPosition.z, 5.0f, carId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -5.0f, carId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 5, carId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -5.0f, carId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 5.0f, carId);
				float bachStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -5.0f, carId);


				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZr);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, leftParent.transform.localPosition.z);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, rightParent.transform.localPosition.z);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, bachStretchZr);

			}


		} else {
			carId = 0;
		}


	}
	void PlaneAnimation()
	{
		if (moveState == "airplane") {
			GameManager.inTransitionNum = airplaneId * 100f;

			if (airplaneId < 1.0f) {

				airplaneId += Time.deltaTime * (1.0f / 10.0f);
			} else {

				if (nextAnim) 
				{
					print ("gone to 3");
					animateCount = 3;
					nextAnim = false;

				}
				if (prevAnim) 
				{
					print ("gone to 2");
					animateCount = 2;
					prevAnim = false;
				}
				moveState = "idle";
			} 

			if (nextAnim) 
			{
				
				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 1000.0f), 
						((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 500.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						((newSpheres [back [12]].transform.localPosition.x - newSpheres [back [i]].transform.localPosition.x) / 500.0f), 
						-((newSpheres [back [12]].transform.localPosition.y - newSpheres [back [i]].transform.localPosition.y) / 1000.0f), 
						0.0f);

				}

				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 5.0f, airplaneId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -2.0f, airplaneId);
				float frontY = Mathf.Lerp (frontParent.transform.localPosition.y, 0.5f, airplaneId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -15.0f, airplaneId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -7.0f, airplaneId);
				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, -1.0f, airplaneId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -10.0f, airplaneId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 10.0f, airplaneId);

				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontY, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);
			
			}
			if (prevAnim) {
				
				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 1000.0f), 
						-((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 500.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						-((newSpheres [back [12]].transform.localPosition.x - newSpheres [back [i]].transform.localPosition.x) / 500.0f), 
						((newSpheres [back [12]].transform.localPosition.y - newSpheres [back [i]].transform.localPosition.y) / 1000.0f), 
						0.0f);

				}


				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 5.0f, airplaneId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -3.0f, airplaneId);

				float frontYr = Mathf.Lerp (frontParent.transform.localPosition.y, 0f, airplaneId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -6.0f, airplaneId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -4.60f, airplaneId);
				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, 0.0f, airplaneId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -4.5f, airplaneId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 4.5f, airplaneId);



				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontYr, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);

			}
		} else {

			airplaneId = 0;
		}

	}

	void JetAnimation()
	{
		if (moveState == "jet") {

			GameManager.inTransitionNum = jetId * 100f;

			if (jetId < 1.0f) {

				jetId += Time.deltaTime * (1.0f / 10.0f);
			} else {
				
				if (nextAnim) {
					print ("gone to 4");
					animateCount = 4;
					nextAnim = false;
				}
				if (prevAnim) {
					print ("gone to 3");
					animateCount = 3;
					prevAnim = false;
				}
				moveState = "idle";

			}


			if (nextAnim) {
				
				for (int i = 0; i < 25; i++) {
					newSpheres [left [i]].transform.Translate (
						((newSpheres [left [12]].transform.localPosition.x - newSpheres [left [i]].transform.localPosition.x) / 300.0f), 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [left [i]].transform.localPosition.y) / 300.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						((newSpheres [right [12]].transform.localPosition.x - newSpheres [right [i]].transform.localPosition.x) / 300.0f), 
						((newSpheres [right [12]].transform.localPosition.y - newSpheres [right [i]].transform.localPosition.y) / 300.0f), 
						0.0f);

					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 900.0f), 
						((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 400.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						-((newSpheres [back [12]].transform.localPosition.x - newSpheres [back [i]].transform.localPosition.x) / 1000.0f), 
						((newSpheres [back [12]].transform.localPosition.y - newSpheres [back [i]].transform.localPosition.y) / 300.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [bs].transform.localPosition.y) / 500.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [fs].transform.localPosition.y) / 500.0f), 
						0.0f);
				}

				//done
				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 3.5f, jetId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -1.8f, jetId);

				float frontY = Mathf.Lerp (frontParent.transform.localPosition.y, 0.0f, jetId);
				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, -4.0f, jetId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -7.0f, jetId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 7.0f, jetId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -10.0f, jetId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -6.0f, jetId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 7.0f, jetId);

				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);


				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontY, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);
			}
			if (prevAnim) {

				for (int i = 0; i < 25; i++) {
					newSpheres [left [i]].transform.Translate (
						-((newSpheres [left [12]].transform.localPosition.x - newSpheres [left [i]].transform.localPosition.x) / 300.0f), 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [left [i]].transform.localPosition.y) / 300.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						-((newSpheres [right [12]].transform.localPosition.x - newSpheres [right [i]].transform.localPosition.x) / 300.0f), 
						-((newSpheres [right [12]].transform.localPosition.y - newSpheres [right [i]].transform.localPosition.y) / 300.0f), 
						0.0f);

					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 900.0f), 
						-((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 400.0f), 
						0.0f);

					newSpheres [back [i]].transform.Translate (
						((newSpheres [back [12]].transform.localPosition.x - newSpheres [back [i]].transform.localPosition.x) / 1000.0f), 
						-((newSpheres [back [12]].transform.localPosition.y - newSpheres [back [i]].transform.localPosition.y) / 300.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [bs].transform.localPosition.y) / 500.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [fs].transform.localPosition.y) / 500.0f), 
						0.0f);
				}

				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 5.0f, jetId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -2.0f, jetId);

				float frontYr = Mathf.Lerp (frontParent.transform.localPosition.y, 0.5f, jetId);
				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, -1.0f, jetId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -10.0f, jetId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 10.0f, jetId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -15.0f, jetId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -7.0f, jetId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 6.0f, jetId);


				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontYr, frontParent.transform.localPosition.z);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);
			}



		} else 
		{

			jetId = 0;
		}
	}

	void NasaPlaneAnimation()
	{


		if (moveState == "nasa") {

			GameManager.inTransitionNum = nasaId * 100f;

			if (nasaId < 1.0f) {

				nasaId += Time.deltaTime * (1.0f / 10.0f);
			} else {

				if (nextAnim) {
					print ("gone to 5");
					animateCount = 5;
					nextAnim = false;
				}
				if (prevAnim) {
					print ("gone to 4");
					animateCount = 4;
					prevAnim = false;
				}
				moveState = "idle";
			} 

			if (nextAnim) {
				
				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 500.0f), 
						((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 400.0f), 
						((newSpheres [front [12]].transform.localPosition.z - newSpheres [front [i]].transform.localPosition.z) / 200.0f));
				
					newSpheres [left [i]].transform.Translate (
						((newSpheres [left [12]].transform.localPosition.x - newSpheres [left [i]].transform.localPosition.x) / 400.0f), 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [left [i]].transform.localPosition.y) / 400.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						((newSpheres [right [12]].transform.localPosition.x - newSpheres [right [i]].transform.localPosition.x) / 400.0f), 
						((newSpheres [right [12]].transform.localPosition.y - newSpheres [right [i]].transform.localPosition.y) / 400.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [bs].transform.localPosition.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						((newSpheres [left [12]].transform.localPosition.y - newSpheres [fs].transform.localPosition.y) / 1000.0f), 
						0.0f);
				}


				//done
				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 3f, nasaId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -1.8f, nasaId);

				float frontZ = Mathf.Lerp (frontParent.transform.localPosition.z, 6.0f, nasaId);
				float LeftRightZ = Mathf.Lerp (leftParent.transform.localPosition.z, -2.0f, nasaId);
				float leftX = Mathf.Lerp (leftParent.transform.localPosition.x, -10.0f, nasaId);
				float rightX = Mathf.Lerp (rightParent.transform.localPosition.x, 10.0f, nasaId);
				float backZ = Mathf.Lerp (backParent.transform.localPosition.z, -5.5f, nasaId);
				float backStretchZ = Mathf.Lerp (backStrechParent.transform.localPosition.z, -5.5f, nasaId);
				float frontStretchZ = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 6.0f, nasaId);

				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZ);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZ);
				leftParent.transform.localPosition = new Vector3 (leftX, leftParent.transform.localPosition.y, LeftRightZ);
				rightParent.transform.localPosition = new Vector3 (rightX, rightParent.transform.localPosition.y, LeftRightZ);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZ);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZ);

			}

			if (prevAnim) {

				for (int i = 0; i < 25; i++) {
					newSpheres [front [i]].transform.Translate (
						-((newSpheres [front [12]].transform.localPosition.x - newSpheres [front [i]].transform.localPosition.x) / 500.0f), 
						-((newSpheres [front [12]].transform.localPosition.y - newSpheres [front [i]].transform.localPosition.y) / 400.0f), 
						-((newSpheres [front [12]].transform.localPosition.z - newSpheres [front [i]].transform.localPosition.z) / 200.0f));

					newSpheres [left [i]].transform.Translate (
						-((newSpheres [left [12]].transform.localPosition.x - newSpheres [left [i]].transform.localPosition.x) / 400.0f), 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [left [i]].transform.localPosition.y) / 400.0f), 
						0.00f);

					newSpheres [right [i]].transform.Translate (
						-((newSpheres [right [12]].transform.localPosition.x - newSpheres [right [i]].transform.localPosition.x) / 400.0f), 
						-((newSpheres [right [12]].transform.localPosition.y - newSpheres [right [i]].transform.localPosition.y) / 400.0f), 
						0.0f);
				}

				//stretch y
				foreach (int bs in backStretch) {
					newSpheres [bs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [bs].transform.localPosition.y) / 1000.0f), 
						0.0f);
				}
				foreach (int fs in frontStretch) {
					newSpheres [fs].transform.Translate (
						0.0f, 
						-((newSpheres [left [12]].transform.localPosition.y - newSpheres [fs].transform.localPosition.y) / 1000.0f), 
						0.0f);
				}


				float topY =  Mathf.Lerp (topParent.transform.localPosition.y, 3.5f, nasaId);
				float bottomY =  Mathf.Lerp (bottomParent.transform.localPosition.y, -1.8f, nasaId);

				float frontZr = Mathf.Lerp (frontParent.transform.localPosition.z, 7.0f, nasaId);
				float LeftRightZr = Mathf.Lerp (leftParent.transform.localPosition.z, -4.0f, nasaId);
				float leftXr = Mathf.Lerp (leftParent.transform.localPosition.x, -7.0f, nasaId);
				float rightXr = Mathf.Lerp (rightParent.transform.localPosition.x, 7.0f, nasaId);
				float backZr = Mathf.Lerp (backParent.transform.localPosition.z, -10.0f, nasaId);
				float backStretchZr = Mathf.Lerp (backStrechParent.transform.localPosition.z, -6.0f, nasaId);
				float frontStretchZr = Mathf.Lerp (frontStrechParent.transform.localPosition.z, 7.0f, nasaId);



				topParent.transform.localPosition = new Vector3 (topParent.transform.localPosition.x, topY, topParent.transform.localPosition.z);
				bottomParent.transform.localPosition = new Vector3 (bottomParent.transform.localPosition.x, bottomY, bottomParent.transform.localPosition.z);

				frontParent.transform.localPosition = new Vector3 (frontParent.transform.localPosition.x, frontParent.transform.localPosition.y, frontZr);
				backParent.transform.localPosition = new Vector3 (backParent.transform.localPosition.x, backParent.transform.localPosition.y, backZr);
				leftParent.transform.localPosition = new Vector3 (leftXr, leftParent.transform.localPosition.y, LeftRightZr);
				rightParent.transform.localPosition = new Vector3 (rightXr, rightParent.transform.localPosition.y, LeftRightZr);
				frontStrechParent.transform.localPosition = new Vector3 (frontStrechParent.transform.localPosition.x, frontStrechParent.transform.localPosition.y, frontStretchZr);
				backStrechParent.transform.localPosition = new Vector3 (backStrechParent.transform.localPosition.x, backStrechParent.transform.localPosition.y, backStretchZr);



			}
		} else {
			nasaId = 0;
		}


	}


	void Update()
	{
		this.transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one * 0.4f, Time.deltaTime / 10.0f);


		UpdateMesh ();


		BallAnimation ();
		CarAnimation ();
		PlaneAnimation ();
		JetAnimation ();
		NasaPlaneAnimation ();

		if ((Input.GetKeyDown ("2") || Input.GetButton ("PS4_R1")) && moveState == "idle") //&& tranformNum > 0 && GameManager.radarIcon == 0) 
		{
				prevAnim = false;
				tranformNum --;

				if (animateCount == 0 && nextAnim == false) {
					moveState = "ball"; 
					nextAnim = true;
					disolveState = true;
				}
				if (animateCount == 1 && nextAnim == false) {
					moveState = "car"; 
					nextAnim = true;
					disolveState = true;
				}
				if (animateCount == 2 && nextAnim == false) {
					moveState = "airplane"; 
					nextAnim = true;
					disolveState = true;
				}
				if (animateCount == 3 && nextAnim == false) {
					moveState = "jet"; 
					nextAnim = true;
					disolveState = true;
				}
				if (animateCount == 4 && nextAnim == false) {
					moveState = "nasa"; 
					nextAnim = true;
					disolveState = true;
				}

				//print("moveState:  "+ moveState+"   count: "+animateCount +" prev: "+prevAnim+"  next: "+nextAnim);
		}

		if ((Input.GetKeyDown ("1") || Input.GetButton ("PS4_L1")) && moveState == "idle")// && tranformNum > 0 && GameManager.radarIcon == 0) 
		{
				nextAnim = false;
				tranformNum --;

				if (animateCount == 5 && prevAnim == false) {
					moveState = "nasa"; 
					prevAnim = true;
					disolveState = true;
				}
				if (animateCount == 4 && prevAnim == false) {
					moveState = "jet"; 
					prevAnim = true;
					disolveState = true;
				}
				if (animateCount == 3 && prevAnim == false) {
					moveState = "airplane"; 
					prevAnim = true;
					disolveState = true;
				}
				if (animateCount == 2 && prevAnim == false) {
					moveState = "car"; 
					prevAnim = true;
					disolveState = true;
				}
				if (animateCount == 1 && prevAnim == false) {
					moveState = "ball"; 
					prevAnim = true;
					disolveState = true;
				}
				
				//print("moveState:  "+ moveState+"   count: "+animateCount +" prev: "+prevAnim+"  next: "+nextAnim);
			}
			
	}

	void ClearAll(){
		
		meshFilter.sharedMesh = null;
		//meshCollider.sharedMesh = null;
		sphereCollider =  null;
		meshRigidBody = null;
		vertices = null;
		triangles = null; 
		normals = null;
		uv = null;

	}


	void UpdateVertices()
	{
		
		for (int i = 0; i < 25; i++) 
		{
			vertices [left [i]] = newSpheres [left [i]].transform.localPosition + leftParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
			vertices [right [i]] = newSpheres [right [i]].transform.localPosition + rightParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
			vertices [front [i]] = newSpheres [front [i]].transform.localPosition + frontParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
			vertices [back [i]] = newSpheres [back [i]].transform.localPosition + backParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);

		}

		foreach (int fs in frontStretch) {
			vertices [fs] = newSpheres [fs].transform.localPosition + frontStrechParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
		}
		foreach (int bs in backStretch) {
			vertices [bs] = newSpheres [bs].transform.localPosition + backStrechParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
		}
	
		for (int j = 0; j < 161; j++) 
		{

			vertices [top [j]] = newSpheres [top [j]].transform.localPosition + topParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
			vertices [bottom [j]] = newSpheres [bottom [j]].transform.localPosition + bottomParent.transform.localPosition - new Vector3(xSize/2, ySize/2,zSize/2);
		}

	}



	private void CreateMesh()
	{

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.name = "dynamic mesh";
		mesh.Clear();

		CreateVertices();
		CreateTriangles();
	
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);
		mesh.RecalculateBounds();
		mesh.Optimize();


		CreateTexture ();
		CreateCollider();
		CreateRigidBody ();



	}

	private void UpdateMesh()
	{

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.name = "dynamic mesh";
		mesh.Clear();



		CreateVertices ();
		UpdateVertices ();
		CreateTriangles();

		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);
		mesh.RecalculateBounds();
		mesh.Optimize();

		UpdateCollider ();
		UpdateTexture ();
		UpdateTrialAndParticles ();

	}
	private void CreateVertices() {


		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;

		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
		normals = new Vector3[vertices.Length];
		uv = new Vector2[vertices.Length];


		int v = 0;
		// sides
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= zSize; z++) {
				SetVertex(v++, xSize , y, z );
			}
			for (int x = xSize - 1; x >= 0; x--) {
				SetVertex(v++, x, y, zSize);
			}
				
			for (int z = zSize - 1; z > 0; z--) {
				SetVertex(v++, 0, y, z);
			}
		}



		// top 
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x , ySize , z );
			}
		}
		//bottom
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x , 0 , z);
			}
		}


	}
	private void SetVertex (int i, int x, int y, int z) {
		Vector3 inner = vertices[i] = new Vector3(x, y , z );

		////sides
		if (x  < roundness) {
			inner.x = roundness;
		}
		else if (x  > xSize - roundness) {
			inner.x = xSize - roundness;
		}

		////top and bottom
		if (y  < roundness) {

			// bottom rounder
			inner.y = roundness;
		}
		else if (y > ySize - roundness) 
		{
			// top rounder
			inner.y = ySize - roundness ;
		}

		////front and back
		if (z < roundness) {

			//front rounder
			inner.z = roundness;
		}
		else if (z > zSize - roundness) {
			//back rounder
			inner.z = zSize - roundness;
		}

		normals[i] = (vertices[i] - inner).normalized;
		vertices[i] = inner + normals[i] * roundness;
		uv[i] = new Vector2((float)x / (xSize * 4), (float)y / (ySize * 4));
	}


	private void CreateTriangles () {
		
		int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		triangles = new int[quads * 6];
		int ring = (xSize + zSize) * 2;
		int t = 0, v = 0;

		for (int y = 0; y < ySize; y++, v++) {
			for (int q = 0; q < ring - 1; q++, v++) {
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
			}
			t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
		}

		t = CreateTopFace(triangles, t, ring);
		t = CreateBottomFace(triangles, t, ring);

	
	}

	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}


	private void CreateCollider(){

		Destroy(sphereCollider);
		sphereCollider = gameObject.AddComponent (typeof(SphereCollider)) as SphereCollider;
		sphereCollider.center = Vector3.zero;


		Destroy(boxCollider);
		boxCollider = gameObject.AddComponent (typeof(BoxCollider)) as BoxCollider;

		colliderCenterY = 0;
		colliderCenterZ = 0;

		colliderSizeX = xSize;
		colliderSizeY = ySize;
		colliderSizeZ = zSize;

		boxCollider.center = new Vector3( 0, colliderCenterY, colliderCenterZ);
		boxCollider.size = new Vector3(colliderSizeX, colliderSizeY , colliderSizeZ );
	}
	private void UpdateCollider()
	{
		
		if (craftMovement.ballState) {

			sphereCollider.enabled = true;
			boxCollider.enabled = false;

		} else {

			sphereCollider.enabled = false;
			boxCollider.enabled = true;

			float lerping = 1f * Time.deltaTime;

			if ((moveState == "ball" && animateCount == 0) || (moveState == "car" && animateCount == 2)) { 
			
				colliderCenterY = Mathf.Lerp (colliderCenterY, 0, lerping);
				colliderCenterZ = Mathf.Lerp (colliderCenterZ, 0, lerping);

				colliderSizeX = Mathf.Lerp (colliderSizeX, xSize, lerping);
				colliderSizeY = Mathf.Lerp (colliderSizeY, ySize, lerping);
				colliderSizeZ = Mathf.Lerp (colliderSizeZ, zSize, lerping);

			} else if ((moveState == "car" && animateCount == 1) || (moveState == "airplane" && animateCount == 3)) {
				colliderCenterY = Mathf.Lerp (colliderCenterY, 1, lerping);
				colliderCenterZ = Mathf.Lerp (colliderCenterZ, 1, lerping);

				colliderSizeX = Mathf.Lerp (colliderSizeX, 8, lerping);
				colliderSizeY = Mathf.Lerp (colliderSizeY, 8, lerping);
				colliderSizeZ = Mathf.Lerp (colliderSizeZ, 14, lerping);
			} else if ((moveState == "airplane" && animateCount == 2) || (moveState == "jet" && animateCount == 4)) {

				colliderCenterZ = Mathf.Lerp (colliderCenterZ, -4, lerping);

				colliderSizeX = Mathf.Lerp (colliderSizeX, 20, lerping);
				colliderSizeY = Mathf.Lerp (colliderSizeY, 8, lerping);
				colliderSizeZ = Mathf.Lerp (colliderSizeZ, 22, lerping);
		
			} else if ((moveState == "jet" && animateCount == 3) || (moveState == "nasa" && animateCount == 5)) {
				colliderCenterY = Mathf.Lerp (colliderCenterY, 1, lerping);
				colliderCenterZ = Mathf.Lerp (colliderCenterZ, -1.5f, lerping);

				colliderSizeX = Mathf.Lerp (colliderSizeX, 14, lerping);
				colliderSizeY = Mathf.Lerp (colliderSizeY, 5, lerping);
				colliderSizeZ = Mathf.Lerp (colliderSizeZ, 16, lerping);


			} else if (moveState == "nasa" && animateCount == 4) {
				colliderCenterY = Mathf.Lerp (colliderCenterY, 0.5f, lerping);
				colliderCenterZ = Mathf.Lerp (colliderCenterZ, 0.5f, lerping);

				colliderSizeX = Mathf.Lerp (colliderSizeX, 20, lerping);
				colliderSizeY = Mathf.Lerp (colliderSizeY, 4, lerping);
				colliderSizeZ = Mathf.Lerp (colliderSizeZ, 11, lerping);
			
			} 

			boxCollider.center = new Vector3 (0, colliderCenterY, colliderCenterZ);
			boxCollider.size = new Vector3 (colliderSizeX, colliderSizeY, colliderSizeZ);
		}
	}

	void UpdateTrialAndParticles(){

		if (craftMovement.ballState) {
			rightTrail.SetActive(false);
			leftTrail.SetActive(false);
			flame1.SetActive (false);
			flame2.SetActive(false);
			flame3.SetActive(false);
			//flame1.GetComponent<ParticleSystem>().

		} else if (craftMovement.groundState) {
			rightTrail.SetActive(false);
			leftTrail.SetActive(false);
			flame1.SetActive (false);
			flame2.SetActive(false);
			flame3.SetActive(false);

		} else if (craftMovement.airSate) {

			rightTrail.SetActive(true);
			leftTrail.SetActive(true);

			if ((moveState == "airplane" && animateCount == 2) || (moveState == "jet" && animateCount == 4)) {
				flame1.SetActive (true);
				flame2.SetActive (false);
				flame3.SetActive (false);
			} else {
				flame1.SetActive (true);
				flame2.SetActive (true);
				flame3.SetActive (true);
			}
				

		}

	}

	private void CreateRigidBody(){
		meshRigidBody = GetComponent (typeof(Rigidbody)) as Rigidbody;
		//meshRigidBody.isKinematic = true;
		//meshRigidBody.useGravity = true;
	}
		

	private void UpdateTexture(){


		float duration = 6.0f;

		if (disolveState) {

			if (disolveLerpState) {
				disolveLerp += Time.deltaTime * (1.0f / duration);
			} else {
				disolveLerp -= Time.deltaTime * (1.0f / duration);
			}

			if (disolveLerp > 1.0f) {
				textureIndex1 = Random.Range (0, stripesTexture.Length - 1);
				material.SetTexture ("_MainTex", stripesTexture [textureIndex1]);

				turnCount += 1;
				disolveLerpState = false;
			} else if (disolveLerp < 0.0f) 
			{
				textureIndex2 = Random.Range (0, stripesTexture.Length - 1);

				material.SetTexture ("_Texture2", stripesTexture [textureIndex2]);


				disolveLerpState = true;
				turnCount += 1;

			}

			if (turnCount >= 2) {
				disolveState = false;
			}

			//print ("turn count:  " + turnCount + "   count: " + disolveLerp);



		} else {

			//pickNewTexture = true;
			turnCount = 0;
			//disolveLerp = 0.0f;
		}


//		material.SetFloat ("_DissolveAmount", disolveLerp);
		material.SetFloat ("_Blend", disolveLerp);
		meshRenderer.material = material;


	}


	private void CreateTexture() {

		meshRenderer = GetComponent<MeshRenderer>();
		textures = new Texture[] {

			Resources.Load ("Burnout") as Texture,
			Resources.Load ("Magma") as Texture,
			Resources.Load ("Noise") as Texture,
			Resources.Load ("Oilrush") as Texture,
			Resources.Load ("Turbulance") as Texture,
			Resources.Load ("Water") as Texture
		};


		stripesTexture = new Texture[] {
			Resources.Load ("TextureStripe2") as Texture,
			Resources.Load ("TextureStripe7") as Texture,
			Resources.Load ("TextureStripe8") as Texture,
			Resources.Load ("TextureStripe9") as Texture,

			Resources.Load ("TextureStripe1") as Texture,
			Resources.Load ("TextureStripe3") as Texture,
			Resources.Load ("TextureStripe4") as Texture,
			Resources.Load ("TextureStripe5") as Texture,
			Resources.Load ("TextureStripe6") as Texture,

			Resources.Load ("TextureStripe10") as Texture,
			Resources.Load ("TextureStripe11") as Texture,
			Resources.Load ("TextureStripe33") as Texture,
			Resources.Load ("TextureStripe44") as Texture,
			Resources.Load ("TextureStripe55") as Texture,
			Resources.Load ("TextureStripe66") as Texture
		};
		int textureIndex = (int)Mathf.Floor (Random.value * textures.Length);
		int textureIndex2 = (int)Mathf.Floor (Random.value * textures.Length);


		material = new Material (Shader.Find (".ShaderExample/CrossFadeGradient"));
		material.SetTexture ("_MainTex", stripesTexture [textureIndex]);
		material.SetTextureScale("_MainTex", new Vector2(1,  Random.Range( 1 , 5) ));
		material.SetTextureOffset ("_MainTex", new Vector2(0.5f, (Random.Range(1,4)/2) + (Random.Range(1,4)/2) + (0.25f)));
		material.SetTexture ("_Texture2", stripesTexture [textureIndex2]);
		material.SetTextureScale("_Texture2", new Vector2(1,  Random.Range( 1 , 5) ));
		material.SetTextureOffset ("_Texture2", new Vector2(0.5f, (Random.Range(1,4)/2) + (Random.Range(1,4)/2) + (0.25f)));

		material.SetFloat ("_Blend", 0.0f);



//		material = new Material (Shader.Find (".ShaderExample/DissolveToColor"));
//		material.SetTexture ("_MainTex", stripesTexture [textureIndex]);
//		material.SetTextureScale("_MainTex", new Vector2(1,  Random.Range( (int)1 , (int)10) ));
//		material.SetTextureOffset ("_MainTex", new Vector2(0.5f, Random.Range(1,8) + (Random.Range(1,4)/2) + (Random.Range(1,4)/2) + (0.25f)));
//
//		material.SetTexture ("_DissolveMap", textures [Random.Range (0, textures.Length - 1)]);
//		material.SetFloat ("_DissolveVal", 1.2f);
//		material.SetFloat ("_LineWidth", 0.2f);
//		material.SetColor ("_LineColor", ExtensionMethods.RandomColor());
//		material.SetColor ("_DissolveColor", ExtensionMethods.RandomColor());

//		material = new Material (Shader.Find (".ShaderExample/DissolveToTransparent"));
//		material.SetTexture ("_MainTex", stripesTexture [textureIndex]);
//		material.SetTextureScale("_MainTex", new Vector2(1,  Random.Range( 1 , 5) ));
//		material.SetTextureOffset ("_MainTex", new Vector2(0.5f, (Random.Range(1,4)/2) + (Random.Range(1,4)/2) + (0.25f)));
//		material.SetTexture ("_DissolveTex", textures [Random.Range (0, textures.Length - 1)]);
//		material.SetFloat ("_DissolveAmount", Random.Range (0, 0.0f));
		//material.SetFloat ("_BurnSize", Random.Range (0, 1.0f));
		meshRenderer.material = material;


	}




}
