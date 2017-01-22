using GGJ2017.CrossContext.Services;
using GGJ2017.Shared.Logging;
using System;
using UnityEngine;

namespace GGJ2017.Game
{
    class Player : MonoBehaviour
    {
        const float WAVE_DURATION = 3.0f;
        const float SERVE_DRINK_DURATION = 3.0f;

        public enum State
        {
            None = 0,
            Idle = 1,
            Waving = 2,
            DrinkServing = 3,
        }

        public KeyCode assignedKey;

        public int drinksServed;
        public int wavesTillServing;
        private int sucessfullWaves;

        public DateTime lastDrinkServed;

		public PlayerScoreView scoreView;

		public TextMesh playerInfo;

        public int id;// Zero base, player one has id=0

		public event Action<int> onStartedWaving = delegate { };

        public State state { get { return _state; } }
        State _state;

        private Animator anim;

        void Awake()
        {
            _state = State.Idle;
            anim = GetComponent<Animator>();
			playerInfo.text = assignedKey.ToString();
        }

		public void Reset()
		{
			drinksServed = 0;
			scoreView.SetScoreAmount(0);
			lastDrinkServed = DateTime.MinValue;
			_state = State.Idle;
			playerInfo.text = assignedKey.ToString();
		}

        public void ShouldWave()
        {
            if (_state == State.Idle)
            {
                Wave();
            }
        }

        void Wave()
        {
            Log.InfoFormat("Player {0} starts waving", assignedKey);

            _state = State.Waving;
			playerInfo.text = "";

            // Start animation
            anim.SetTrigger("Wave");

            AudioService.instance.Play(GetOrderDrinkAudioId(id));

			onStartedWaving(id);

            Invoke("OnWavingFinished", WAVE_DURATION);
        }

        void OnWavingFinished()
        {
            Log.InfoFormat("Player {0} has finished waving", assignedKey);

            if (sucessfullWaves >= wavesTillServing)
            {
                sucessfullWaves = 0;
                ServeDrink();
            }
            else
            {
                _state = State.Idle;
                anim.SetTrigger("Idle");
            }

			playerInfo.text = assignedKey.ToString();
        }

        public void ServeDrink()
        {
            drinksServed++;

			scoreView.SetScoreAmount(drinksServed);

            lastDrinkServed = DateTime.Now;

            _state = State.DrinkServing;

            Log.InfoFormat("Player {0} gets served a drink (total={1})", assignedKey, drinksServed);

            Invoke("OnDrinkServingFinished", SERVE_DRINK_DURATION);

			playerInfo.text = "";
        }

        void OnDrinkServingFinished()
        {
            Log.InfoFormat("Player {0} has finished getting a drink served", assignedKey);
            _state = State.Idle;
            anim.SetTrigger("Idle");
			playerInfo.text = assignedKey.ToString();
        }

        AudioId GetOrderDrinkAudioId(int playerId)
        {
            switch (playerId)
            {
                case 0:
                default:
                    return AudioId.WaveCustomer01;
                case 1:
                    return AudioId.WaveCustomer02;
                case 2:
                    return AudioId.WaveCustomer03;
                case 3:
                    return AudioId.WaveCustomer04;
                case 4:
                    return AudioId.WaveCustomer05;
            }
        }
    }
}