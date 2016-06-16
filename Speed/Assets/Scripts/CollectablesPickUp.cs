using UnityEngine;
using System.Collections;

public class CollectablesPickUp : MonoBehaviour {

	public GameObject trans = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			GameObject e = (GameObject)Instantiate(Resources.Load ("CollectableExplosionBurstParticle"), col.contacts[0].point, Quaternion.identity);
			Destroy (e, 2.0f);


			//Destroy (this.gameObject);

			GameManager.scoreCountDuration = 15.0f;
			GameManager.scoreNum += 150;

			GameManager.collectedItems += 1;


			if (this.gameObject.name == "CollectableItem12") {
				GameManager.disableSun = true;
			}

			Items.RemoveObjectFromList (this.gameObject, Items.collectablesItems);


		}

	}


}
