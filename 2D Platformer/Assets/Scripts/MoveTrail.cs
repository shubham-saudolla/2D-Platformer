/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
	public float moveSpeed = 230;

	void Update()
	{
		transform.Translate(Vector3.right * Time.deltaTime * moveSpeed); 	//this is useful w=for moving a gameobject without a rigidbody
		Destroy(this.gameObject, 1);
	}
}
