using UnityEngine;
using System.Collections;


public class CharacterThruster : MonoBehaviour {


	public float thrusterStrength, thrusterDistance;
	public Transform[] thrusters;

	private Rigidbody rigidB;
	private CharacterMovement craftMovement;

	void Awake(){
		rigidB = GetComponent<Rigidbody> ();
		craftMovement = GetComponent<CharacterMovement> ();

	}

	void FixedUpdate () 
	{
		
		if(craftMovement.groundState == true){
			
			RaycastHit hit;
			foreach (Transform thruster in thrusters) 
			{
				Vector3 downwardForce;
				float distancePercentage;


				if (Physics.Raycast (thruster.position, thruster.up * -1, out hit, thrusterDistance)) {


					// the thruster is within thrusterDistance to the ground. How far away?
					distancePercentage = 1 - (hit.distance / thrusterDistance);

					//calculate how much force to push:
					downwardForce = transform.up * thrusterStrength * distancePercentage;

					//correct the force for the mass of the car and deltatime:
					downwardForce = downwardForce * Time.deltaTime * rigidB.mass;

					//print (" dist to ground: " + distancePercentage + "    doward force: " + downwardForce);

					//apply the force where the thruster is :
					rigidB.AddForceAtPosition (downwardForce, thruster.position);

				}


			}
		}




	}




}
