using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			GameManager.scoreCountDuration = 10.0f;
			GameManager.scoreNum += 20;

			GameManager.health += 20;
			GameManager.flashCount = 0;
			//GameManager.healthCollectableItems += 1;
			//Destroy (this.gameObject);
			//print (gameObject.name + "  has collided with " + col.gameObject.name);

			Items.RemoveObjectFromList(this.gameObject, Items.healthItems);
		}

		//print ("health: " + GameManager.health);

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
