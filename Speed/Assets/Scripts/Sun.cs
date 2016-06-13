using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	private float dist = 0;

	void Start(){

		this.name = "Sun";
	}
	void Update () {

		this.transform.Rotate (0.0f, 0.2f, 0.0f);

		if (this.enabled == true)
		{
			dist = Vector3.Distance(GameObject.Find("Craft").transform.position,this.transform.position);

			if (dist < 100f)
			{
				GameManager.health -= 0.1f;

			}else if (dist < 50f )
			{
				GameManager.health -= 0.3f;
			}
		}

		if (GameManager.disableSun == true) {
			Destroy (this.gameObject);
		}
	}



}
