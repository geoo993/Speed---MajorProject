﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class SwirlPipe : MonoBehaviour {

	[Range(1.0f, 50.0f)] public float pipeRadius;
	[Range(1.0f, 100.0f)] public float minCurveRadius; 
	[Range(20.0f, 100.0f)] public float maxCurveRadius;
	[Range(0,100)] public int curveSegment, pipeSegment;

	[Range(0,10)] public float ringDistance;
	[Range(1,360)] public float xRotation;

	private float curveRadius;
	public float CurveRadius {
		get {
			return curveRadius;
		}
	}

	private float curveAngle;
	public float CurveAngle {
		get {
			return curveAngle;
		}
	}

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uv;

	void Awake () {

		this.name = "Pipe";
		CreateMesh ();
	}

	void Update () {

		//CreateMesh ();
		//print (" vertices: " + vertices.Length + "  triangles: " + triangles.Length);
	}


	private void CreateMesh(){

		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter == null)
		{
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = filter.sharedMesh;
		if (mesh == null){
			filter.mesh = new Mesh();
			mesh = filter.sharedMesh;
		}
		mesh.name = "Pipe Mesh";
		mesh.Clear ();

		curveRadius = Random.Range(minCurveRadius, maxCurveRadius);

		SetVertices();
		SetUV();
		SetTriangles();
		SetMeshRenderer ();
		SetMeshCollider ();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

	}
	private void SetVertices () 
	{
		int numberOfVertices = pipeSegment * curveSegment * 4;
		vertices = new Vector3[numberOfVertices];

		float uStep = ringDistance / curveRadius;//(2f * Mathf.PI) / curveSegmentCount;
		curveAngle = uStep * curveSegment * (360f / (2f * Mathf.PI));
		CreateFirstQuadRing(uStep);
		int iDelta = pipeSegment * 4;
		for (int u = 2, i = iDelta; u <= curveSegment; u++, i += iDelta) {
			CreateQuadRing(u * uStep, i);
		}
		mesh.vertices = vertices;

	}
	private void SetUV () {
		uv = new Vector2[vertices.Length];
		for (int i = 0; i < vertices.Length; i+= 4) {
			uv[i] = Vector2.zero;
			uv[i + 1] = Vector2.right;
			uv[i + 2] = Vector2.up;
			uv[i + 3] = Vector2.one;
		}
		mesh.uv = uv;
	}
	private void SetTriangles () 
	{
		int numberOfTriangles = pipeSegment * curveSegment * 6;
		triangles = new int[numberOfTriangles];
		for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
			triangles[t] = i;

//			triangles[t + 1] = triangles[t + 4] = i + 1;
//			triangles[t + 2] = triangles[t + 3] = i + 2;
//
			triangles[t + 1] = triangles[t + 4] = i + 2;
			triangles[t + 2] = triangles[t + 3] = i + 1;

			triangles[t + 5] = i + 3;
		}
		mesh.triangles = triangles;
	}



	private void SetMeshRenderer()
	{
//		MeshRenderer meshRenderer = GetComponent (typeof(MeshRenderer)) as MeshRenderer;
//		Material material =  Resources.Load ("Swirl") as Material;
//		//Material material = new Material (Shader.Find ("Unlit/Texture"));
//		//Texture texture = Resources.Load ("Magma") as Texture;
//		//material.SetTexture("_MainTex",texture);
//		//material.color = Color.white;
//		meshRenderer.material = material;
	}
	private void SetMeshCollider()
	{
		MeshCollider meshCollider = GetComponent (typeof(MeshCollider)) as MeshCollider;
		meshCollider.sharedMesh = mesh;
	}

	private Vector3 GetPointOnTorus (float u, float v) {
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos(v));
		p.x = r * Mathf.Sin(u);
		p.y = r * Mathf.Cos(u);
		p.z = pipeRadius * Mathf.Sin(v);
		return p;
	}

	private void CreateFirstQuadRing (float u) 
	{
		float vStep = (2f * Mathf.PI) / pipeSegment;

		Vector3 vertexA = GetPointOnTorus(0f, 0f);
		Vector3 vertexB = GetPointOnTorus(u, 0f);
		for (int v = 1, i = 0; v <= pipeSegment; v++, i += 4) {
			vertices[i] = vertexA;
			vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
			vertices[i + 2] = vertexB;
			vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
		}
	}
	private void CreateQuadRing (float u, int i) 
	{
		float vStep = (2f * Mathf.PI) / pipeSegment;
		int ringOffset = pipeSegment * 4;

		Vector3 vertex = GetPointOnTorus(u, 0f);
		for (int v = 1; v <= pipeSegment; v++, i += 4) {
			vertices[i] = vertices[i - ringOffset + 2];
			vertices[i + 1] = vertices[i - ringOffset + 3];
			vertices[i + 2] = vertex;
			vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
		}


	}


	public void AlignWith (SwirlPipe pipe) {

		float relativeRotation = Random.Range(0, curveSegment) * xRotation / pipeSegment;
		
		transform.SetParent(pipe.transform, false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.curveAngle);
		transform.Translate(0f, pipe.curveRadius, 0f);
		transform.Rotate(relativeRotation, 0f, 0f);
		transform.Translate(0f, -curveRadius, 0f);
		transform.SetParent(pipe.transform.parent);

	}

}