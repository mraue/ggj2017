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

		const float TURN_DURATION_MINIMUM = 0.7f;
		const float TURN_DURATION_MAXIMUM = 2f;

		const float STATE_IDLE_MINIMUM = 1f;
		const float STATE_IDLE_MAXIMUM = 2f;

		const float STATE_MOVING_MINIMUM = 1.5f;
		const float STATE_MOVING_MAXIMUM = 3f;

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

		public GameObject movementAnchor;

		public List<GameObject> customers;

		public float viewConeHalfAngle = 20f;

		public event Action<int> onServeDrink = delegate { };

		float _stateDuration;
		float _stateCurrentDuration;

		Quaternion _startingRotation;
		Vector3 _startingPositionMovement;
		Quaternion _startingRotationMovement;
		Quaternion _endRotationMovement;

		public void CustomerStartedWaving(int id)
		{	
			float dot = Vector3.Dot(head.transform.forward.normalized, (customers[id].transform.position - transform.position).normalized);
			var angle = Mathf.Acos(dot) * 180f / Math.PI;

			Log.InfoFormat("[BarkeeperManager::CustomerStartedWaving] id={0}, dot={1}, angle={2}", id, dot, angle);

			if (angle <= viewConeHalfAngle)
			{				
				AudioService.instance.Play(AudioId.ServeDrink);
				onServeDrink(id);
			}
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

						_startingPositionMovement = movementAnchor.transform.position;
						_startingRotationMovement = movementAnchor.transform.rotation;

						moveToTargets.NextTarget();
						moveToTargets.currentDuration = 0f;
						moveToTargets.duration = _stateDuration;

						break;
						
				}

				_stateCurrentDuration = 0f;
			}

			switch(_state)
			{
				case State.Moving:
					UpdateMovement();
					break;
			}
		}

		void UpdateMovement()
		{
			moveToTargets.currentDuration += Time.deltaTime;

			var progress = moveToTargets.currentDuration / moveToTargets.duration;
			var pos = movementAnchor.transform.position;
			pos.z = Vector3.Lerp(_startingPositionMovement, moveToTargets.currentTarget.transform.position, progress).z;

			movementAnchor.transform.position = pos;

			var movementDistance = (movementAnchor.transform.position - _startingPositionMovement).magnitude;

			if (movementDistance < 0.1f)
			{				
				return;
			}

			if (progress < 0.2f)
			{
				var currentRotation = movementAnchor.transform.rotation;
				movementAnchor.transform.LookAt(moveToTargets.currentTarget.transform);
				var finalRotation = movementAnchor.transform.rotation;
				movementAnchor.transform.rotation = Quaternion.Lerp(_startingRotationMovement, finalRotation, progress / 0.2f);
				_endRotationMovement = movementAnchor.transform.rotation;
			}
			else if(progress > 0.8f)
			{
				//movementAnchor.transform.rotation = Quaternion.Lerp(_endRotationMovement, _startingRotation, (progress - 0.8f) / 0.2f);
			}
		}

		void UpdateLookingDirection()
		{
			if (_state != State.ServingCustomer)
			{
				lookAtTargets.currentDuration += Time.deltaTime;

				head.transform.LookAt(lookAtTargets.currentTarget.transform);
				head.transform.rotation = head.transform.rotation;// * Quaternion.Euler(0f, 90f, 0f);

				var newRotation = head.transform.rotation;

				float progress = lookAtTargets.currentDuration / lookAtTargets.duration;

				slerpInterpolation = slerpCurve.Evaluate(progress);

				var targetRotation = Quaternion.Slerp(_startingRotation, newRotation, slerpInterpolation);

				rotationUpdater.newRotation = targetRotation;

				if (lookAtTargets.currentDuration >= lookAtTargets.duration)
				{
					InitializeNewTurn();
				}
			}
		}

		void InitializeNewTurn()
		{
			_startingRotation = head.transform.rotation;

			lookAtTargets.currentDuration = 0f;
			lookAtTargets.duration = UnityEngine.Random.Range(TURN_DURATION_MINIMUM, TURN_DURATION_MAXIMUM);
			lookAtTargets.NextTarget();
		}
	}
}