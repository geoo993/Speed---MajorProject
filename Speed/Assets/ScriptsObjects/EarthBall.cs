using UnityEngine;
using System.Collections;

public class EarthBall : MonoBehaviour {

	private float duration = 10.0F;


	private float count = 0.0f;
	private float duration2 = 10.0f;
	private Color nextCol = Color.blue;
	private Color currentCol = Color.yellow;


	void Start(){

		this.name = "Earth";

	}

	void Update () {

		LightAndColor ();

	}

	void LightAndColor()
	{

		this.transform.Rotate (0.0f, 0.5f, 0.0f);

		if (count < 1.0f) {

			count += Time.deltaTime * (1.0f / duration2);
		} else {

			count = 0;
			duration2 = Random.Range (5.0f, 15.0f);
			currentCol = nextCol;
			nextCol = ExtensionMethods.RandomColor ();
		} 

		Color col = Color.Lerp (currentCol, nextCol, count);
		//print (count);

		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

		GetComponent<MeshRenderer> ().material.SetColor ("_Color", col);
		GetComponent<MeshRenderer> ().material.SetColor ("_SpecColor", Color.black * amplitude);
		GetComponent<MeshRenderer> ().material.SetFloat("_Divide", amplitude);


	}
}
