using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	// Use this for initialization
	void Start () {

		AddCollectableItemPositon ();
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Rotate (0.0f, 0.2f, 0.0f);

	}
	private void AddCollectableItemPositon(){

		Items.collectablesItemsPositions.Add (this.transform.position);
	}


}
