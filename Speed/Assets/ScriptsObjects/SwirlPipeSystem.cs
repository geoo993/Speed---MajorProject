using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwirlPipeSystem : MonoBehaviour 
{

	public SwirlPipe pipePrefab;

	public int pipeCount;

	private SwirlPipe[] pipes;

	private float time = 0; 
	private Color currentColor = Color.green; 
	private Color previousColor = Color.red; 
	private int duration = 20;

	private void Awake () 
	{
		Vector3 smallPipePos = new Vector3(Random.Range (700, 1200), Random.Range (500.0f, 1000.0f ), Random.Range (1500.0f, 2000.0f));
		Vector3 bigPipePos = new Vector3(-(Random.Range (700, 1200)), Random.Range (500.0f, 1000.0f ), Random.Range (1500.0f, 2000.0f));
		this.transform.position = smallPipePos;

		pipes = new SwirlPipe[pipeCount];
		for (int i = 0; i < pipeCount; i++) 
		{
			pipes[i] = Instantiate<SwirlPipe>(pipePrefab);
			SwirlPipe pipe = pipes[i];

			Vector3 pos = this.transform.localPosition + pipe.transform.localPosition;
			createCollectables (pos, pipe.CurveRadius, pipe.CurveAngle - pipe.pipeRadius, pipe.transform);


			pipe.transform.SetParent(transform, false);

			if (i > 0) 
			{
				pipe.AlignWith(pipes[i - 1]);
			}
		}


	}

	Vector3 CircumferencePoint ( Vector3 center , float ang,  float radius  ){
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z;
		return pos;
	}

	private GameObject createCollectables(Vector3 pos, float radius, float angle, Transform parent){

		GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
		a.transform.parent = parent;
		a.transform.localPosition = CircumferencePoint(pos, angle, radius);
		a.transform.localScale = new Vector3 (10f, 10f, 10f);

		return a;
	}


	private void Update () 
	{
		if (time < 1.0f) {
			time += Time.deltaTime / duration;
		} else {

			time = 0;
			duration = Random.Range (20, 50);

			currentColor = previousColor;
			previousColor = ExtensionMethods.RandomColor();
		}

		Color col = Color.Lerp (currentColor, previousColor, time);


		for (int i = 0; i < pipes.Length; i++) {

			pipes[i].GetComponent<MeshRenderer>().material.SetColor ("_Color", col);
		}
	}



}

