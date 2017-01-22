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

		const float STATE_IDLE_MINIMUM = 1f;
		const float STATE_IDLE_MAXIMUM = 2f;

		const float STATE_MOVING_MINIMUM = 1f;
		const float STATE_MOVING_MAXIMUM = 2f;

		public enum State
		{
			None = 0,
			Idle,
			Moving,
			Dancing,
			ServingCustomer,
		}

		public State state { get { return _state; } }
		public State _state;

		public GameObject head;
		public RotationLateUpdater rotationUpdater;

		public ActionTargets lookAtTargets;

		[Range(0f, 1.0f)]
		public float slerpInterpolation = 0.1f;
		public AnimationCurve slerpCurve;

		public ActionTargets moveToTargets;

		public List<GameObject> customers;

		public event Action<int> onServeDrink = delegate { };

		float _stateDuration;
		float _stateCurrentDuration;

		public void CustomerStartedWaving(int id)
		{	
			float dot = Vector3.Dot(GetCorrectedForwardRotation(), (customers[id].transform.position - transform.position).normalized);
			var angle = Mathf.Acos(dot) * 180f / Math.PI;
			Log.InfoFormat("[BarkeeperManager::CustomerStartedWaving] id={0}, dot={1}, angle={2}", id, dot, angle);
			onServeDrink(id);
		}

		private Vector3 GetCorrectedForwardRotation()
		{
			return head.transform.forward.normalized;
		}

		void Start()
		{
			InitializeNewTurn();
		}

		void Update()
		{
			UpdateState();
			UpdateLookingDirection();
		}

		void UpdateState()
		{
			_stateCurrentDuration += Time.deltaTime;

			if (_stateCurrentDuration > _stateDuration)
			{				
				switch (UnityEngine.Random.Range(0, 2))
				{
					default:
					case 0:
						_state = State.Idle;
						_stateDuration = UnityEngine.Random.Range(STATE_IDLE_MINIMUM, STATE_IDLE_MAXIMUM);
						break;
					case 1:
						_state = State.Moving;
						_stateDuration = UnityEngine.Random.Range(STATE_MOVING_MINIMUM, STATE_MOVING_MAXIMUM);

						moveToTargets.NextTarget();
						moveToTargets.currentDuration = 0f;
						moveToTargets.duration = _stateDuration;
						break;
						
				}
				_stateCurrentDuration = 0f;
			}
		}

		void UpdateLookingDirection()
		{
			if (_state != State.ServingCustomer)
			{
				lookAtTargets.currentDuration += Time.deltaTime;

				var oldRotation = head.transform.rotation;

				head.transform.LookAt(lookAtTargets.currentTarget.transform);
				head.transform.rotation = head.transform.rotation;// * Quaternion.Euler(0f, 90f, 0f);

				var newRotation = head.transform.rotation;

				float progress = lookAtTargets.currentDuration / lookAtTargets.duration;

				slerpInterpolation = slerpCurve.Evaluate(progress);

				var targetRotation = Quaternion.Slerp(oldRotation, newRotation, slerpInterpolation);

				rotationUpdater.newRotation = targetRotation;

				if (lookAtTargets.currentDuration >= lookAtTargets.duration)
				{
					InitializeNewTurn();
				}
			}
		}

		void InitializeNewTurn()
		{
			lookAtTargets.currentDuration = 0f;
			lookAtTargets.duration = UnityEngine.Random.Range(TURN_DURATION_MINIMUM, TURN_DURATION_MAXIMUM);
			lookAtTargets.NextTarget();
		}
	}
}