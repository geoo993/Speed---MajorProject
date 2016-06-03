using UnityEngine;
using System.Collections;

public class CoinPickUp : MonoBehaviour {

	public GameObject city = null;
	
	// Update is called once per frame
	void Update () {

		this.transform.Rotate (0f, 0f, 5f);
	}



	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			//Destroy (this.gameObject);

			if (Items.coinItems.Count > 0) {
				//Destroy (this.gameObject);
				GameManager.coinCollectableItems += 1;
				Items.RemoveObjectFromHealthList (this.gameObject, Items.coinItems);
			}

		}

	}

	void OnCollisionExit(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{

		}

	}

}
