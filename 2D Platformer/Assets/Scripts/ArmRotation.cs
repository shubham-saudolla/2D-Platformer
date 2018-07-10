/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{	
	public int rotationOffset = 0;

	void Update()
	{
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize();							//normalizing the vector
		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //find the angle in degrees
		transform.rotation = Quaternion.Euler(0, 0, rotZ + rotationOffset);
	}
}
