using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Locator : MonoBehaviour {

	public Image arrowIndicator = null;
	public Image targetIndicator = null;

	private List<Image> arrowPool = new List<Image>();
	private int arrowCursor = 0;

	private List<Image> targetPool = new List<Image>();
	private int targetCursor = 0;

	private int range = 200;

	public GameObject[] arrowParts = null;
	[Range(100f, 1500f)] public float radarRange = 800;

	public bool showArrow = false;
	public bool showArrowIndicator = false;
	public bool showTargetIndicator = false;

	private Vector3 targetToLocate = Vector3.zero;



	void Start(){

		this.name = "Locator";

	}

	void Update()
	{

		UpdateArrow ();
	}
	void LateUpdate(){

		UpdateIndicators ();

	}

	void UpdateIndicators(){

		ResetPool ();

		foreach (GameObject d in Items.collectablesItems) {
			
			//Vector3 screenPos = Camera.main.WorldToScreenPoint (Items.collectablesItems[Items.collectablesItems.Count - 1].transform.position);
			Vector3 screenPos = Camera.main.WorldToScreenPoint (d.transform.position);

			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
				
			if (screenPos.z > 0 &&
				screenPos.x > 0 && screenPos.x < screenWidth &&
				screenPos.y > 0 && screenPos.y < screenHeight) {//// onScreen

				//print ("On Screen  ");

				Image targetInd = GetTarget ();
				targetInd.color = showTargetIndicator ? GameObject.Find("GameManager").GetComponent<GameManager>().interfaceColor: Color.clear;
				targetInd.transform.localPosition = new Vector3( screenPos.x, screenPos.y , 0.0f);



			} else { //// offScreen

				//print ("Off Screen  ");

				if (screenPos.z < 0) {

					screenPos *= -1;  //stuff is flipped when its behind us
				}

				////create center of the screen instead of bottom left
				Vector3 screenCenter = new Vector3(screenWidth,screenHeight,0.0f) / 2 ;//Vector3.one;
				screenPos -= screenCenter;

				////find angle from center of screen to mouse to mouse position
				float angle = Mathf.Atan2(screenPos.y , screenPos.x );

				angle -= 90.0f * Mathf.Deg2Rad;
				float cos = Mathf.Cos (angle);
				float sin = Mathf.Sin (angle);

				screenPos = screenCenter + new Vector3 (sin + 150.0f, cos + 150.0f, 0.0f);

				////y = mx+b format
				float m = cos / sin;

				Vector3 screenBounds = screenCenter * 0.9f;

				////check if up and down first
				if (cos > 0) {
					//up
					screenPos = new Vector3 (screenBounds.y / m, screenBounds.y, 0.0f);

				} else {
					//down
					screenPos = new Vector3 (-screenBounds.y / m, -screenBounds.y, 0.0f);

				}
				////if out of bounds, get point on appropriate side
				if (screenPos.x > screenBounds.x) {//// out of bounds! must be on the right
					screenPos = new Vector3(-screenBounds.x, screenBounds.x * m, 0.0f);
				}else if(screenPos.x < -screenBounds.x){////out of bounds left
					screenPos = new Vector3(screenBounds.x, -screenBounds.x * m, 0.0f);
				}//else in bounds

				////remove coordinate translation
				screenPos += screenCenter;

				Image arrowInd = GetArrow ();
				
				//float dist = Vector3.Distance (craft.transform.position, (Items.collectablesItems[Items.collectablesItems.Count - 1].transform.position));
				float dist = Vector3.Distance (GameObject.Find("Craft").transform.position, d.transform.position);
				Color col = Color.Lerp (Color.green, Color.red, dist / radarRange);
				arrowInd.color = showArrowIndicator ? col : Color.clear;
				arrowInd.transform.localPosition = new Vector3( screenPos.x , screenPos.y , 0.0f);

				arrowInd.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle * Mathf.Deg2Rad);
				
			}

		}

		CleanPool ();
	}
	private Image GetArrow(){

		Image output;

		if (arrowCursor < arrowPool.Count) {
			output = arrowPool [arrowCursor];// get existing
		} else {
			output = Instantiate(arrowIndicator) as Image; // make new
			output.transform.parent = GameObject.Find ("Canvas/Indicators").transform;
			arrowPool.Add (output);
		}

		arrowCursor ++;

		return output;
	}

	private Image GetTarget(){
		
		Image output;

		if (targetCursor < targetPool.Count) {
			output = targetPool [targetCursor];// get existing
		} else {
			output = Instantiate(targetIndicator) as Image; // make new
			output.transform.parent = GameObject.Find ("Canvas/Indicators").transform;
			targetPool.Add (output);
		}

		targetCursor ++;

		return output;
	
	}

	private void ResetPool(){

		arrowCursor = 0;
		targetCursor = 0;
	}
		
	void CleanPool(){

		while(arrowPool.Count > arrowCursor){

			Image arrow = arrowPool [arrowPool.Count - 1];//get last
			arrowPool.Remove(arrow);
			Destroy (arrow.gameObject);

		}

		while(targetPool.Count > targetCursor){

			Image target = targetPool [targetPool.Count - 1];//get last
			targetPool.Remove(target);
			Destroy (target.gameObject);

		}

	}



	void UpdateArrow(){

		this.transform.position = new Vector3 (
			GameObject.Find("Craft").transform.position.x, 
			GameObject.Find("Craft").transform.position.y + 8, 
			GameObject.Find("Craft").transform.position.z);

		if (Items.collectablesItems.Count > 0) {
			targetToLocate = Items.collectablesItems [Items.collectablesItems.Count - 1].transform.position; //GetClosestIcon (craft.transform.position, Items.collectablesItems);
			FocusOnTarget (targetToLocate);
		}


	}
	void FocusOnTarget( Vector3 target){

		float dist = Vector3.Distance (GameObject.Find("Craft").transform.position, target);
		Color col = Color.Lerp (Color.green, Color.red, dist / radarRange);

		foreach (GameObject p in arrowParts) {
			if (showArrow) {
				p.SetActive (true);
				p.GetComponent<MeshRenderer> ().material.color = col;
			} else {
				p.SetActive (false);
			}
		}

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
