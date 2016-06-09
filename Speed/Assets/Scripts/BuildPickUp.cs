using UnityEngine;
using System.Collections;

public class BuildPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			//Destroy (this.gameObject);

			GameManager.coinCollectableItems += 1;

			Items.RemoveObjectFromList (this.gameObject, Items.coinItems);

			if (GameManager.coinCollectableItems < GenerateCity.buildingsIndex.Count) {

				Items.numberCollected += 1;
				int pickBuildingIndex = Random.Range (0, GenerateCity.buildingsIndex.Count - 1);
				//print ("pick "+pickBuildingIndex);
				GenerateCity.buildingsRemovedAreaIndex.Add(pickBuildingIndex);
				GenerateCity.buildingsCurrentIndex = pickBuildingIndex;
				GenerateCity.addOneBuilding = true;

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
