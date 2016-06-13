using UnityEngine;
using System.Collections;

public class CoinPickUp : MonoBehaviour {


	void Update () {

		this.transform.Rotate (0f, 0f, 5f);

	}



	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			//Destroy (this.gameObject);
			GameObject e = (GameObject)Instantiate(Resources.Load ("CoinExplosionBurstParticle"), col.contacts[0].point, Quaternion.identity);
			Destroy (e, 2.0f);


			if (Items.coinItems.Count > 0) {
				//Destroy (this.gameObject);

				Items.RemoveObjectFromList (this.gameObject, Items.coinItems);

				int pickBuildingIndex = Random.Range (0, GenerateCity.buildingsIndex.Count - 1);
				print ("pick "+pickBuildingIndex);

				GenerateCity.buildingsCurrentIndex = pickBuildingIndex;
				GenerateCity.addOneBuilding = true;

				GenerateCity.RemoveIntFromList (pickBuildingIndex, GenerateCity.buildingsIndex);

				GameManager.coinCollectableItems += 1;

			}

		}

	}



}
