using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {
	public int offsetX = 2;
	//offset so that camera doesnt move ahead of end of background before hte buddy is made

	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false; //if obj not tilaable
	private float spriteWidth = 0f; //width of our element

	private Camera cam;
	private Transform myTransform;


	void Awake(){
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (hasALeftBuddy == false || hasARightBuddy == false) {
			// calc camera's extend, meaning half the width of what the camera can see in world coords
			float camHoriExtend = cam.orthographicSize * Screen.width / Screen.height;

			//calc x pos where cam can see edge of sprite
			float edgeVisiblePosRight = (myTransform.position.x + spriteWidth/2) - camHoriExtend;
			float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth/2) + camHoriExtend;

			//checking to see if we can see edge of element
			if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasARightBuddy == false){
				makeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePosLeft + offsetX && hasALeftBuddy == false){
				makeNewBuddy(-1);
				hasALeftBuddy = true;
			}

		}
		
	}

	void makeNewBuddy(int rightOrLeft){
		// calc new pos for new buddy
		Vector3 newPos = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		// instantiating new buddy and storing in var
		Transform newbuddy = Instantiate (myTransform, newPos, myTransform.rotation) as Transform;

		//allow us to invert the element not tilable, so it reverses x so that it repeats seamlessly
		if (reverseScale == true) {
			newbuddy.localScale = new Vector3 (newbuddy.localScale.x * -1, newbuddy.localScale.y, newbuddy.localScale.z);
		}

		newbuddy.parent = myTransform.parent;

		if (rightOrLeft > 0) {
			newbuddy.GetComponent<Tiling> ().hasALeftBuddy = true;
		} else {
			newbuddy.GetComponent<Tiling> ().hasARightBuddy = true;
		}
	}
}
