﻿/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public float fireRate = 0;
	public int damage = 25;
	public LayerMask whatToHit;
	public Transform BulletTrailPrefab;
	public Transform muzzleFlashPrefab;
	public Transform HitPrefab;
	private float _timeToFire = 0f;
	private float _timeToSpawnEffect = 0f;
	public float effectSpawnRate = 10f;
	public float weaponRaycastRange = 100f;
	private Transform _firePoint;
	public float camShakeAmount = 0.1f;

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
			
			Enemy enemy = hit.collider.GetComponent<Enemy>();

			if(enemy != null)
			{
				enemy.DamageEnemy(damage);
				// Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage.");
			}
		}

		if(Time.time >= _timeToSpawnEffect)
		{
			Vector3 hitPos;
			Vector3 hitNormal;

			if(hit.collider == null)
			{
				hitPos = (mousePosition - firePointPosition)*100;
				hitNormal = new Vector3(999, 999, 999);
			}
			else
			{
				hitPos= hit.point;
				hitNormal = hit.normal;
			}

			Effect(hitPos, hitNormal);

			_timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}	
	}

	void Effect(Vector3 hitPos, Vector3 hitNormal)
	{
		Transform trail = Instantiate(BulletTrailPrefab, _firePoint.position, _firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer>();
		if(lr != null)
		{
			//set position
			lr.SetPosition(0, _firePoint.position);
			lr.SetPosition(1, hitPos);
		}

		Destroy(trail.gameObject, 0.04f);

		if(hitNormal != new Vector3(999, 999, 999))
		{
			Transform hitParticles = Instantiate(HitPrefab, hitPos,Quaternion.FromToRotation(Vector3.up, hitNormal)) as Transform;

			Destroy(hitParticles.gameObject, 1f);
		}

		Transform clone = Instantiate(muzzleFlashPrefab, _firePoint.position, _firePoint.rotation) as Transform;	//typecasting
		clone.parent = _firePoint;
		float size = Random.Range(0.6f, 0.9f);
		clone.localScale = new Vector3(size, size, size);
		Destroy(clone.gameObject, 0.03f);

		//shake the camera
		CameraShake.instance.Shake(camShakeAmount, 0.2f);
	}
}