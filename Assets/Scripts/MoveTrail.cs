using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {
	public int moveSpeed = 100;
	
	// Update is called once per frame
	void Update () {
		//to move objs over time, without a rigid body in one direction
		transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 1);
	}
}
