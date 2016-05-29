using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			GameManager.health += 25;
			//Destroy (this.gameObject);
			print (gameObject.name + "  has collided with " + col.gameObject.name);

			Items.RemoveObjectFromHealthList(this.gameObject, Items.healthItems);
		}

		//print ("health: " + GameManager.health);

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
