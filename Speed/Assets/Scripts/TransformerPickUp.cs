using UnityEngine;
using System.Collections;

public class TransformerPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			CharacterMeshComplete.TranformNum += 1;
			//Destroy (this.gameObject);
			Items.RemoveObjectFromHealthList(this.gameObject, Items.transformerItems);
		}

		//print ("transform: "+CharacterMeshComplete.TranformNum);
	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
