using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarObject {

	public Image icon { get; set; }
	public GameObject owner { get; set; }

}

public class Radar : MonoBehaviour {

	public Transform playerPos;
	private float mapScale = 1f;

	public static List<RadarObject> radarObjects = new List<RadarObject> ();

	public static void RegisterRadarObject(GameObject o, Image i)
	{

		Image image = Instantiate (i);
		radarObjects.Add ( new RadarObject(){owner = o, icon = image} );
	}

	public static void RemoveRadarObject(GameObject o)
	{
		List <RadarObject> newList = new List<RadarObject> ();

		for (int i = 0; i < radarObjects.Count; i++) {

			if (radarObjects [i].owner == o) {

				Destroy (radarObjects [i].icon);
				continue;
			} else {

				newList.Add (radarObjects [i]);
			}
		}

		radarObjects.RemoveRange (0, radarObjects.Count);
		radarObjects.AddRange (newList);

	}

	void DrawRadarDots(){

		foreach (RadarObject radObj in radarObjects) 
		{

			float angle = -90;//270.0f;
			Vector3 radarPos = (radObj.owner.transform.position - playerPos.position); 
			float distanceToObject = Vector3.Distance (playerPos.localPosition, radObj.owner.transform.position) * mapScale;
			float deltaY = Mathf.Atan2 (radarPos.x, radarPos.z) * Mathf.Rad2Deg - angle - playerPos.eulerAngles.y;
			radarPos.x = distanceToObject * Mathf.Cos (deltaY * Mathf.Deg2Rad) * - 1f;
			radarPos.z = distanceToObject * Mathf.Sin (deltaY * Mathf.Deg2Rad);	

			radObj.icon.transform.SetParent(this.transform);
			radObj.icon.transform.position = new Vector3 (radarPos.x,radarPos.z, 0.0f) + this.transform.position;


			if (playerPos.position.y < -10.0f) {
				radObj.icon.color = new Color (radObj.icon.color.r, radObj.icon.color.g, radObj.icon.color.b, 0.1f);
			} else {
				radObj.icon.color = new Color (radObj.icon.color.r, radObj.icon.color.g, radObj.icon.color.b, 1f);
			}
		}


	}

	void Update(){

		DrawRadarDots ();


	}


}
