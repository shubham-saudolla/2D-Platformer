/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject playerPrefab;
	public Transform spawnPoint;
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

	public IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds(spawnDelay);
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}
}
