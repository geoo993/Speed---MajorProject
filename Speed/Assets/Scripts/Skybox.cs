using UnityEngine;
using System.Collections;

public class Skybox : MonoBehaviour {



	private Camera camera;
	private int resolution = 256;
	private float duration = 10.0f;
	private Color CurrentColor =  Color.red;
	private Color previousColor = Color.blue;
	private float t = 0;

	public Color topColor = Color.blue;
	public Color midColor = Color.blue;
	public Color bottomColor = Color.blue;

	public bool lerpTopColor = false;
	public bool lerpMidColor = false;
	public bool lerpBottomColor = false;

	[Range(0.0f, 1.0f)]public float height = 0.5f;

	[HideInInspector] public Color tc;
	[HideInInspector] public Color bc; 
	[HideInInspector] public Color mc;



	public static Material CreateGradientMaterial(Color topColor, Color middleColor, Color bottomColor, float h)
	{
		Material result = new Material (Shader.Find (".ShaderExample/GradientThreeColor"));

		result.SetFloat ("_Middle", h);
		result.SetColor("_ColorTop", topColor);
		result.SetColor("_ColorMid",middleColor);
		result.SetColor("_ColorBot", bottomColor);
		return result;
	}



	void Update()
	{

		////type 3 set with external material
		if (t < 1.0f) {
			t += Time.deltaTime * (1.0f / duration);
		} else {
			t = 0;
			duration = Random.Range (20f, 50f);

			CurrentColor = previousColor;
			previousColor = ExtensionMethods.RandomColor ();
		}
		Color lerp = Color.Lerp (CurrentColor,previousColor, t) / 2.0f;

		//print("time: "+t+" duration:  "+ duration);

		tc = lerpTopColor ? lerp : topColor;
		mc = lerpMidColor ? lerp : midColor;
		bc = lerpBottomColor ? lerp : bottomColor;
		Material material = CreateGradientMaterial(tc, mc, bc, height);
		SetSkybox(material);
		//enabled = false;

	}

	void SetSkybox(Material material)
	{
		GameObject camera = Camera.main.gameObject;
		Skybox skybox = camera.GetComponent<Skybox>();
		if (skybox == null)
			skybox = camera.AddComponent<Skybox>();
		RenderSettings.skybox = material;
	}
}

