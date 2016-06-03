using UnityEngine;
using System.Collections;

public class BasePyramid : MonoBehaviour {

	private int mapWidth = 900;
	private int mapHeight = 900;

	private GameObject pyramid = null;
	private MeshFilter pyramidMeshFilter = new MeshFilter();
	private MeshRenderer pyramidRenderer = new MeshRenderer();
	private Color pyramidColor = new Color();
	//private Color pyramidTargetColor = new Color();

	private Vector3 pyramidV4 = new Vector3 (); 
	private Vector3 pyramidV5 = new Vector3 ();
	private Vector3 pyramidV6 = new Vector3 (); 
	private Vector3 pyramidV7 = new Vector3 ();

	private float pyramidHeight = 50.0f;
	private float morphCount = 10.0f;

	private void Awake () 
	{
		drawPyramid ();

	}

	void Update () 
	{

		UpdatePyramid ();
	}

	private void drawPyramid()
	{
		mapWidth = this.GetComponent<GenerateCity> ().mapWidth;
		mapHeight = this.GetComponent<GenerateCity> ().mapHeight;

		pyramid = new GameObject("Pyramid");
		pyramid.transform.parent = this.transform;
		pyramid.transform.position = new Vector3 (this.transform.position.x+(mapWidth/2), this.transform.position.y , this.transform.position.z+(mapHeight/2));
		pyramid.transform.eulerAngles = new Vector3( 0, 0, 180f);

		pyramidMeshFilter = pyramid.AddComponent<MeshFilter>();
		pyramidRenderer = pyramid.AddComponent<MeshRenderer> ();
		pyramidColor = Camera.main.GetComponent<Skybox> ().bottomColor;


	}

	void UpdatePyramid()
	{

		if (morphCount > 0.0f) {
			morphCount -= Time.deltaTime;
		} else {
			morphCount = Random.Range (5.0f, 12.0f);
			pyramidHeight = Random.Range (50.0f, mapHeight/2f);

		}


		if (pyramidMeshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		Mesh mesh = pyramidMeshFilter.sharedMesh;
		if (mesh == null){
			pyramidMeshFilter.mesh = new Mesh();
			mesh = pyramidMeshFilter.sharedMesh;
		}

		mesh.Clear();

		Vector3 pyramidV0 = new Vector3 (-mapWidth/2, 0, mapWidth/2 ); 
		Vector3 pyramidV1 = new Vector3 (mapWidth/2, 0, mapWidth/2 ); 
		Vector3 pyramidV2 = new Vector3 (mapWidth/2 , 0, -mapWidth/2); 
		Vector3 pyramidV3 = new Vector3 (-mapWidth/2 , 0 , -mapWidth/2);
		// morph to pyramid
		pyramidV4 = Vector3.Lerp (pyramidV4, new Vector3 (0, pyramidHeight, 0 ),Time.deltaTime); 
		pyramidV5 = Vector3.Lerp (pyramidV5, new Vector3 (0, pyramidHeight, 0 ), Time.deltaTime);
		pyramidV6 = Vector3.Lerp (pyramidV6, new Vector3 (0, pyramidHeight, 0), Time.deltaTime); 			
		pyramidV7 = Vector3.Lerp (pyramidV7, new Vector3 (0, pyramidHeight, 0), Time.deltaTime);


		mesh.vertices = new Vector3[]{

			// Front face 
			pyramidV4, pyramidV5, pyramidV0, pyramidV1,

			// Back face 
			pyramidV6, pyramidV7, pyramidV2, pyramidV3,

			// Left face 
			pyramidV7, pyramidV4, pyramidV3, pyramidV0,

			// Right face
			pyramidV5, pyramidV6, pyramidV1, pyramidV2,

			// Top face 
			pyramidV7, pyramidV6, pyramidV4, pyramidV5,

			// Bottom face 
			pyramidV0, pyramidV1, pyramidV3, pyramidV2


		};

		//Add Triangles region 
		//these are three point, and work clockwise to determine what side is visible
		mesh.triangles = new int[]{


			//front face
			0,2,3, // first triangle
			3,1,0, // second triangle

			//back face
			4,6,7, // first triangle
			7,5,4, // second triangle

			//left face
			8,10,11, // first triangle
			11,9,8, // second triangle

			//right face
			12,14,15, // first triangle
			15,13,12, // second triangle

			//top face
			16,18,19, // first triangle
			19,17,16, // second triangle

			//bottom face
			20,22,23, // first triangle
			23,21,20, // second triangle

		};

		//Add Normales region
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		Vector3 up 		= Vector3.up;
		Vector3 down 	= Vector3.down;

		mesh.normals = new Vector3[]
		{
			// Front face
			front, front, front, front,

			// Back face
			back, back, back, back,

			// Left face
			left, left, left, left,

			// Right face
			right, right, right, right,

			// Top face
			up, up, up, up,

			// Bottom face
			down, down, down, down

		};
		//end Normales region

		//Add UVs region 
		Vector2 u00 = new Vector2( 0f, 0f );
		Vector2 u10 = new Vector2( 1f, 0f );
		Vector2 u01 = new Vector2( 0f, 1f );
		Vector2 u11 = new Vector2( 1f, 1f );

		Vector2[] uvs = new Vector2[]
		{
			// Front face uv
			u01, u00, u11, u10,

			// Back face uv
			u01, u00, u11, u10,

			// Left face uv
			u01, u00, u11, u10,

			// Right face uv
			u01, u00, u11, u10,

			// Top face uv
			u01, u00, u11, u10,

			// Bottom face uv
			u01, u00, u11, u10
		};
		//End UVs region


		Material material = new Material (Shader.Find ("Standard"));
		//pyramidColor = Color.Lerp(pyramidColor, pyramidTargetColor, Time.deltaTime);
		material.color = pyramidColor;

		pyramidRenderer.material = material;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}



}
