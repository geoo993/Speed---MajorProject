using UnityEngine;
using System.Collections;

public class Controllerr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		if (Input.GetButton ("PS4_PSN")) {
			print("Pressed PSN");
		}

	}
}
