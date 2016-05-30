using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
	//.............. plane...............
    //Rotaton and position of our airplane
	private static float rotationx = 0;
	private static float rotationy = 0.0f;
	private static float rotationz = 0.0f;

	[HideInInspector] public static float maxSpeed = 800f;
	[HideInInspector] public static float minSpeed = 600f;
	[HideInInspector] public static float defaultSpeed = 700f;
	[HideInInspector] public static float speed = 0.0f;// speed variable is the speed


	private float pseudogravitation = -0.3f; // downlift for driving through landscape

	private float rightleftsoft = 0.0f; // Variable for soft curveflight
	private float rightleftsoftabs = 0.0f; // Positive rightleftsoft Variable 

	private float divesalto  = 0.0f; //blocks the forward salto
	private float diveblocker = 0.0f; //blocks sideways stagger flight while dive


	//.............. hovering car...............
	public float rotationRate;
	public float turnRotationAngle, turnRotationSeekSpeed;
	private float rotationVelocity, groundAngleVelocity;

	private Rigidbody rigid; 
	private CharacterMeshComplete craft;

	[HideInInspector] public bool ballState, groundState, airSate;

	private bool changeSpeed = false;


	void Awake(){

		craft = GetComponent<CharacterMeshComplete> ();
		rigid = GetComponent<Rigidbody> ();
	
	}

	void Update () 
	{

		VehicleTransition ();
		RigidBodyControl ();
		UpdateSpeed ();
	}

	void FixedUpdate ()
	{
		if (ballState) {
			BallMovement ();
		}

		if (groundState) {

			GroundHoveringMovement ();
		}

	}
	void VehicleTransition(){


		if (airSate) {
			AirMovement ();
		}

		if ((craft.moveState == "ball" && craft.animateCount == 0) || (craft.moveState == "car" && craft.animateCount == 2)) {

			ballState = true;
			groundState = false;
			airSate = false;

		}else if ((craft.moveState == "car" && craft.animateCount == 1) || (craft.moveState == "airplane" && craft.animateCount == 3)) {

			ballState = false;
			groundState = true;
			airSate = false;
			changeSpeed = true;

		} else if (
			(craft.moveState == "airplane" && craft.animateCount == 2) || (craft.moveState == "jet" && craft.animateCount == 4)||
			(craft.moveState == "jet" && craft.animateCount == 3) || (craft.moveState == "nasa" && craft.animateCount == 5) ||
			(craft.moveState == "nasa" && craft.animateCount == 4)){

			ballState = false;
			groundState = false;
			airSate = true;
		}

	}

	void RigidBodyControl()
	{

		if ((craft.moveState == "ball" && craft.animateCount == 0) || (craft.moveState == "car" && craft.animateCount == 2)) {

			rigid.useGravity = true;
			rigid.mass = 1f;


		}else if ((craft.moveState == "car" && craft.animateCount == 1) || (craft.moveState == "airplane" && craft.animateCount == 3)) {
				
			rigid.useGravity = true;
			rigid.mass = 100f;

		} else if((craft.moveState == "airplane" && craft.animateCount == 2) || (craft.moveState == "jet" && craft.animateCount == 4))
		{
			rigid.useGravity = false;
			rigid.mass = 300f;

		} else if ((craft.moveState == "jet" && craft.animateCount == 3) || (craft.moveState == "nasa" && craft.animateCount == 5)){
			rigid.useGravity = false;
			rigid.mass = 220f;
		
		}else if (craft.moveState == "nasa" && craft.animateCount == 4)
		{
			rigid.useGravity = false;
			rigid.mass = 150f;

		} 

	}


	void BallMovement(){
		speed = 200f;

		float moveHorizontal = Input.GetAxis ("Horizontal"); 
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigid.AddForce (movement * (speed * 10) * Time.deltaTime);

	}

	void GroundHoveringMovement ()
	{
		speed = 300f;

		//check if we are touching the ground:
		if (Physics.Raycast (transform.position, transform.up * -1, 10f)) {
			////we are on the ground; enable the accelartor and increase drag:
			rigid.drag = 1;

			////calculate forward force:
			Vector3 forwardForce = transform.forward * (speed * 10) * Input.GetAxis ("Vertical");

			////correct the force for deltatime and vehical mass:
			forwardForce = forwardForce * Time.deltaTime * rigid.mass;
			rigid.AddForce (forwardForce);


		} else {
			//we aren't on the ground and dont want to just halt in the mid-air; reduce drag:
			rigid.drag = 0;
		}

		////you can turn in the air or on the ground:
		Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis ("Horizontal");

		////correct the force for deltatime and vehicle mass:
		turnTorque = turnTorque * Time.deltaTime * rigid.mass;
		rigid.AddTorque (turnTorque);

		////"Fake" rotate the car when you are turning:
		Vector3 newRotation = transform.eulerAngles;
		float xRotation = Mathf.Lerp (this.transform.rotation.x, 0f, Time.deltaTime / 5.0f);
		float zRotation = Mathf.SmoothDampAngle (
			newRotation.z, Input.GetAxis ("Horizontal") * - turnRotationAngle,
			ref rotationVelocity, turnRotationSeekSpeed);
		newRotation = new Vector3 (0, newRotation.y, zRotation);
		transform.eulerAngles = newRotation;

	}


	void AirMovement(){


		if (changeSpeed == true) {
			speed = 700f;
			changeSpeed = false;
		}


		// Turn variables to rotation and position of the object
		rotationx = (int)transform.eulerAngles.x; 
		rotationy = (int)transform.eulerAngles.y; 
		rotationz = (int)transform.eulerAngles.z; 

		//------------------------- Rotations of the airplane -------------------------------------------------------------------------

		//Up Down, limited to a minimum speed
		if ((Input.GetAxis ("Vertical") <= 0) && ((speed > 595))) {
			transform.Rotate ((Input.GetAxis ("Vertical") * Time.deltaTime * 80), 0, 0); 
		}
		//Special case dive above 90 degrees
		if ((Input.GetAxis ("Vertical") > 0) && ((speed > 595))) {
			transform.Rotate ((0.8f - divesalto) * (Input.GetAxis ("Vertical") * Time.deltaTime * 80), 0, 0); 
		}

		//// Left Right in the air  
		transform.Rotate (0, Time.deltaTime * 100 * rightleftsoft, 0, Space.World); 

		//Tilt multiplied with minus 1 to go into the right direction	
		transform.Rotate (0, 0, Time.deltaTime * 100 * (1.0f - rightleftsoftabs - diveblocker) * Input.GetAxis ("Horizontal") * -1.0f); 		

		//------------------------------------------------ Pitch and Tilt calculations ------------------------------------------
		//variable rightleftsoft + rightleftsoftabs

		//	//Soft rotation calculation -----This prevents the airplaine to fly to the left while it is still tilted to the right
		if ((Input.GetAxis ("Horizontal") <= 0) && (rotationz > 0) && (rotationz < 90))
			rightleftsoft = rotationz * 2.2f / 100 * -1; //to the left
		if ((Input.GetAxis ("Horizontal") >= 0) && (rotationz > 270))
			rightleftsoft = (7.92f - rotationz * 2.2f / 100);//to the right

		//Limit rightleftsoft so that the switch isn`t too hard when flying overhead
		if (rightleftsoft > 1)
			rightleftsoft = 1;
		if (rightleftsoft < -1)
			rightleftsoft = -1;

		//Precision problem rightleftsoft to zero
		if ((rightleftsoft > -0.01f) && (rightleftsoft < 0.01f))
			rightleftsoft = 0;

		//Retreives positive rightleftsoft variable 
		rightleftsoftabs = Mathf.Abs (rightleftsoft);

		// -------------------- Calculations Block salto forward -----------------------------------------------------

		// Variable divesalto
		//   dive salto forward blocking
		if (rotationx < 90)
			divesalto = rotationx / 100.0f;//Updown
		if (rotationx > 90)
			divesalto = -0.2f;//Updown

		//Variable diveblocker
		// blocks sideways stagger flight while dive
		if (rotationx < 90)
			diveblocker = rotationx / 200.0f;
		else
			diveblocker = 0;

		//---------------------------- everything rotate back ---------------------------------------------------------------------------------

		//  rotateback when key wrong direction 
		if ((rotationz < 180) && (Input.GetAxis ("Horizontal") > 0))
			transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);
		if ((rotationz > 180) && (Input.GetAxis ("Horizontal") < 0))
			transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);

		//Rotate back in z axis general, limited by no horizontal button pressed
		if (!Input.GetButton ("Horizontal")) 
		{
			if ((rotationz < 135))
				transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * -100);
			if ((rotationz > 225))
				transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * 100);
		}

		//Rotate back X axis
		if (!Input.GetButton ("Vertical") ) {
			if ((rotationx > 0) && (rotationx < 180))
				transform.Rotate (Time.deltaTime * -50, 0, 0);
			if ((rotationx > 0) && (rotationx > 180))
				transform.Rotate (Time.deltaTime * 50, 0, 0);
		}

		//----------------------------Speed driving and flying ----------------------------------------------------------------

		// Speed
		transform.Translate (0, 0, speed / 20f * Time.deltaTime);
		//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable

		// Input Accellerate and deccellerate in the air
		if ((Input.GetKey ("z")) && (speed < maxSpeed))////800
			speed += Time.deltaTime * 240;
		if ((Input.GetKey ("x")) && (speed > minSpeed))//(speed > 600))  ///600
			speed -= Time.deltaTime * 240;

		if (speed < 0)
			speed = 0; //floatingpoint calculations makes a fix necessary so that speed cannot be below zero

		//Another speed floatingpoint fix:
		if ((!Input.GetKey ("z")) && (!Input.GetKey ("x")) && (speed > (defaultSpeed - 5f)) && (speed < (defaultSpeed + 5f))) ////695 === 705
			speed = defaultSpeed;
		

		//----------------------------------------------------- Uplift  ----------------------------------------------------------------------

		//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. 
		//Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.

		// Neutral speed at 700
		//This code resets the speed to 700 when there is no acceleration or deccelleration. speed is between Maximum 800, minimum 600, so go back to 700
		if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > (minSpeed - 5f)) && (speed < defaultSpeed)) //595 === 700
			speed += Time.deltaTime * 240.0f;
		if ((Input.GetKey ("z") == false) && (Input.GetKey ("x") == false) && (speed > (minSpeed - 5f)) && (speed > defaultSpeed))// 595 === 700
			speed -= Time.deltaTime * 240.0f;
		
	}

	void UpdateSpeed()
	{

		if (airSate) 
		{
			float lerpingTime = 1f * Time.deltaTime;

			if ((craft.moveState == "airplane" && craft.animateCount == 2) || (craft.moveState == "jet" && craft.animateCount == 4)) {

				maxSpeed = Mathf.Lerp(maxSpeed, 1000f, lerpingTime);
				minSpeed = Mathf.Lerp(minSpeed, 600f, lerpingTime);
				defaultSpeed = Mathf.Lerp(defaultSpeed, 800f, lerpingTime);
			}

			if ((craft.moveState == "jet" && craft.animateCount == 3) || (craft.moveState == "nasa" && craft.animateCount == 5)) 
			{
				maxSpeed = Mathf.Lerp(maxSpeed, 1500f, lerpingTime);
				minSpeed = Mathf.Lerp(minSpeed, 900f, lerpingTime);
				defaultSpeed = Mathf.Lerp(defaultSpeed, 1100f, lerpingTime);

			}

			if ((craft.moveState == "nasa" && craft.animateCount == 4)) 
			{
				maxSpeed = Mathf.Lerp(maxSpeed, 2000f, lerpingTime);
				minSpeed = Mathf.Lerp(minSpeed, 1200f, lerpingTime);
				defaultSpeed = Mathf.Lerp(defaultSpeed, 1500f, lerpingTime);
			}


		}

	}

//	void OnGUI() {
//		
//		GUI.contentColor = Color.red; 
//		GUI.Label(new Rect(2000, 10, 200, 20), ("Plane Speed: "+ speed).ToString());
//		GUI.Label(new Rect(2000, 30, 200, 20), ("Plane Y: "+ this.transform.position.y).ToString());
//		GUI.Label(new Rect(2000, 50, 200, 20), ("Plane Z: "+ this.transform.position.z).ToString());
//		GUI.Label(new Rect(2000, 70, 200, 20), ("Plane X: "+ this.transform.rotation.x).ToString());
//		GUI.Label(new Rect(2000, 90, 200, 20), ("Plane Rotation X: "+ this.transform.localRotation.x).ToString());
//		GUI.Label(new Rect(2000, 110, 200, 20), ("Plane Rotation Y: "+ this.transform.localRotation.y).ToString());
//		GUI.Label(new Rect(2000, 130, 200, 20), ("Plane Rotation Z: "+ this.transform.localRotation.z).ToString());
//		GUI.Label(new Rect(2000, 230, 200, 20), ("rightleftsoftabs: "+ rightleftsoftabs).ToString());
//		GUI.Label(new Rect(2000, 250, 200, 20), ("Pseudo gravitation: "+ pseudogravitation).ToString());
//	}


}