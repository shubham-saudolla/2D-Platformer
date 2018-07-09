/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
	public Transform[] backgrounds;
	private float[] parallaxScales;			//proportions of the camera movements to move the backgrounds
	public float smoothing = 1f; 			//how smooth the parallxing will be
	private Transform cam;
	private Vector3 previousCamPosition; 	//the position of camera in the previous frame

	//called before Start()
	void Awake()
	{
		cam = Camera.main.transform;
	}

	void Start()
	{
		//the previous frames and currenty frame's camera position
		previousCamPosition = cam.position;

		parallaxScales = new float[backgrounds.Length];
		
		for(int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}
	}
	
	void Update()
	{
		for(int i = 0; i < backgrounds.Length; i++)
		{
			//the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
			float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

			//set a target position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//create a target position which is the background's current position with it's target's x position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//fade between current positions and the target position using lerp
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		
		//set the previousCamPos to the current camera position at the end of the frame
		previousCamPosition = cam.position;
	}
}
