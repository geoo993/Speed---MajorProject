using UnityEngine;
using System.Collections;

public class ResetPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{
			Destroy (this.gameObject);

			GameManager.collecteditems = 0;
			print (gameObject.name + "  has collided with " + col.gameObject.name +"  now reset");
		}

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
