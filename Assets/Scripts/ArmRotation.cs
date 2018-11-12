using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {
	public int rotOffset = 90;
	
	// Update is called once per frame
	void Update () {
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position; //diff of player from mouse pos
		difference.Normalize (); 		//Normalizing, ie. sum of vector will be equal to 1

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
		//finds angle in degrees

		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotOffset);

	}
}
