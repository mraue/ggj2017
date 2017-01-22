using GGJ2017.CrossContext.Services;
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

        private BarLogo barBarLogo;

        State _state;

        DateTime _gameStarted;

        public static event Action OnGameStarted;
        public static event Action OnMainMenuStarted;

        void Awake()
        {
            Setup();
        }

        void Setup()
        {
            startScreenController.onStartGame += OnStartGame;
            gameFinishedController.onContinue += OnContinue;

            SceneManager.sceneLoaded += OnSceneLoaded;

            //Log.logHandler = Debug.Log;
            AudioService.instance.Setup();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == SCENE_ID_BAR_INTERIOR)
            {
                barBarLogo = GameObject.FindGameObjectWithTag("BarLogo").GetComponent<BarLogo>();
            }
            else if (scene.name == SCENE_ID_BAR_PLAYERS)
            {
                gameLoopManager = GameObject.FindGameObjectWithTag("GameLoopManager").GetComponent<GameLoopManager>();
            }
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
                double secondsRunning = DateTime.Now.Subtract(_gameStarted).TotalSeconds;

                if (secondsRunning < GAME_DURATION)
                {
                    timerViewController.clockFill.fillAmount = Remap((float)secondsRunning, 0, GAME_DURATION, 0, 1);
                }
                else
                {
                    OnGameFinished();
                }
            }
        }
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        void OnContinue()
        {
            if (_state == State.EndScreen)
            {
                gameFinishedController.Hide();
                startScreenController.Show();
                barBarLogo.OnMainMenu();

                _state = State.StartScreen;
                if (OnMainMenuStarted != null) OnMainMenuStarted.Invoke();
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

                gameLoopManager.Reset();
                gameLoopManager.acceptingInput = true;
                barBarLogo.OnMainMenu();
                barBarLogo.OnGameStart();

                if (OnGameStarted != null) OnGameStarted.Invoke();
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