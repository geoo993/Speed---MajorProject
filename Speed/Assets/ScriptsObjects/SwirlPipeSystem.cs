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
		Vector3 pos = new Vector3(Random.Range (0, 1000), Random.Range (500.0f, 1000.0f ), Random.Range (1500.0f, 2000.0f));
		this.transform.position = pos;

		pipes = new SwirlPipe[pipeCount];
		for (int i = 0; i < pipeCount; i++) 
		{
			pipes[i] = Instantiate<SwirlPipe>(pipePrefab);
			SwirlPipe pipe = pipes[i];

			pipe.transform.SetParent(transform, false);

			if (i > 0) 
			{
				pipe.AlignWith(pipes[i - 1]);
			}
		}


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

