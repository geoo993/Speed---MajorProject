using UnityEngine;
using System.Collections;

public class BuildPickUp : MonoBehaviour {

	public GameObject city = null;

	void OnCollisionEnter(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "Craft")
		{
			//Destroy (this.gameObject);
			Items.numberCollected += 1;

			Items.RemoveObjectFromHealthList (this.gameObject, Items.coinItems);

			int pickBuildingIndex = Random.Range (0, GenerateCity.buildingsIndex.Count - 1);
			//print ("pick "+pickBuildingIndex);

			GenerateCity.buildingsCurrentIndex = pickBuildingIndex;
			GenerateCity.addOneBuilding = true;

			GenerateCity.RemoveIntFromList (pickBuildingIndex, GenerateCity.buildingsIndex);

			GameManager.coinCollectableItems += 1;
		}

	}

	void OnTriggerEnter  ( Collider other ) {

		//print (other.name);


	}

	void OnTriggerExit  ( Collider other ) {

	}


}
