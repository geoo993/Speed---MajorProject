using UnityEngine;
using System.Collections;

public class Controllerr : MonoBehaviour {


	void Update () {
	
		//PS4Controls ();

		XboxControls ();
	}

	void PS4Controls()
	{

		////Axis
		print("Right Analog Horizontal: "+ Input.GetAxis ("PS4_RightAnalogHorizontal") +
				"    Right Analog Vertical: "+ Input.GetAxis ("PS4_RightAnalogVertical")+
				"    Horizontal: "+ Input.GetAxis ("Horizontal") +
				"    Vertical: "+ Input.GetAxis ("Vertical")
		);


		print("D-Pad Horizontal: "+ Input.GetAxis ("PS4_DirectionalPadHorizontal") +
			"    D-Pad Vertical: "+ Input.GetAxis ("PS4_DirectionalPadVertical")+
			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
			"    Vertical: "+ Input.GetAxis ("Vertical")
		);

		print("L2: "+ Input.GetAxis ("PS4_L2") +
			"    R2: "+ Input.GetAxis ("PS4_R2")+
			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
			"    Vertical: "+ Input.GetAxis ("Vertical")
		);

		print("Left Analog Horizontal: "+ Input.GetAxis ("PS4_LeftAnalogHorizontal") +
			"    Left Analog Vertical: "+ Input.GetAxis ("PS4_LeftAnalogVertical")+
			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
			"    Vertical: "+ Input.GetAxis ("Vertical")
		);


		////buttons
		if (Input.GetButton ("PS4_X")) 
		{
			print("Pressed X");
		}
		if (Input.GetButton ("PS4_O")) {
			print("Pressed O");
		}
		if (Input.GetButton ("PS4_Triangle")) {
			print("Pressed Triangle");
		}
		if (Input.GetButton ("PS4_Square")) {
			print("Pressed Square");
		}
		if (Input.GetButton ("PS4_Share")) {
			print("Pressed Share");
		}
		if (Input.GetButton ("PS4_Options")) {
			print("Pressed Options");
		}
		if (Input.GetButton ("PS4_R1")) {
			print("Pressed R1");
		}
		if (Input.GetButton ("PS4_R3")) {
			print("Pressed R3");
		}
		if (Input.GetButton ("PS4_L1")) {
			print("Pressed L1");
		}
		if (Input.GetButton ("PS4_L3")) {
			print("Pressed L3");
		}
		if (Input.GetButton ("PS4_Touch")) {
			print("Pressed Touch");
		}
//		if (Input.GetButton ("PS4_PSN")) {
//			print("Pressed PSN");
//		}

	}


	void XboxControls()
	{
//
//		print("Right Analog Horizontal: "+ Input.GetAxis ("360_RightAnalogHorizontal") +
//			"    Right Analog Vertical: "+ Input.GetAxis ("360_RightAnalogVertical")+
//			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//			"    Vertical: "+ Input.GetAxis ("Vertical")
//		);
//
//		print("Left Analog Horizontal: "+ Input.GetAxis ("360_LeftAnalogHorizontal") +
//			"    Left Analog Vertical: "+ Input.GetAxis ("360_LeftAnalogVertical")+
//			"    Horizontal: "+ Input.GetAxis ("Horizontal") +
//			"    Vertical: "+ Input.GetAxis ("Vertical")
//		);

		print("Left Thumb Trigger: "+ Input.GetAxis ("360_LeftThumbTrigger") +
			"    360_RightThumbTrigger: "+ Input.GetAxis ("360_RightThumbTrigger")+
			"    L2: "+ Input.GetAxis ("PS4_L2") +
			"    R2: "+ Input.GetAxis ("PS4_R2")
		);


//		if (Input.GetButton ("360_A")) {
//			print("Pressed 360_A");
//		}
//		if (Input.GetButton ("360_B")) {
//			print("Pressed 360_B");
//		}
//		if (Input.GetButton ("360_X")) {
//			print("Pressed X");
//		}
//		if (Input.GetButton ("360_Y")) {
//			print("Pressed 360_Y");
//		}
//		if (Input.GetButton ("360_Back")) {
//			print("Pressed 360_Back");
//		}
//		if (Input.GetButton ("360_Start")) {
//			print("Pressed 360_Start");
//		}
//
//		if (Input.GetButton ("360_LeftBumper")) {
//			print("Pressed 360_LeftBumper");
//		}
//		if (Input.GetButton ("360_RightBumper")) {
//			print("Pressed 360_RightBumper");
//		}
//
//
//		if (Input.GetButton ("360_LeftAnalogPress")) {
//			print("Pressed 360_LeftAnalogPress");
//		}
//		if (Input.GetButton ("360_RightAnalogPress")) {
//			print("Pressed 360_RightAnalogPress");
//		}
//		if (Input.GetButton ("360_DirectionalPadHorizontalUp")) {
//			print("Pressed 360_DirectionalPadHorizontalUp");
//		}
//		if (Input.GetButton ("360_DirectionalPadHorizontalDown")) {
//			print("Pressed 360_DirectionalPadHorizontalDown");
//		}
//
//		if (Input.GetButton ("360_DirectionalPadVerticalLeft")) {
//			print("Pressed 360_DirectionalPadVerticalLeft");
//		}
//		if (Input.GetButton ("360_DirectionalPadVerticalRight")) {
//			print("Pressed 360_DirectionalPadVerticalRight");
//		}
//			


	}


}
