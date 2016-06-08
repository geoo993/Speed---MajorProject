using UnityEngine;
using System.Collections;

public class CoinPickUp : MonoBehaviour {

	public GameObject city = null;

	private Mesh mesh;
	private MeshCollider mCollider;

	void Start (){


	}
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

	void OnCollisionExit(Collision col){

		//print (gameObject.name + "  has collided with " + col.gameObject.name);

		if (col.gameObject.name == "craft")
		{

		}

	}

}
