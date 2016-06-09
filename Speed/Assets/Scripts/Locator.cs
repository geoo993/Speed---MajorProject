using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Locator : MonoBehaviour {

	public GameManager gameManager = null;

	private int range = 200;

	public GameObject city = null;
	public GameObject craft = null;

	public GameObject[] arrowParts = null;
	public Image playerImage;
	[Range(100f, 1500f)] public float radarRange = 600;

	public bool showArrow = false;

	public enum ItemsToLocate { Transformers, Health, Coin, Collectables };
	public ItemsToLocate itemsToLocate = ItemsToLocate.Collectables;

	private Vector3 locatorTarget = Vector3.zero;

	void Start(){

		this.name = "Locator";
	}
	void Update()
	{


//		switch (itemsToLocate) {
//
//		case ItemsToLocate.Transformers:
//			if (CharacterMeshComplete.tranformNum <= 0 && Items.transformerItems.Count <= 0) {
//
//				itemsToLocate = ItemsToLocate.Collectables;
//
//			} else {
//				GameManager.radarIcon = 0 ;
//				locatorTarget = GetClosestIcon (craft.transform.position, Items.transformerItems);
//			}
//			break;
//		case ItemsToLocate.Health: 
//
//			if (GameManager.healthCollectableItems <= 0 && Items.healthItems.Count <= 0) {
//
//				itemsToLocate = ItemsToLocate.Collectables;
//
//			}else{
//				GameManager.radarIcon = 1 ;
//				locatorTarget = GetClosestIcon (craft.transform.position, Items.healthItems);
//			} 
//			break;
//		case ItemsToLocate.Coin: 
//
//			if (GameManager.coinCollectableItems <= 0 && Items.coinItems.Count <= 0) {
//				
//				itemsToLocate = ItemsToLocate.Collectables;
//
//			} else {
//				GameManager.radarIcon = 2 ;
//				locatorTarget = GetClosestIcon (craft.transform.position, Items.coinItems);
//			}
//
//			break;
//		case ItemsToLocate.Collectables: 
//
//			GameManager.radarIcon = 3 ;
			//locatorTarget = city.GetComponent<Items>().nextItemPos;
			locatorTarget = GetClosestIcon (craft.transform.position, Items.collectablesItems);
//	
//			break;
//
//		}
		//gameManager.GetComponent<GameManager>().ShowHideRadarLocatorIcon (GameManager.radarIcon);
		locatorArrow (locatorTarget);

//		if (Input.GetKeyDown ("3") || (Input.GetAxis ("PS4_DirectionalPadHorizontal") == -1.0f)) {
//			itemsToLocate = ItemsToLocate.Transformers;
//		}else if (Input.GetKeyDown ("4") || (Input.GetAxis ("PS4_DirectionalPadVertical") == -1.0f)) {
//			itemsToLocate = ItemsToLocate.Health;
//		}else if (Input.GetKeyDown ("5") || (Input.GetAxis ("PS4_DirectionalPadVertical") == 1.0f)) {
//			itemsToLocate = ItemsToLocate.Coin;
//		}else if (Input.GetKeyDown ("6") || (Input.GetAxis ("PS4_DirectionalPadHorizontal") == 1.0f)) {
//			itemsToLocate = ItemsToLocate.Collectables;
//		}
//
//

	}

	void locatorArrow( Vector3 target){

		float dist = Vector3.Distance (craft.transform.position, target);
		Color col = Color.Lerp (Color.green, Color.red, dist / radarRange);

		playerImage.color =  col;
		foreach (GameObject p in arrowParts) {

			if (showArrow) {
				p.SetActive (true);
				p.GetComponent<MeshRenderer> ().material.color = col;
			} else {
				p.SetActive (false);
			}
		}

		this.transform.position = new Vector3 (
			craft.transform.position.x, 
			craft.transform.position.y + 8, 
			craft.transform.position.z);


		Quaternion targetRotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f); 

	}


	Vector3 GetClosestIcon(Vector3 currentPosition, List<GameObject> icons)
	{
		Vector3 closestIcon = new Vector3();
		float closestDistanceSqr = Mathf.Infinity;

		foreach(GameObject potentialIcons in icons)
		{
			Vector3 directionToIcon = potentialIcons.transform.position - currentPosition;

			float dSqrToTarget = directionToIcon.sqrMagnitude;

			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				closestIcon = potentialIcons.transform.position;
			}
		}

		return closestIcon;
	}



}
