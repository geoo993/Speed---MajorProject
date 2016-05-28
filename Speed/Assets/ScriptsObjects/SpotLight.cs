using UnityEngine;
using System.Collections;

public class SpotLight : MonoBehaviour {

	private float duration = 1.0F;
	public Light[] light;


	private float count = 0.0f;
	private float duration2 = 10.0f;
	private Color nextCol = Color.blue;
	private Color currentCol = Color.yellow;

	void Start() 
	{
		
	}
	void Update() {


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

		for (int i = 0; i < light.Length; i++) {
			light[i].color = col;
			light[i].intensity = amplitude;

		}
		//this.transform.Rotate (0.0f, 1f, 0.0f);
	}
}
