/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public float fireRate = 0;
	public float damage = 10;
	public LayerMask whatToHit;

	private float _timeToFire = 0f;
	public float weaponRaycastRange = 100f;

	private Transform _firePoint;

	void Awake()
	{
		_firePoint = transform.Find("FirePoint");

		if(_firePoint == null)
		{
			Debug.LogError("FirePoint not found.");
		}
	}
	
	void Update()
	{
		if(fireRate == 0)								//if single burst
		{
			if(Input.GetMouseButtonDown(0))
			{
				Shoot();
			}
		}
		else
		{
			if(Input.GetMouseButtonDown(0) && Time.time > _timeToFire)
			{
				_timeToFire = Time.time + 1/fireRate;
				Shoot();
			}
		}
	}

	void Shoot()
	{
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y );

		Vector2 firePointPosition = new Vector2(_firePoint.position.x, _firePoint.position.y);

		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, weaponRaycastRange, whatToHit);

		Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*weaponRaycastRange, Color.cyan);

		if(hit.collider != null)
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Debug.Log("We hit" + hit.collider.name + " and did " + damage + " damage.");
		}	
	}
}
