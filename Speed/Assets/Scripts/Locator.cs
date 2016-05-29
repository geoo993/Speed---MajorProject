using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Locator : MonoBehaviour {

	private int range = 200;

	public GameObject city = null;
	public GameObject craft = null;
	public GameManager gameManager = null;

	public GameObject[] arrowParts = null;
	public Image playerImage;
	[Range(100f, 1500f)] public float radarRange = 600;

	public bool showArrow = false;

	public enum ItemsToLocate { Transformers, Health, Reset, Collectables };
	public ItemsToLocate itemsToLocate = ItemsToLocate.Collectables;

	private Vector3 locatorTarget = Vector3.zero;

	void Update()
	{


		switch (itemsToLocate) {

		case ItemsToLocate.Transformers:

			gameManager.GetComponent<GameManager>().ShowHideRadarLocatorIcon (0);
			locatorTarget = GetClosestIcon (craft.transform.position, Items.transformerItems);

			break;
		case ItemsToLocate.Health: 

			gameManager.GetComponent<GameManager>().ShowHideRadarLocatorIcon (1);
			locatorTarget = GetClosestIcon (craft.transform.position, Items.healthItems);

			break;
		case ItemsToLocate.Reset: 

			gameManager.GetComponent<GameManager>().ShowHideRadarLocatorIcon (2);
			locatorTarget = GetClosestIcon (craft.transform.position, Items.resetItems);

			break;
		case ItemsToLocate.Collectables: 

			gameManager.GetComponent<GameManager>().ShowHideRadarLocatorIcon (3);
			locatorTarget = city.GetComponent<Items>().nextItemPos;

			break;

		}

		locatorArrow (locatorTarget);

		//print (city.GetComponent<Items> ().healthItems.Count);

//		print ("reset items "+city.GetComponent<Items> ().resetItems.Count);
//		print ("transformer items "+city.GetComponent<Items> ().transformerItems.Count);
//		print ("health items "+ city.GetComponent<Items> ().healthItems.Count);

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
