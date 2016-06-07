using UnityEngine;
using System.Collections;

public class HeartColliders : MonoBehaviour {

	void OnCollisionEnter(Collision col){

	
		if (col.gameObject.name == "Craft")
		{
			print (gameObject.name + "  has collided with " + col.gameObject.name);

		}

	}
}
