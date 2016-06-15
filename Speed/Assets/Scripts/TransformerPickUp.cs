using UnityEngine;
using System.Collections;

public class TransformerPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			if (Items.transformerItems.Count > 0) {

				GameManager.scoreCountDuration = 20.0f;
				GameManager.scoreNum += 250;

				GameManager.transformCollectItem += 1;
				//CharacterMeshComplete.tranformNum += 1;
				//Destroy (this.gameObject);
				Items.RemoveObjectFromList (this.gameObject, Items.transformerItems);
			}
		}

		//print ("transform: "+CharacterMeshComplete.TranformNum);
	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);

	}

	void OnTriggerExit  ( Collider other ) {

	}


}
