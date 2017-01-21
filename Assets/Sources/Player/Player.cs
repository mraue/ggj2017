using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using GGJ2017.CrossContext.Services;

namespace GGJ2017.Game
{
	class Player : MonoBehaviour
	{
		const float WAVE_DURATION = 3f;
		const float SERVE_DRINK_DURATION = 3f;

		public enum State
		{
			None = 0,
			Idle = 1,
			Waving = 2,
			DrinkServing = 3,
		}

		public KeyCode assignedKey;

		public int drinksServed;

		public DateTime lastDrinkServed;

		public int id;// Zero base, player one has id=0

		public State state { get { return _state; } }
		State _state;

		void Awake()
		{
			_state = State.Idle;
		}

		public void ShouldWave()
		{
			Log.InfoFormat("Player {0} should wave", assignedKey);
			if (_state == State.Idle)
			{
				Wave();
			}
		}

		void Wave()
		{
			Log.InfoFormat("Player {0} starts waving", assignedKey);

			_state = State.Waving;

			// Start animation

			AudioService.instance.Play(GetOrderDrinkAudioId(id));

			Invoke("OnWavingFinished", WAVE_DURATION);
		}

		void OnWavingFinished()
		{
			Log.InfoFormat("Player {0} has finished waving", assignedKey);
			_state = State.Idle;
		}

		public void ServeDrink()
		{
			drinksServed += 1;
			lastDrinkServed = DateTime.Now;

			_state = State.DrinkServing;

			Log.InfoFormat("Player {0} gets served a drink (total={1})", assignedKey, drinksServed);

			Invoke("OnDrinkServingFinished", WAVE_DURATION);
		}

		void OnDrinkServingFinished()
		{
			Log.InfoFormat("Player {0} has finished getting a drink served", assignedKey);
			_state = State.Idle;
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