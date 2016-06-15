using UnityEngine;
using System.Collections;

public class HeartColliders : MonoBehaviour {

	private float time = 0f;
	[Range(-500, 500f)]public float X = 0;
	[Range(-500 , 500f)]public float Y = 0;
	[Range(-500, 500f)]public float Z = 0;

	void Update(){

		this.transform.localPosition = new Vector3 ( pumping (time) * X, pumping (time ) * Y, pumping (time ) * Z );

		float dist = Vector3.Distance (this.transform.position, GameObject.Find ("Craft").transform.position);

		if (dist < 500f) {
			
			time = 10f;
		} else {
			time = 1f;
		}

	}

	public float pumping( float duration)
	{
		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;

		return amplitude;
	}


	void OnCollisionEnter(Collision col){


		if (col.gameObject.name == "Craft")
		{
			print (gameObject.name + "  has collided with " + col.gameObject.name);

			float impact = 	Vector3.Magnitude(col.gameObject.GetComponent<Rigidbody>().velocity);

			print ("impact " + impact);
			GameManager.health -= (int)impact / CharacterCollider.healthDamage;
		}


	}

}
