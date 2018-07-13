/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour
{
	//what to chase
	public Transform target;
	
	//how many times we will update our path per second
	public float updateRate = 2f;

	public string playerTag;

	//caching
	private Seeker _seeker;
	private Rigidbody2D _rb;

	//the calculated path
	public Path path;

	//the AI's speed per second
	public float speed =  300f;

	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathHasEnded = false;

	//the max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3f;

	//waypoint we are currently moving towards
	private int _currentWaypoint;

	private bool _searchingForPlayer = false;

	void Start()
	{
		_seeker = GetComponent<Seeker>();
		_rb = GetComponent<Rigidbody2D>();

		if(target == null)
		{
			if(!_searchingForPlayer)
			{
				_searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}

		//start a new path to the target position, return the result to the OnComplete method
		_seeker.StartPath(transform.position, target.position, onPathComplete);

		StartCoroutine(UpdatePath());
	}

	public IEnumerator SearchForPlayer()
	{
		GameObject sResult =  GameObject.FindGameObjectWithTag(playerTag);

		if(sResult == null)
		{
			yield return new WaitForSeconds((0.5f));

			StartCoroutine(SearchForPlayer());
		}
		else
		{
			target = sResult.transform;
			_searchingForPlayer = false;
			StartCoroutine(UpdatePath());
			yield return false;
		}
	}

	public void onPathComplete(Path p)
	{
		Debug.Log("Did it have an error? " + p.error);

		if(!p.error)
		{
			path = p;

			_currentWaypoint = 0;
		}
	}

	public IEnumerator UpdatePath()
	{
		if(target == null)
		{
			if(!_searchingForPlayer)
			{
				_searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			yield return false;
		}

		//start a new path to the target position, return the result to the OnComplete method
		_seeker.StartPath(transform.position, target.position, onPathComplete);

		yield return new WaitForSeconds( 1f/updateRate);

		StartCoroutine(UpdatePath());
	}

	void FixedUpdate()
	{
		if(target == null)
		{
			if(!_searchingForPlayer)
			{
				_searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}

		//TODO: always look at player

		if(_currentWaypoint >= path.vectorPath.Count)
		{
			if(pathHasEnded)
			{
				return;
			}

			Debug.Log("End of path reached.");
			pathHasEnded = true;
			return;
		}

		pathHasEnded = false;

		//direction to the next waypoint
		Vector3 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;

		dir *= speed*Time.fixedDeltaTime;

		//move the AI
		_rb.AddForce(dir, fMode);

		float dist = Vector3.Distance(transform.position, path.vectorPath[_currentWaypoint]);
		if(dist < nextWaypointDistance)
		{
			_currentWaypoint++;
			return;
		}
	}

}
