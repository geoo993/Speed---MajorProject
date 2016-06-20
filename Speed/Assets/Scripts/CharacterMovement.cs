using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{

	private GameManager gameManagerScript;

	public ParticleSystem travelerParticle = null;
	public ParticleSystem rearParticle1 = null;
	public ParticleSystem rearParticle2 = null;



	///.............. balll...............
	private float rollinglerp = 0.0f;
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
	private float moveVertical = 0.0f;
	private float moveHorizontal = 0.0f;

	private string craftLevel = "idle";


	private Vector3 lastPosition;
	private Transform myTransform;
	private Vector3 forwardForce;


	void Awake(){

		gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager> ();
		craft = GetComponent<CharacterMeshComplete> ();
		rigid = GetComponent<Rigidbody> ();
		myTransform = transform;
		lastPosition = myTransform.position;

	}

	void Update () 
	{

		VehicleTransition ();
		RigidBodyControl ();
		UpdateSpeed ();
		SetParticles ();
		SetFieldView ();

	}

	void FixedUpdate ()
	{
		if (ballState) {
			BallMovement ();
		}

		if (groundState) {

			GroundHoveringMovement ();
		}


		if ((craft.moveState == "ball" && craft.animateCount == 0) || (craft.moveState == "car" && craft.animateCount == 2)) {

			craftLevel = "ball";

		}else if ((craft.moveState == "car" && craft.animateCount == 1) || (craft.moveState == "airplane" && craft.animateCount == 3)) {

			craftLevel = "car";

		} else if((craft.moveState == "airplane" && craft.animateCount == 2) || (craft.moveState == "jet" && craft.animateCount == 4))
		{
			craftLevel = "airplane";

		} else if ((craft.moveState == "jet" && craft.animateCount == 3) || (craft.moveState == "nasa" && craft.animateCount == 5)){

			craftLevel = "jet";

		}else if (craft.moveState == "nasa" && craft.animateCount == 4)
		{
			craftLevel = "nasa";
		} 


	}


	void VehicleTransition(){

		if (airSate) {
			AirMovement ();

		}

		if (craftLevel == "ball") {

			ballState = true;
			groundState = false;
			airSate = false;

		} else if (craftLevel == "car") {
			
			ballState = false;
			groundState = true;
			airSate = false;
			changeSpeed = true;

		} else if (craftLevel == "airplane") {

			ballState = false;
			groundState = false;
			airSate = true;
		} else if (craftLevel == "jet") {

			ballState = false;
			groundState = false;
			airSate = true;
		} else if (craftLevel == "nasa") {
			ballState = false;
			groundState = false;
			airSate = true;
		}

	}

	void RigidBodyControl()
	{

		if (craftLevel == "ball") {
			rigid.useGravity = true;
			//rigid.mass = 1f;

		} else if (craftLevel == "car") {

			rigid.useGravity = true;
			rigid.mass = 100f;

		} else if (craftLevel == "airplane") {

			rigid.useGravity = false;
			rigid.mass = 250f;

		} else if (craftLevel == "jet") {

			rigid.useGravity = false;
			rigid.mass = 200f;

		} else if (craftLevel == "nasa") {
			rigid.useGravity = false;
			rigid.mass = 150f;
		}

	}

	void SetParticles(){

		if (craftLevel == "ball") {

			UpdateWindParticle (0.0f, 0.0f,0.0f);
			UpdateRearParticles (0.0f, 0.0f, 0.0f, 0.0f);

		} else if (craftLevel == "car") {

			UpdateWindParticle (0.0f, 2.0f, 0.0f); 
			UpdateRearParticles (0.0f, 0.0f, 0.0f, 0.0f);

		} else if (craftLevel == "airplane") {
			
			UpdateWindParticle (50.0f, 10.0f, 5.0f);
			UpdateRearParticles (150.0f, 100.0f, 50.0f, 60.0f);

		} else if (craftLevel == "jet") {

			UpdateWindParticle (80.0f, 20.0f, 10.0f);
			UpdateRearParticles (80.0f, 50.0f, 20.0f, 40.0f);

		} else if (craftLevel == "nasa") {
			
			UpdateWindParticle (300.0f, 40.0f, 30f);
			UpdateRearParticles (120.0f, 80.0f, 50.0f, 40.0f);
		}

	
	}


	private void UpdateWindParticle(float withSpeed, float withoutSpeed, float slowingDown){

		travelerParticle.startColor = gameManagerScript.interfaceColor;

		if (Input.GetKey ("z") || (Input.GetAxis ("PS4_R2") > 0.0f)) {

			//ParticleSystemExtension.SetEmissionRate (travelerParticle, withSpeed);

		}else if (Input.GetKey ("x") || (Input.GetAxis ("PS4_L2") > 0.0f)) {
			
			//ParticleSystemExtension.SetEmissionRate (travelerParticle, slowingDown);
		}
		else {
			//ParticleSystemExtension.SetEmissionRate (travelerParticle, withoutSpeed);
		}
	}
	private void UpdateRearParticles(float withSpeed, float withoutSpeed, float slowingDown, float startSpeed){

		rearParticle1.startSpeed = startSpeed;
		rearParticle2.startSpeed = startSpeed;

		if (Input.GetKey ("z") || (Input.GetAxis ("PS4_R2") > 0.0f)) {

			//ParticleSystemExtension.SetEmissionRate (rearParticle1, withSpeed);
			//ParticleSystemExtension.SetEmissionRate (rearParticle2, withSpeed);

			rearParticle1.startColor = gameManagerScript.interfaceColor;
			rearParticle2.startColor = gameManagerScript.interfaceColor;


		}else if (Input.GetKey ("x") || (Input.GetAxis ("PS4_L2") > 0.0f)) {

			//ParticleSystemExtension.SetEmissionRate (rearParticle1, slowingDown);
			//ParticleSystemExtension.SetEmissionRate (rearParticle2, slowingDown);
			rearParticle1.startColor = Color.white;
			rearParticle2.startColor = Color.white;
		}
		else {

			//ParticleSystemExtension.SetEmissionRate (rearParticle1, withoutSpeed);
			//ParticleSystemExtension.SetEmissionRate (rearParticle2, withoutSpeed);

			rearParticle1.startColor = Color.white;
			rearParticle2.startColor = Color.white;
		}
	}
	void SetFieldView(){

		if (craftLevel == "ball") {

			UpdateFieldView (moveVertical,"out", 50.0f, 50.0f, 0.0f);

		} else if (craftLevel == "car") {

			UpdateFieldView (moveVertical,"out", 38.0f, 42.0f, 5.0f);

		} else if (craftLevel == "airplane") {

			//30, 40, 50
			Camera.main.fieldOfView = Mathf.Clamp(GameManager.speedValue,30f, 50f);

		} else if (craftLevel == "jet") {
			// 45,  55,  75
			Camera.main.fieldOfView = Mathf.Clamp(GameManager.speedValue - 5.0f,40f, 70f);

		} else if (craftLevel == "nasa") {
			//60, 75, 100
			Camera.main.fieldOfView = Mathf.Clamp(GameManager.speedValue - 10f,50f, 90f);

		}
		//print (Camera.main.fieldOfView);
	}
	void UpdateFieldView(float control, string inOrOut, float minView, float maxView, float sensitivity ){

		float fov = Camera.main.fieldOfView;

		if (inOrOut == "out"){
			fov += control * sensitivity;
		}
		if (inOrOut == "in"){
			fov -= control * sensitivity;
		}
		fov = Mathf.Clamp(fov, minView, maxView);
		Camera.main.fieldOfView = fov;

	}


	void BallMovement(){
		speed = 200f;

		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {

			moveHorizontal = Input.GetAxis ("Horizontal"); 
			moveVertical = Input.GetAxis ("Vertical");

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
			moveHorizontal = Input.GetAxis ("PS4_LeftAnalogHorizontal"); 
			moveVertical = Input.GetAxis ("PS4_LeftAnalogVertical");
		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller || gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {
			moveHorizontal = Input.GetAxis ("360_LeftAnalogHorizontal"); 
			moveVertical = Input.GetAxis ("360_LeftAnalogVertical");
		}



		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {

			//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical) * (speed * 10) * Time.deltaTime;
			//rigid.AddForce(movement);

			forwardForce = (Camera.main.gameObject.transform.forward) * moveVertical * speed; 

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
			
			if ((Input.GetAxis ("PS4_L2") > 0.0f)) 
			{
				forwardForce = (Camera.main.gameObject.transform.forward) * -Input.GetAxis ("PS4_L2") * (speed * 10); 

			} else if ((Input.GetAxis ("PS4_R2") > 0.0f)) {
				
				forwardForce = (Camera.main.gameObject.transform.forward) * Input.GetAxis ("PS4_R2") * (speed * 10); 
			} else {
				forwardForce = Vector3.zero;
			}

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller) {

			if ((Input.GetAxis ("360_LeftThumbTrigger") > 0.0f)) 
			{
				forwardForce = (Camera.main.gameObject.transform.forward) * -Input.GetAxis ("360_LeftThumbTrigger") * (speed * 10); 

			} else if ((Input.GetAxis ("360_RightThumbTrigger") > 0.0f)) {

				forwardForce = (Camera.main.gameObject.transform.forward) * Input.GetAxis ("360_RightThumbTrigger") * (speed * 10); 
			} else {
				forwardForce = Vector3.zero;
			}

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			forwardForce = (Camera.main.gameObject.transform.forward) * -Input.GetAxis ("360PC_ThumbTriggers") * (speed * 10); 

		}

		rigid.AddForce(forwardForce);

		if (moveHorizontal == 0 && moveVertical == 0 && ((Input.GetAxis ("PS4_L2") <= 0.0f) || Input.GetAxis ("360PC_ThumbTriggers") == 0.0f) && (Input.GetAxis ("PS4_R2") <= 0.0f) && this.transform.position.y < 5f) 
		{
			if (rollinglerp < 1.0f) {

				rollinglerp += Time.deltaTime * (1.0f / 10.0f);
			}

			rigid.velocity = Vector3.Lerp (rigid.velocity, Vector3.zero, rollinglerp);
			rigid.angularVelocity = Vector3.Lerp (rigid.angularVelocity, Vector3.zero, rollinglerp);
			rigid.mass = 1f;

		} else {
			rollinglerp = 0;
			rigid.mass = 10f;
		}


	}

	void GroundHoveringMovement ()
	{
		speed = 300f;


		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {
			moveVertical = Input.GetAxis ("Vertical");
			moveHorizontal = Input.GetAxis ("Horizontal");

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
			
			moveVertical = Input.GetAxis ("PS4_LeftAnalogVertical");

			if (gameManagerScript.switchAnalogStick) {
				moveHorizontal = Input.GetAxis ("PS4_LeftAnalogHorizontal");
			} else {
				moveHorizontal = Input.GetAxis ("PS4_RightAnalogHorizontal");
			}

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller || gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			moveVertical = Input.GetAxis ("360_LeftAnalogVertical");

			if (gameManagerScript.switchAnalogStick) {
				moveHorizontal = Input.GetAxis ("360_LeftAnalogHorizontal");
			} else {
				moveHorizontal = Input.GetAxis ("360_RightAnalogHorizontal");
			}

		}


		//check if we are touching the ground:
		if (Physics.Raycast (transform.position, transform.up * -1, 10f)) {
			////we are on the ground; enable the accelartor and increase drag:
			rigid.drag = 1;


			if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {

				forwardForce = transform.forward * (speed * 10) * moveVertical;

			} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
				
				if ((Input.GetAxis ("PS4_L2") > 0.0f)) {
					////calculate forward force:
					forwardForce = transform.forward * (speed * 100) * -Input.GetAxis ("PS4_L2");
				} else if ((Input.GetAxis ("PS4_R2") > 0.0f)) {
					////calculate forward force:
					forwardForce = transform.forward * (speed * 100) * Input.GetAxis ("PS4_R2");
				} else {
					forwardForce = Vector3.zero;
				}
			}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller) {

				if ((Input.GetAxis ("360_LeftThumbTrigger") > 0.0f)) {
					////calculate forward force:
					forwardForce = transform.forward * (speed * 100) * -Input.GetAxis ("360_LeftThumbTrigger");
				} else if ((Input.GetAxis ("360_RightThumbTrigger") > 0.0f)) {
					////calculate forward force:
					forwardForce = transform.forward * (speed * 100) * Input.GetAxis ("360_RightThumbTrigger");
				} else {
					forwardForce = Vector3.zero;
				}
			}else if (gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {
				
					forwardForce = transform.forward * (speed * 100) * -Input.GetAxis ("360PC_ThumbTriggers");
				
			}



			////correct the force for deltatime and vehical mass:
			forwardForce = forwardForce * Time.deltaTime * rigid.mass;
			rigid.AddForce (forwardForce);

			//print (moveVertical +"   R2: "+Input.GetAxis ("PS4_R2")+"   L2: "+Input.GetAxis ("PS4_L2"));

		} else {
			//we aren't on the ground and dont want to just halt in the mid-air; reduce drag:
			rigid.drag = 0;

		}

		////you can turn in the air or on the ground:
		Vector3 turnTorque = Vector3.up * rotationRate * moveHorizontal;

		////correct the force for deltatime and vehicle mass:
		turnTorque = turnTorque * Time.deltaTime * rigid.mass;
		rigid.AddTorque (turnTorque);

		////"Fake" rotate the car when you are turning:
		Vector3 newRotation = transform.eulerAngles;
		float xRotation = Mathf.Lerp (this.transform.rotation.x, 0f, Time.deltaTime / 5.0f);
		float zRotation = Mathf.SmoothDampAngle (
			newRotation.z, moveHorizontal * - turnRotationAngle,
			ref rotationVelocity, turnRotationSeekSpeed);
		newRotation = new Vector3 (0, newRotation.y, zRotation);
		transform.eulerAngles = newRotation;

	}


	void AirMovement(){


		if (changeSpeed == true) {
			speed = 700f;
			changeSpeed = false;
		}

		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {
			moveVertical = Input.GetAxis ("Vertical");
			moveHorizontal = Input.GetAxis ("Horizontal");

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {

			if (gameManagerScript.switchAnalogStick) {
				moveVertical = Input.GetAxis ("PS4_LeftAnalogVertical");
				moveHorizontal = Input.GetAxis ("PS4_LeftAnalogHorizontal");
			} else {
				moveVertical = Input.GetAxis ("PS4_RightAnalogVertical");
				moveHorizontal = Input.GetAxis ("PS4_RightAnalogHorizontal");
			}
		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller || gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			if (gameManagerScript.switchAnalogStick) {
				moveVertical = Input.GetAxis ("360_LeftAnalogVertical");
				moveHorizontal = Input.GetAxis ("360_LeftAnalogHorizontal");
			} else {
				moveVertical = Input.GetAxis ("360_RightAnalogVertical");
				moveHorizontal = Input.GetAxis ("360_RightAnalogHorizontal");
			}
		}

		// Turn variables to rotation and position of the object
		rotationx = (int)transform.eulerAngles.x; 
		rotationy = (int)transform.eulerAngles.y; 
		rotationz = (int)transform.eulerAngles.z; 

		//------------------------- Rotations of the airplane -------------------------------------------------------------------------

		//Up Down, limited to a minimum speed
		if ((moveVertical <= 0) && ((speed > 595))) {
			transform.Rotate ((moveVertical * Time.deltaTime * 80), 0, 0); 
		}
		//Special case dive above 90 degrees
		if ((moveVertical > 0) && ((speed > 595))) {
			transform.Rotate ((0.8f - divesalto) * (moveVertical * Time.deltaTime * 80), 0, 0); 
		}

		//// Left Right in the air  
		transform.Rotate (0, Time.deltaTime * 100 * rightleftsoft, 0, Space.World); 

		//Tilt multiplied with minus 1 to go into the right direction	
		transform.Rotate (0, 0, Time.deltaTime * 100 * (1.0f - rightleftsoftabs - diveblocker) * moveHorizontal * -1.0f); 		

		//------------------------------------------------ Pitch and Tilt calculations ------------------------------------------
		//variable rightleftsoft + rightleftsoftabs

		//	//Soft rotation calculation -----This prevents the airplaine to fly to the left while it is still tilted to the right
		if ((moveHorizontal <= 0) && (rotationz > 0) && (rotationz < 90))
			rightleftsoft = rotationz * 2.2f / 100 * -1; //to the left
		if ((moveHorizontal >= 0) && (rotationz > 270))
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
		if ((rotationz < 180) && (moveHorizontal > 0))
			transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);
		if ((rotationz > 180) && (moveHorizontal < 0))
			transform.Rotate (0, 0, rightleftsoft * Time.deltaTime * 80);



		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {

			//Rotate back in z axis general, limited by no horizontal button pressed
			if (!Input.GetButton ("Horizontal")) 
			{
				if ((rotationz < 135))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * -100);
				if ((rotationz > 225))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * 100);
			}

			//Rotate back X axis
			if (!Input.GetButton ("Vertical")) {
				if ((rotationx > 0) && (rotationx < 180))
					transform.Rotate (Time.deltaTime * -50, 0, 0);
				if ((rotationx > 0) && (rotationx > 180))
					transform.Rotate (Time.deltaTime * 50, 0, 0);
			}

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller || gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller || gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			//Rotate back in z axis general, limited by no horizontal button pressed
			if (moveHorizontal == 0.0f) 
			{
				if ((rotationz < 135))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * -100);
				if ((rotationz > 225))
					transform.Rotate (0, 0, rightleftsoftabs * Time.deltaTime * 100);
			}

			//Rotate back X axis
			if (moveVertical == 0.0f) {
				if ((rotationx > 0) && (rotationx < 180))
					transform.Rotate (Time.deltaTime * -50, 0, 0);
				if ((rotationx > 0) && (rotationx > 180))
					transform.Rotate (Time.deltaTime * 50, 0, 0);
			}
		}


		//----------------------------Speed driving and flying ----------------------------------------------------------------

		// Speed
		transform.Translate (0, 0, speed / 20f * Time.deltaTime);
		//We need a minimum speed limit in the air. We limit again with the groundtrigger.triggered variable

		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {
			
			// Input Accellerate and deccellerate in the air
			if ((Input.GetKey ("z")) && (speed < maxSpeed))////800
				speed += Time.deltaTime * 240;
			if ((Input.GetKey ("x")) && (speed > minSpeed))//(speed > 600))  ///600
				speed -= Time.deltaTime * 240;
			
		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
			
			// Input Accellerate and deccellerate in the air
			if ((Input.GetAxis ("PS4_R2") > 0.0f) && (speed < maxSpeed))////800
				
				speed += Time.deltaTime * 240;
			if ((Input.GetAxis ("PS4_L2") > 0.0f) && (speed > minSpeed))//(speed > 600))  ///600
				speed -= Time.deltaTime * 240;

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller) {

			// Input Accellerate and deccellerate in the air
			if ((Input.GetAxis ("360_LeftThumbTrigger") > 0.0f) && (speed < maxSpeed))////800

				speed += Time.deltaTime * 240;
			if ((Input.GetAxis ("360_RightThumbTrigger") > 0.0f) && (speed > minSpeed))//(speed > 600))  ///600
				speed -= Time.deltaTime * 240;

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			// Input Accellerate and deccellerate in the air
			if ((Input.GetAxis ("360PC_ThumbTriggers") > 0.0f) && (speed < maxSpeed))////800

				speed += Time.deltaTime * 240;
			if ((Input.GetAxis ("360PC_ThumbTriggers") > 0.0f) && (speed > minSpeed))//(speed > 600))  ///600
				speed -= Time.deltaTime * 240;

		}


			

		if (speed < 0)
			speed = 0; //floatingpoint calculations makes a fix necessary so that speed cannot be below zero

		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {
			//Another speed floatingpoint fix:
			if ((!Input.GetKey ("z")) && (!Input.GetKey ("x")) && (speed > (defaultSpeed - 5f)) && (speed < (defaultSpeed + 5f))) ////695 === 705
				speed = defaultSpeed;
		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {
			//Another speed floatingpoint fix:
			if (((Input.GetAxis ("PS4_R2") <= 0.0f) && (Input.GetAxis ("PS4_L2") <= 0.0f)) && (speed > (defaultSpeed - 5f)) && (speed < (defaultSpeed + 5f))) ////695 === 705
				speed = defaultSpeed;
		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller) {
			//Another speed floatingpoint fix:
			if (((Input.GetAxis ("360_RightThumbTrigger") <= 0.0f) && (Input.GetAxis ("360_LeftThumbTrigger") <= 0.0f)) && (speed > (defaultSpeed - 5f)) && (speed < (defaultSpeed + 5f))) ////695 === 705
				speed = defaultSpeed;
		}else if (gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {
			//Another speed floatingpoint fix:
			if ((Input.GetAxis ("360PC_ThumbTriggers") <= 0.0f) && (speed > (defaultSpeed - 5f)) && (speed < (defaultSpeed + 5f))) ////695 === 705
				speed = defaultSpeed;
		}
			

		//----------------------------------------------------- Uplift  ----------------------------------------------------------------------

		//When we don`t accellerate or deccellerate we want to go to a neutral speed in the air. With this speed it has to stay at a neutral height. 
		//Above this value the airplane has to climb, with a lower speed it has to  sink. That way we are able to takeoff and land then.

		if (gameManagerScript.controlsType == GameManager.ControlsType.Keyboard) {

			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. speed is between Maximum 800, minimum 600, so go back to 700
			if (!Input.GetKey ("z") && (!Input.GetKey ("x")) && (speed > (minSpeed - 20f)) && (speed < defaultSpeed)) //595 === 700
				speed += Time.deltaTime * 240.0f;
			if (!Input.GetKey ("z") && (!Input.GetKey ("x")) && (speed > (minSpeed - 20f)) && (speed > defaultSpeed))// 595 === 700
				speed -= Time.deltaTime * 240.0f;

		} else if (gameManagerScript.controlsType == GameManager.ControlsType.PS4_Controller) {

			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. speed is between Maximum 800, minimum 600, so go back to 700
			if (((Input.GetAxis ("PS4_R2") <= 0.0f) && (Input.GetAxis ("PS4_L2") <= 0.0f)) && (speed > (minSpeed - 20f)) && (speed < defaultSpeed)) //595 === 700
				speed += Time.deltaTime * 240.0f;

			if (((Input.GetAxis ("PS4_R2") <= 0.0f) && (Input.GetAxis ("PS4_L2") <= 0.0f)) && (speed > (minSpeed - 20f)) && (speed > defaultSpeed))// 595 === 700
				speed -= Time.deltaTime * 240.0f;

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.Xbox_Controller) {

			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. speed is between Maximum 800, minimum 600, so go back to 700
			if (((Input.GetAxis ("360_RightThumbTrigger") <= 0.0f) && (Input.GetAxis ("360_LeftThumbTrigger") <= 0.0f)) && (speed > (minSpeed - 20f)) && (speed < defaultSpeed)) //595 === 700
				speed += Time.deltaTime * 240.0f;

			if (((Input.GetAxis ("360_RightThumbTrigger") <= 0.0f) && (Input.GetAxis ("360_LeftThumbTrigger") <= 0.0f)) && (speed > (minSpeed - 20f)) && (speed > defaultSpeed))// 595 === 700
				speed -= Time.deltaTime * 240.0f;

		}else if (gameManagerScript.controlsType == GameManager.ControlsType.XboxPC_Controller) {

			// Neutral speed at 700
			//This code resets the speed to 700 when there is no acceleration or deccelleration. speed is between Maximum 800, minimum 600, so go back to 700
			if ((Input.GetAxis ("360PC_ThumbTriggers") <= 0.0f) && (speed > (minSpeed - 20f)) && (speed < defaultSpeed)) //595 === 700
				speed += Time.deltaTime * 240.0f;

			if ((Input.GetAxis ("360PC_ThumbTriggers") <= 0.0f) && (speed > (minSpeed - 20f)) && (speed > defaultSpeed))// 595 === 700
				speed -= Time.deltaTime * 240.0f;

		}


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


}