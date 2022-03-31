using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameState : Singleton<PauseGameState>
{
	bool gamePaused;

    protected override void Awake()
    {
		base.Awake();
		AddEventListeners();
    }
    protected override void OnDestroy()
	{
		base.OnDestroy();
		RemoveEventListeners();
	}

	private void AddEventListeners()
	{
		EventBroker.PauseGame += OnPauseGame;
		EventBroker.UnpauseGame += OnUnpauseGame;
	}

	void RemoveEventListeners()
	{
		EventBroker.PauseGame -= OnPauseGame;
		EventBroker.UnpauseGame -= OnUnpauseGame;
	}

	private void OnPauseGame()
	{
		gamePaused = true;
		Time.timeScale = 0;
	}

	private void OnUnpauseGame()
	{
		gamePaused = false;
		Time.timeScale = 1;
	}

	public bool GamePaused
    {
		get { return gamePaused; }
    }
}
