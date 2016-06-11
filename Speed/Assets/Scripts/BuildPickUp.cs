using UnityEngine;
using System.Collections;

public class BuildPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{

			GameObject e = (GameObject)Instantiate(Resources.Load ("CoinExplosionBurstParticle"), col.contacts[0].point, Quaternion.identity);
			Destroy (e, 2.0f);



			//Destroy (this.gameObject);
			GameManager.scoreCountDuration = 10.0f;
			GameManager.scoreNum += 10;
			GameManager.coinCollectableItems += 1;

			Items.RemoveObjectFromList (this.gameObject, Items.coinItems);

			if (GameManager.coinCollectableItems < GenerateCity.buildingsIndex.Count) {

				Items.numberCollected += 1;
				int pickBuildingIndex = Random.Range (0, GenerateCity.buildingsIndex.Count - 1);
				//print ("pick "+pickBuildingIndex);
				GenerateCity.buildingsRemovedAreaIndex.Add(pickBuildingIndex);
				GenerateCity.buildingsCurrentIndex = pickBuildingIndex;
				//GenerateCity.addOneBuilding = true;

				GenerateCity.RemoveIntFromList (pickBuildingIndex, GenerateCity.buildingsIndex);
			}

		}

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
