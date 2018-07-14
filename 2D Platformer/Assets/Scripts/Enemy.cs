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
		public int maxHealth = 100;
		private int _curHealth;

		public int curHealth //getter, setter
		{
			get{ return _curHealth;}
			set{ _curHealth = Mathf.Clamp(value, 0, maxHealth); }
		}

		public int damage = 40;

		public void init()
		{
			curHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats();

	[Header("Optional:")]
	[SerializeField]
	private StatusIndicator statusIndicator;
		
	void Start()
	{
		stats.init();

		if(statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	public void DamageEnemy(int damage)
	{
		stats.curHealth -= damage;

		if(stats.curHealth <= 0)
		{
			GameManager.instance.KillEnemy(this);
		}

		if(statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	void OnCollisionEnter2D(Collision2D colInfo)
	{
		Player _player = colInfo.collider.GetComponent<Player>();

		if(_player != null)
		{
			_player.DamagePlayer(stats.damage);
			GameManager.instance.KillEnemy(this);
		}
	}
}
