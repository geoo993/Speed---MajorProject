using UnityEngine;
using System.Collections;

public class TrailControl : MonoBehaviour {


	TrailRenderer trail;
	private CharacterMovement craftMovement;

	void Start () {

		trail = GetComponent<TrailRenderer> ();
		craftMovement = transform.parent.gameObject.GetComponent<CharacterMovement> ();
	}

	void Update () {

		if (craftMovement.groundState || craftMovement.airSate) {
			
			trail.enabled = true;
		} else {

			trail.enabled = false;
		}
	}
}
