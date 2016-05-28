using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Locator : MonoBehaviour {

	private int range = 200;

	public GameObject city = null;
	public GameObject craft = null;

	public GameObject[] arrowParts = null;
	public Image playerImage;
	[Range(100f, 1500f)] public float radarRange = 600;

	public bool showArrow = false;

	void Update()
	{

		float dist = Vector3.Distance (craft.transform.position, city.GetComponent<Items> ().nextItemPos);
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

		Vector3 targetPoint = city.GetComponent<Items>().nextItemPos;
		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f); 



	}


}
