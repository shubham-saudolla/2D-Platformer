/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[System.Serializable]
	public class EnemyStats
	{
		public int health = 100;
	}

	public EnemyStats stats = new EnemyStats();		//an instance of the PlayerStats class

	public void DamageEnemy(int damage)
	{
		stats.health -= damage;

		if(stats.health <= 0)
		{
			GameManager.instance.KillEnemy(this);
		}
	}
}
