using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	[Range(0, 16)] public int textureIndex = 2;
	private float stretcher = 10f;
	private float buildingsExtrusion = 8f;

	private static int xSize, ySize, zSize ;
	private int roundness = 6;
	private bool roundTop = false;
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


	public GameObject CreateBuilding(Vector3 position, int x, int y, int z)
	{
		xSize = x;
		ySize = y;
		zSize = z;

		roundTop = (Random.Range (0, 2) == 0);


		verticesCopy.Clear();
		controlPoints.Clear();
		listOfVerticesIndexes.Clear();

		pivotControlPoint.Clear();
		topControlPointIndexes.Clear (); 
		frontControlPointIndexes.Clear();
		backControlPointIndexes.Clear (); 
		sidesControlPointIndexes.Clear();

		vertices.Clear();
		normals.Clear();
		uv.Clear();

		triangles = null;


		GameObject build = new GameObject ();

		MeshFilter meshF = build.AddComponent< MeshFilter >();
		Mesh m = meshF.mesh;
		if (meshF == null){
			Debug.LogError("MeshFilter not found!");
			return null;
		}

		m = meshF.sharedMesh;
		if (m == null){
			meshF.mesh = new Mesh();
			m = meshF.sharedMesh;
		}
		m.name = "building mesh";

		m.Clear();


		CreateVertices();
		GetIndexes ();
		RandomOutlinesGeneration ();
		CreateTriangles();

		m.vertices = vertices.ToArray();
		m.triangles = triangles;

		m.normals = normals.ToArray();
		m.uv = uv.ToArray();

		m.RecalculateNormals();
		m.RecalculateBounds();
		m.Optimize();

		MeshCollider meshC = build.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshC.sharedMesh = m; // Give it your mesh here.

		MeshRenderer meshR = build.AddComponent<MeshRenderer> ();

		CreateColorAndtexture (meshR);

		build.transform.position = position;

		return build;
	}

	private void GetIndexes (){

		int xlength = xSize + 1;
		int ylength = ySize + 1;
		int zlength = zSize + 1;

		int zExtra = zSize - 2;
		int offset = ((xlength * 2 ) + (zSize - 1 + zExtra)) ;

		//Debug.Log (" offset " + offset);

		for (int x = 0; x < offset + 1; x++) {

			for (int z = 0; z < zlength; z++)
			{
				List<int> innerArray = new List<int>();

				for (int y = 0; y < ylength; y++)
				{
					int myPos = (((offset * y) + x) + y);
					innerArray.Add (myPos);
					//print(innerArray[y]);
				}
				controlPoints.Insert(x, innerArray.ToArray());
			}

		}



		int p = 0;
		for (int v = 0; v < vertices.Count; v++) {

			verticesCopy.Add( vertices [v] );

			listOfVerticesIndexes.Add (p);
			p++;

			//top vertices
			if (vertices [v].y == ySize) {
				topControlPointIndexes.Add (v);
			}

		}
		//Debug.Log ("vertices length: "+vertices.Count +"   list of indexes length: "+ listOfVerticesIndexes.Count+"  vertex points copied: "+verticesCopy.Count);

		int midY = (int)(Mathf.Round (ySize / 2));
		int pivot = 0;

		for (int vi = 0; vi < listOfVerticesIndexes.Count; vi++) {

			//print (listOfIndexes.ToArray());
			for (int o = 0; o < offset + 1; o++) {

				if (  listOfVerticesIndexes[vi].Equals(controlPoints [o] [midY])   ) {

					pivotControlPoint.Add (pivot);
					//print (listOfIndexes[s]+"   "+newSpheres.Count);
					pivot++;
				} 
			}
		}

		////front
		for (int f = 0; f < xlength; f++) {

			frontControlPointIndexes.Add (f);
		}

		////back
		for (int b = xSize + zSize; b < pivotControlPoint.Count - (zSize - 1); b++) {

			backControlPointIndexes.Add (b);
		}


		//first side
		for (int sd = 0; sd < zSize - 1; sd++) {

			sidesControlPointIndexes.Add (sd + xlength);
		}

		//second side
		for (int sd2 = (xlength * 2) + (zSize - 1); sd2 < pivotControlPoint.Count; sd2++) {

			sidesControlPointIndexes.Add (sd2);
		}

	}

	private void RandomOutlinesGeneration()
	{

		////front calcutations
		int fFromLoop = Random.Range (0, frontControlPointIndexes.Count - 1);
		int fToLoop = Mathf.Clamp (Random.Range (fFromLoop, frontControlPointIndexes.Count - 1), 0, frontControlPointIndexes.Count - 1);
		float fRandOffset = Random.Range (-stretcher, stretcher) + buildingsExtrusion;;

		if (fToLoop > fFromLoop + 1) {

			for (int i = fFromLoop; i < fToLoop; i++) {

				for (int z = 0; z < controlPoints [frontControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [frontControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].x,
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].z + fRandOffset);
				}

			}

		}

		////back calcutations
		int bFrom = Random.Range (0, backControlPointIndexes.Count - 1);
		int bTo = Mathf.Clamp (Random.Range (bFrom, backControlPointIndexes.Count - 1), 0, backControlPointIndexes.Count - 1);
		float bRandOffset = Random.Range (-stretcher, stretcher) + buildingsExtrusion;

		if (bTo > bFrom + 1) {
			for (int i = bFrom; i < bTo; i++) {

				for (int z = 0; z < controlPoints [backControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [backControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [backControlPointIndexes [i]] [z]].x,
						vertices [controlPoints [backControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [backControlPointIndexes [i]] [z]].z + fRandOffset);
				}

			}
		}


		////sides calcutations
		int sType = Random.Range (0, 3);
		int sFrom = 0;
		int sTo = 0;
		int sFrom2 = 0;
		int sTo2 = 0;
		float sRandOffset = Random.Range (-stretcher, stretcher) + buildingsExtrusion;

		switch (sType) {

		case 0:
			sFrom = 1;
			sTo = (sidesControlPointIndexes.Count / 2) - 1;
			break;
		case 1:
			sFrom = (sidesControlPointIndexes.Count / 2) + 1;
			sTo = (sidesControlPointIndexes.Count) - 1;
			break;
		case 2:
			sFrom = 1;
			sTo = (sidesControlPointIndexes.Count/2) - 1;
			sFrom2 = sTo + 2;
			sTo2 = sidesControlPointIndexes.Count - 1;
			break;

		}

		if (sType != 2) {

			for (int i = sFrom; i < sTo; i++) {


				for (int z = 0; z < controlPoints [sidesControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].x + sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].z);
				}

			}
		}else{

			for (int a = sFrom; a < sTo; a++) {

				for (int z = 0; z < controlPoints [sidesControlPointIndexes [a]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [a]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].x + sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].z);
				}

			}
			for (int aa = sFrom2; aa < sTo2; aa++) {

				for (int z = 0; z < controlPoints [sidesControlPointIndexes [aa]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [aa]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].x - sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].z);
				}
			}
		}


		////top calcutations
		float tRandOffset = Random.Range (-stretcher, stretcher/2);

		for (int y = 0; y < topControlPointIndexes.Count; y++) {

			vertices [topControlPointIndexes [y]] = new Vector3 (
				vertices [topControlPointIndexes [y]].x,
				vertices [topControlPointIndexes [y]].y + tRandOffset,
				vertices [topControlPointIndexes [y]].z);

		}

	}

	private void CreateVertices() {


		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;

		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;

		int verticesLength = cornerVertices + edgeVertices + faceVertices;

		int v = 0;
		// sides
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= zSize; z++) {
				SetVertex(v++, xSize, y, z);
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
				SetVertex(v++, x, ySize, z);
			}
		}
		//bottom
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, 0, z);
			}
		}


	}
	private void SetVertex (int i, int x, int y, int z) {

		Vector3 vect = new Vector3 (x, y, z);
		Vector3 inner = vect;


		////sides
		if (x < roundness) {
			inner.x = roundness;
		}
		else if (x > xSize - roundness) {

			inner.x = xSize - roundness;

		}

		////top and bottom
		if (y < roundness) {
			//bottom rounder
			//inner.y = roundness;
		}
		else if (y > ySize - roundness) {
			// top rounder
			//inner.y = 0;
			if (roundTop) {
				inner.y = ySize - roundness;
			} else {
				inner.y = ySize; 
			}
		}


		////front and back
		if (z < roundness) {
			
			inner.z = roundness;
		}
		else if (z > zSize - roundness) {
			inner.z = zSize - roundness;
		
		}

		normals.Add((vect - inner).normalized);
		vertices.Add(inner + normals[i] * roundness);
		uv.Add(new Vector2((float)x / ( xSize), (float)y / (ySize) ));


	}

	private void CreateTriangles () {

		int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		int triLength = quads * 6;
		triangles = new int[triLength];
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
		int vMid = vertices.Count - (xSize - 1) * (zSize - 1);

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

	private void CreateColorAndtexture(MeshRenderer mR) 
	{

		Texture[] stripes = new Texture[] 
		{
			Resources.Load ("TextureStripe7") as Texture,
			Resources.Load ("TextureStripe8") as Texture,
			Resources.Load ("TextureStripe9") as Texture,
			Resources.Load ("TextureStripe2") as Texture,

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
			Resources.Load ("TextureStripe66") as Texture,
			Resources.Load ("GridSample") as Texture,
			Resources.Load ("Tile") as Texture

		};

		Texture texture = stripes [textureIndex] as Texture;

		Material mat = new Material (Shader.Find (".ShaderExample/GradientThreeColor(Texture)"));
		mat.SetTexture ("_MainTex", texture);
		mat.SetColor ("_ColorTop", Color.white);
		mat.SetColor ("_ColorMid", Color.white);
		mat.SetColor ("_ColorBot", Color.white);
		mat.SetFloat ("_Middle", Random.Range(0.2f,0.6f));
		mR.material = mat;

	}

}
