using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {
	// Array to store elements we need to apply parallaxing to
	public Transform[] backgrounds;
	private float[] parallaxScales; //Proportion of camera's movt to move bgs by
	public float smoothing = 1f; 	//how smooth parallax is going to be. Must be above  0.
	private Transform cam; 			//Ref to main cameras transform
	private Vector3 previousCamPos; //pos of cam in prev frame

	// Called before Start (). Great for references.
	void Awake () {
		cam = Camera.main.transform;

	}

	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length ; i++) {
			parallaxScales [i] = backgrounds [i].position.z * -1;
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < backgrounds.Length; i++) {
			//parallax is the oppposite of the camera movement because the prev frame multiplied by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			// set target x pos which is current + parallax
			float backgroundPosX = backgrounds[i].position.x + parallax;

			//create a target position which is the background's current position with its target X position
			Vector3 backgroundTargetPos = new Vector3(backgroundPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//fade b/w current and atarget pos using lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing*Time.deltaTime);
		}

		//set prev campos as cams pos at end of frame
		previousCamPos = cam.position;
		
	}
}
