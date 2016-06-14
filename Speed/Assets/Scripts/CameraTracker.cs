using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	private Transform target;
	private float distance = 50.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensivityX = 0.0f;
	private float sensivityY = 0.0f;

	[Range(-50.0f, 50.0f)]public float distanceUP, distanceBack, minimumHeight =  1.0f;

	private Vector3 positionVelocity;
	private Vector3 offset;

	private string getPos = "ball";

	private Vector3 prevPos;
	private float groundTimer = 0.0f;

	void Start (){
		
		this.name = "Main Camera";
	}

	void Update ()
	{
		
		target = GameObject.Find ("Craft").transform;
		
		UpdateControls ();
	}


	private Vector3 lookTargetFromBehind()
	{
		////calculate a new position to place the camera:
		Vector3 newPosition =  target.position + (target.forward * distanceBack);
		float newPosY = Mathf.Max (newPosition.y + distanceUP, minimumHeight);
		newPosition = new Vector3(newPosition.x, newPosY, newPosition.z);

		return newPosition;

	}

	void UpdateControls(){

		if (GameObject.Find("GameManager").GetComponent<GameManager>().controlsType == GameManager.ControlsType.Keyboard) 
		{
			currentX += Input.GetAxis ("VerticalSW");
			currentY += Input.GetAxis ("HorizontalAD");

		} else if (GameObject.Find("GameManager").GetComponent<GameManager>().controlsType == GameManager.ControlsType.Controller) 
		{
			if (GameObject.Find("GameManager").GetComponent<GameManager>().switchAnalogStick) {
				currentX += Input.GetAxis ("PS4_LeftAnalogVertical");
				currentY += Input.GetAxis ("PS4_LeftAnalogHorizontal");
			} else {
				currentX += Input.GetAxis ("PS4_RightAnalogVertical");
				currentY += Input.GetAxis ("PS4_RightAnalogHorizontal");
			}
		}
	}

	void LateUpdate () {

		if (target != null) {
			if (target.GetComponent<CharacterMovement> ().ballState == true) {
			
				FollowTargetWhenRolling ();

			} else if (target.GetComponent<CharacterMovement> ().groundState == true || target.GetComponent<CharacterMovement> ().airSate == true) {

				FollowTargetOnGroundAir ();
	
			}
		}

	}

	void FollowTargetWhenRolling ()
	{
		Vector3 dir = new Vector3 (0.0f, 0.0f, -distance);
		Quaternion rotation = Quaternion.Euler (currentX, currentY, 0);
		transform.position = target.position + rotation * dir;
		transform.LookAt (target.position);

		//print ("pos: " + transform.position + "   rotation: " + rotation);

//		if (getPos == "ball") {
//			transform.position = lookTargetFromBehind();
//			offset = transform.position - target.position ;
//			getPos = "idle";
//		}
//
//		transform.position = target.position + offset ;
//		//transform.LookAt (transform.position);
//
//		float movement = Input.GetAxis ("Horizontal2") * 20f * Time.deltaTime;
//		if(!Mathf.Approximately (movement, 0f)) {
//			transform.RotateAround (target.position, Vector3.up, movement);
//			offset = transform.position - target.position ;
//		}

	}

	void FollowTargetOnGroundAir (){

		//getPos = "ball";

		transform.position = Vector3.SmoothDamp (transform.position, lookTargetFromBehind (), ref positionVelocity, 0.18f);

		////rotate the camera to look at where the target is pointing
		Vector3 lookAt = target.position + (target.forward * 5);
		transform.LookAt (lookAt);

	}



}
