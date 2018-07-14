/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[System.Serializable]
	public enum SpawnState {Spawning, Waiting, Counting};

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave[] waves;
	public Transform[] spawnPoints;

	private int nextWave = 0;
	public float timeBetweenWaves = 5f;
	private float waveCountdown = 0f;

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.Counting;

	void Start()
	{
		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if(state == SpawnState.Waiting)
		{
			//check if enemies are still alive
			if(!EnemyIsAlive())
			{
				//begin a new round
				WaveCompleted();
				return;
			}
			else
			{
				return;
			}
		}

		if(waveCountdown <= 0)
		{
			if(state != SpawnState.Spawning)
			{
				//start spawning now
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave completed.");

		state = SpawnState.Counting;
		waveCountdown = timeBetweenWaves;

		if(nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("Completed all waves. Looping...");
		}
		else
		{
			nextWave++;
		}		
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;

		if(searchCountdown <= 0f)
		{
			searchCountdown = 1f;

			if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
			{
				return false;
			}
		}

		return true;
	}

	public IEnumerator SpawnWave(Wave _wave)
	{
		state = SpawnState.Spawning;

		//spawn
		Debug.Log("Spawning wave " + _wave.name);

		for(int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy);
			yield return new WaitForSeconds(1f / _wave.rate);
		}

		state = SpawnState.Waiting;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Debug.Log("Spawning enemy " + _enemy.name);

		if(spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn point reference.");
		}

		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}
}
