using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwirlPipeSystem : MonoBehaviour 
{

	public SwirlPipe pipePrefab;
	public int pipeCount;
	public enum PipeType { easy, hard };
	public PipeType pipeType = PipeType.hard;

	public GameObject coin = null;

	private SwirlPipe[] pipes;

	private float time = 0; 
	private Color currentColor = Color.green; 
	private Color previousColor = Color.red; 
	private int duration = 20;

	private Vector3 pipePos;

	private void Awake () 
	{
		if (pipeType == PipeType.easy) {
			
			pipePos = new Vector3 (0.0f, Random.Range (1000.0f, 2000.0f), Random.Range (1000.0f, 1500.0f));

		} else if (pipeType == PipeType.hard) {
			pipePos = new Vector3 (0.0f, Random.Range (1000.0f, 2000.0f), -Random.Range (1000.0f, 1500.0f));

		}
		this.transform.position = pipePos;


		pipes = new SwirlPipe[pipeCount];
		for (int i = 0; i < pipeCount; i++) 
		{
			pipes[i] = Instantiate<SwirlPipe>(pipePrefab);
			SwirlPipe pipe = pipes[i];

			Vector3 pos = pipe.transform.localPosition;

			if (i == pipeCount / 2) 
			{
				pipe.name = "collectablePipeParent";
				Items.pipeCollectablesItemsPositions.Add ( CircumferencePoint(pos, pipe.CurveAngle - pipe.pipeRadius, pipe.CurveRadius));

			} else {
				createCollectables (pos, pipe.CurveRadius, pipe.CurveAngle - pipe.pipeRadius, pipe.transform);
			}

			pipe.transform.SetParent(transform, false);

			if (i > 0) 
			{
				pipe.AlignWith(pipes[i - 1]);
			}
		}


		if (pipeType == PipeType.easy) {

			int lastIndex = pipes.Length - 1;
			Items.fourPoints2.Add (CircumferencePoint (pipes [0].transform.position, pipes [0].CurveAngle - pipes [0].pipeRadius, pipes [0].CurveRadius));
			Items.fourPoints3.Add (CircumferencePoint (pipes [lastIndex].transform.position, pipes [lastIndex].CurveAngle - pipes [lastIndex].pipeRadius, pipes [lastIndex].CurveRadius));

		}
		if (pipeType == PipeType.hard) {

			int lastIndex = pipes.Length - 1;
			Items.fourPoints1.Add (CircumferencePoint (pipes [0].transform.position, pipes [0].CurveAngle - pipes [0].pipeRadius, pipes [0].CurveRadius));
			Items.fourPoints4.Add (CircumferencePoint (pipes [lastIndex].transform.position, pipes [lastIndex].CurveAngle - pipes [lastIndex].pipeRadius, pipes [lastIndex].CurveRadius));
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

		//GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject a = Instantiate(coin) as GameObject;
		a.transform.parent = parent;
		a.transform.localPosition = CircumferencePoint(pos, angle, radius);
		//a.transform.localScale = new Vector3 (10f, 10f, 10f);
		Items.coinItems.Add (a);
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

