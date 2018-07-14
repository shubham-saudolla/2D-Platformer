/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Camera cam;

	public float magnitude = 0;

	public static CameraShake instance;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
	{
		if(cam == null)
		{
			cam = Camera.main;
		}
	}

	public void Shake(float mag, float duration)
	{
		magnitude = mag;

		InvokeRepeating("BeginShake", 0, 0.1f);
		InvokeRepeating("StopShake", duration, 0);
	}

	void BeginShake()
	{
		if(magnitude > 0)
		{
			Vector3 camPos = cam.transform.position;

			float offsetX = Random.value * magnitude * 2 - magnitude;
			float offsetY = Random.value * magnitude * 2 - magnitude;

			camPos.x += offsetX;
			camPos.y += offsetY;

			cam.transform.position = camPos;
		}
	}

	void StopShake()
	{
		CancelInvoke("BeginShake");
		cam.transform.localPosition = Vector3.zero;
	}
}
