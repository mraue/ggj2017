using GGJ2017.CrossContext.Services;
using GGJ2017.Shared.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GGJ2017.Shared.Extensions;

namespace GGJ2017.Game
{
	class BarkeeperManager : MonoBehaviour
	{
		const int LOOK_AT_TARGETS_MULTIPLIER = 2;
		const float TURN_DURATION_MINIMUM = 1f;
		const float TURN_DURATION_MAXIMUM = 2f;

		public enum State
		{
			None = 0,
			Idle,
			Walking,
			Dancing,
			ServingCustomer,
		}

		public State state { get { return _state; } }
		State _state;

		public GameObject head;

		public List<GameObject> lookAtTargets;

		[Range(0f, 1.0f)]
		public float slerpInterpolation = 0.1f;
		public AnimationCurve slerpCurve;

		List<GameObject> _currentLookAtTargets = new List<GameObject>();

		float _turnDuration;
		float _currentTurnDuration;

		Transform _currentLookAtTarget;

		void Start()
		{
			InitializeNewTurn();
		}

		void InitializeLookAtTargets()
		{
			_currentLookAtTargets.Clear();

			for (int i = 0; i < LOOK_AT_TARGETS_MULTIPLIER; i++)
			{
				_currentLookAtTargets.AddRange(lookAtTargets);
			}

			_currentLookAtTargets.Shuffle();
		}

		void LateUpdate()
		{
			UpdateLookingDirection();
		}

		void UpdateLookingDirection()
		{
			if (_state != State.ServingCustomer)
			{
				_currentTurnDuration += Time.deltaTime;

				var oldRotation = head.transform.rotation;

				head.transform.LookAt(_currentLookAtTarget);
				head.transform.rotation = head.transform.rotation * Quaternion.Euler(0f, 90f, 0f);

				var newRotation = head.transform.rotation;

				float progress = _currentTurnDuration / _turnDuration;

				slerpInterpolation = slerpCurve.Evaluate(progress);

				head.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, slerpInterpolation);

				if (_currentTurnDuration >= _turnDuration)
				{
					InitializeNewTurn();
				}
			}
		}

		void InitializeNewTurn()
		{
			_currentTurnDuration = 0f;

			_turnDuration = UnityEngine.Random.Range(TURN_DURATION_MINIMUM, TURN_DURATION_MAXIMUM);

			if (_currentLookAtTarget == null && _currentLookAtTargets.Count > 0)
			{
				_currentLookAtTarget = _currentLookAtTargets[0].transform;
			}
			else
			{
				if (_currentLookAtTargets.Count <= 1)
				{
					InitializeLookAtTargets();
				}

				_currentLookAtTargets.RemoveAt(0);

				_currentLookAtTarget = _currentLookAtTargets[0].transform;
			}
		}
	}
}