/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[System.Serializable]
	public class PlayerStats
	{
		public int maxHealth = 100;

		private int _curHealth;

		public int curHealth //getter, setter
		{
			get{ return _curHealth; }
			set{ _curHealth = Mathf.Clamp(value, 0, maxHealth); }
		}

		public void init()
		{
			curHealth = maxHealth;
		}
	}

	public PlayerStats stats = new PlayerStats();		//an instance of the PlayerStats class

	public int fallBoundary = -20;

	[SerializeField]
	private StatusIndicator _statusIndicator;

	//there were multiple instances of the player prefab, thus implemented a singleton pattern
	public static Player instance;

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

	void Start()
	{
		stats.init();

		if(_statusIndicator == null)
		{
			Debug.LogError("No statusindicator reference on player");
		}
		else
		{
			_statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}

	void Update()
	{
		if(transform.position.y <= fallBoundary)
		{
			DamagePlayer(999);
		}
	}

	public void DamagePlayer(int damage)
	{
		stats.curHealth -= damage;

		if(stats.curHealth <= 0)
		{
			GameManager.instance.KillPlayer(this);
		}

		_statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
	}
}
