using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using GGJ2017.CrossContext.Services;
using UnityEngine.SceneManagement;

namespace GGJ2017.Game
{
	class GameManager : MonoBehaviour
	{
		const string BAR_SCENE_ID = "bar";

		public enum State
		{
			None = 0,
			StartScreen,
			Running,
			EndScreen,
		}

		public GameLoopManager gameLoopManager;

		public StartScreenController startScreenController;
		public GameFinishedController gameFinishedController;

		State _state;

		void Awake()
		{
			Setup();
		}

		void Setup()
		{
			startScreenController.onStartGame += OnStartGame;
			gameFinishedController.onContinue += OnContinue;

			Log.logHandler = Debug.Log;
			AudioService.instance.Setup();
		}

		void Start()
		{
			SceneManager.LoadScene(BAR_SCENE_ID, LoadSceneMode.Additive);

			_state = State.StartScreen;

			AudioService.instance.SetBackground(AudioId.BarBackground);
		}

		void OnContinue()
		{
			if (_state == State.EndScreen)
			{
				gameFinishedController.Hide();
				startScreenController.Show();

				_state = State.StartScreen;
			}
			else
			{
				Log.Warning("[GameManager::OnContinue] We are not on end screen");
			}
		}

		public void OnStartGame()
		{
			if (_state == State.StartScreen)
			{
				startScreenController.Hide();

				_state = State.Running;
				gameLoopManager.acceptingInput = true;
			}
			else
			{
				Log.Warning("[GameManager::OnStartGame] We are not on the start screen");
			}
		}
	}
}