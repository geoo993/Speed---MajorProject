using UnityEngine;
using System.Collections;

public class TransformerPickUp : MonoBehaviour {


	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{
			CharacterMeshComplete.TranformNum += 1;
			Destroy (this.gameObject);
		}

		//print ("transform: "+CharacterMeshComplete.TranformNum);
	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
