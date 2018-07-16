/*
Copyright (c) Shubham Saudolla
https://github.com/shubham-saudolla
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
	[SerializeField]
	WaveSpawner spawner;

	[SerializeField]
	Animator waveAnimator;

	[SerializeField]
	Text waveCountdownText;

	[SerializeField]
	Text waveCountText;

	private WaveSpawner.SpawnState previousState;

	void Start()
	{
		if(spawner == null)
		{
			Debug.LogError("No spawner referenced.");
			this.enabled = false;
		}
		if(waveAnimator == null)
		{
			Debug.LogError("No animator referenced.");
			this.enabled = false;
		}
		if(waveCountdownText == null)
		{
			Debug.LogError("No countdown text referenced.");
			this.enabled = false;
		}
		if(waveCountText == null)
		{
			Debug.LogError("No wave count text referenced.");
			this.enabled = false;
		}
	}
	
	void Update()
	{
		switch(spawner.State)
		{
			case WaveSpawner.SpawnState.COUNTING:
				UpdateCountingUI();
				break;
			
			case WaveSpawner.SpawnState.SPAWNING:
				UpdateSpawningUI();
				break;
		}

		previousState = spawner.State;
	}

	void UpdateCountingUI()
	{
		if(previousState != WaveSpawner.SpawnState.COUNTING)
		{
			waveAnimator.SetBool("WaveIncoming", false);
			waveAnimator.SetBool("WavesCountdown", true);
			Debug.Log("Counting");
		}

		waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
	}

	void UpdateSpawningUI()
	{
		if(previousState != WaveSpawner.SpawnState.SPAWNING)
		{
			waveAnimator.SetBool("WavesCountdown", false);
			waveAnimator.SetBool("WaveIncoming", true);

			waveCountdownText.text = (spawner.NextWave).ToString();
			Debug.Log("Spawning");
		}
	}
}
