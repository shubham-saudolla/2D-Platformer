/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
	public int offsetX = 2;					//the offset to prevent any weird errors

	//these are used for checking if we need to instantiate stuff
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false; 		//used if the object is not tilable

	public float spriteWidth = 0f;			//width of the element

	private Camera cam;
	private Transform myTransform;

	void Awake()
	{
		cam = Camera.main;
		myTransform = this.transform;
	}

	void Start()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	void Update()
	{
		if(hasALeftBuddy == false || hasARightBuddy == false)
		{
			//calculate the camera extent (half the width) of what the camera can see in world cordinates
			float camHorizontalExtent = cam.orthographicSize * Screen.width/Screen.height;

			//calculate the x position where the camera can see the edge of the sprite 
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtent;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtent;

			//checking whether we can see the edge of the element and then calling the MakeBudyy() if we can
			if((cam.transform.position.x >= (edgeVisiblePositionRight - offsetX)) && (hasARightBuddy == false))
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}

			if((cam.transform.position.x <= (edgeVisiblePositionLeft + offsetX)) && (hasALeftBuddy == false))
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	}

	void MakeNewBuddy(int rightOrLeft)
	{
		Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		
		Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform; //typecasting and instantiation

		if(reverseScale == true) //the object is not tilable
		{
			newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = myTransform.parent;

		if(rightOrLeft > 0)
		{
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}
}
