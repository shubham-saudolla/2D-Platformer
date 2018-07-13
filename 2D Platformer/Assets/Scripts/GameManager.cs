/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject playerPrefab;
	public Transform spawnPoint;
	public GameObject spawnPrefab;
	public float spawnDelay = 2f;
	
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
	
	public void KillPlayer(Player player)
	{
		Destroy(player.gameObject);
		
		StartCoroutine(RespawnPlayer());
	}

	public void KillEnemy(Enemy enemy)
	{
		Destroy(enemy.gameObject);
	}

	public IEnumerator RespawnPlayer()
	{
		AudioManager.instance.Play("SpawnCountdown");
		
		yield return new WaitForSeconds(spawnDelay);

		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		
		GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		Destroy(clone, 3f);
	}
}
