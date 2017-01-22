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
		const float SERVE_DRINK_DURATION = 3f;

		const float TURN_DURATION_MINIMUM = 0.7f;
		const float TURN_DURATION_MAXIMUM = 2f;

		const float STATE_IDLE_MINIMUM = 1f;
		const float STATE_IDLE_MAXIMUM = 2f;

		const float STATE_MOVING_MINIMUM = 1.5f;
		const float STATE_MOVING_MAXIMUM = 3f;

		const float STATE_DANCING_MINIMUM = 2f;
		const float STATE_DANCING_MAXIMUM = 3f;

		const string ANIMATION_ID_IDLE = "Idle";
		const string ANIMATION_ID_SERVE_CUSTOMER = "Pouring";
		const string ANIMATION_ID_DANCE = "Dance";
		const string ANIMATION_ID_WALK = "Walk";

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

		public Animator animator;

		public event Action<int> onServeDrink = delegate { };

		float _stateDuration;
		float _stateCurrentDuration;

		Quaternion _defaultRotation;

		Quaternion _startingRotation;
		Vector3 _startingPositionMovement;
		Quaternion _startingRotationMovement;
		Quaternion _endRotationMovement;

		int _activeCustomerId;
		bool _serveCustomerAnimationSet;

		void Awake()
		{
			_defaultRotation = movementAnchor.transform.rotation;
		}

		public void CustomerStartedWaving(int id)
		{	
			float dot = Vector3.Dot(head.transform.forward.normalized, (customers[id].transform.position - head.transform.position).normalized);
			var angle = Mathf.Acos(dot) * 180f / Math.PI;

			Log.InfoFormat("[BarkeeperManager::CustomerStartedWaving] id={0}, dot={1}, angle={2}", id, dot, angle);

			if (_state == State.ServingCustomer)
			{
				Log.Info("[BarkeeperManager::CustomerStartedWaving] We are serving someone already");
				return;
			}

			if (angle <= viewConeHalfAngle)
			{				
				Log.InfoFormat("[BarkeeperManager::CustomerStartedWaving] Serving customer id={0}", id);

				AudioService.instance.Play(AudioId.ServeDrink);
				_activeCustomerId = id;
				onServeDrink(id);

				_stateCurrentDuration = 0f;
				_stateDuration = SERVE_DRINK_DURATION;

				_startingPositionMovement = movementAnchor.transform.position;
				_startingRotationMovement = movementAnchor.transform.rotation;

				animator.SetTrigger(ANIMATION_ID_WALK);

				_serveCustomerAnimationSet = false;

				_state = State.ServingCustomer;
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
				switch (UnityEngine.Random.Range(0, 12))
				{
					default:
					case 0:
						_state = State.Idle;
						_stateDuration = UnityEngine.Random.Range(STATE_IDLE_MINIMUM, STATE_IDLE_MAXIMUM);
						animator.SetTrigger(ANIMATION_ID_IDLE);
						break;
					case 1:
					case 2:
					case 3:
						_state = State.Moving;
						_stateDuration = UnityEngine.Random.Range(STATE_MOVING_MINIMUM, STATE_MOVING_MAXIMUM);

						_startingPositionMovement = movementAnchor.transform.position;
						_startingRotationMovement = movementAnchor.transform.rotation;

						moveToTargets.NextTarget();
						moveToTargets.currentDuration = 0f;
						moveToTargets.duration = _stateDuration;

						var movementDistance = (moveToTargets.currentTarget.transform.position - _startingPositionMovement).magnitude;

						if (movementDistance > 1f)
						{							
							animator.SetTrigger(ANIMATION_ID_WALK);
						}

						break;
					case 4:
						_state = State.Dancing;
						_stateDuration = UnityEngine.Random.Range(STATE_DANCING_MINIMUM, STATE_DANCING_MAXIMUM);
						animator.SetTrigger(ANIMATION_ID_DANCE);
						break;
				}

				_stateCurrentDuration = 0f;
			}

			switch(_state)
			{
				case State.Moving:
					UpdateMovement();
					break;
				case State.ServingCustomer:
					UpdateServeCustomer();
					break;
			}
		}

		void UpdateServeCustomer()
		{
			var progress = _stateCurrentDuration / _stateDuration;

			if(progress <= 0.3f)
			{				
				var pos = movementAnchor.transform.position;
				pos.z = Vector3.Lerp(_startingPositionMovement, customers[_activeCustomerId].transform.position, progress / 0.3f).z;
				movementAnchor.transform.position = pos;
				movementAnchor.transform.rotation = Quaternion.Lerp(_startingRotationMovement, _defaultRotation, progress / 0.3f);
			}
			else if (!_serveCustomerAnimationSet)
			{				
				animator.SetTrigger(ANIMATION_ID_SERVE_CUSTOMER);
				_serveCustomerAnimationSet = true;
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
				movementAnchor.transform.rotation = Quaternion.Lerp(_endRotationMovement, _startingRotationMovement, (progress - 0.8f) / 0.2f);
			}
		}

		void UpdateLookingDirection()
		{
			if (_state != State.ServingCustomer
			    && _state != State.Dancing)
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