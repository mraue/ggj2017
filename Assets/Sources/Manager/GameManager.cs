﻿using GGJ2017.CrossContext.Services;
using GGJ2017.Shared.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ2017.Game
{
    class GameManager : MonoBehaviour
    {
        const string SCENE_ID_BAR_PLAYERS = "BartenderAndCustomers";
        const string SCENE_ID_BAR_INTERIOR = "BarInterior";

        const float GAME_DURATION = 60f;

        public enum State
        {
            None = 0,
            StartScreen,
            Running,
            EndScreen,
        }

        List<string> _scenesToLoad = new List<string> { SCENE_ID_BAR_INTERIOR, SCENE_ID_BAR_PLAYERS };

        public GameLoopManager gameLoopManager;

        public StartScreenController startScreenController;
        public GameFinishedController gameFinishedController;

        public TimerViewController timerViewController;

        public ResultsView resultsView;

        State _state;

        DateTime _gameStarted;

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
            foreach (var sceneId in _scenesToLoad)
            {
                SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
            }

            _state = State.StartScreen;

            AudioService.instance.SetBackground(AudioId.StartScreenBackground);
        }

        void Update()
        {
            if (_state == State.Running)
            {
                var secondsRunning = DateTime.Now.Subtract(_gameStarted).TotalSeconds;

                if (secondsRunning < GAME_DURATION)
                {
                    timerViewController.label.text = string.Format("{0:D2}", (int)(GAME_DURATION - secondsRunning) + 1);
                }
                else
                {
                    OnGameFinished();
                }
            }
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
                AudioService.instance.SetBackground(AudioId.BarBackground);

                startScreenController.Hide();
                timerViewController.Show();

                _state = State.Running;
                _gameStarted = DateTime.Now;

                gameLoopManager = GameObject.FindGameObjectWithTag("GameLoopManager").GetComponent<GameLoopManager>();
                gameLoopManager.Reset();
                gameLoopManager.acceptingInput = true;
            }
            else
            {
                Log.Warning("[GameManager::OnStartGame] We are not on the start screen");
            }
        }

        public void OnGameFinished()
        {
            gameLoopManager.acceptingInput = false;

            resultsView.Show(gameLoopManager.GetHighscoreData());

            timerViewController.Hide();
            gameFinishedController.Show();

            _state = State.EndScreen;
        }
    }
}