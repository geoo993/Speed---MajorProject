using UnityEngine;
using System.Collections;

public class ResetPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			//Destroy (this.gameObject);
			GameManager.resetCollectableItems += 1;

			GameManager.CreateSwirl();
			//print (gameObject.name + "  has collided with " + col.gameObject.name +"  now reset");

			Items.RemoveObjectFromHealthList(this.gameObject, Items.resetItems);
		}

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
