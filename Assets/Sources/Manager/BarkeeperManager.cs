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
		public RotationLateUpdater rotationUpdater;

		public ActionTargets lookAtTargets;

		[Range(0f, 1.0f)]
		public float slerpInterpolation = 0.1f;
		public AnimationCurve slerpCurve;

		void Start()
		{
			InitializeNewTurn();
		}

		void Update()
		{
			UpdateLookingDirection();
		}

		void UpdateLookingDirection()
		{
			if (_state != State.ServingCustomer)
			{
				lookAtTargets.currentDuration += Time.deltaTime;

				var oldRotation = head.transform.rotation;

				head.transform.LookAt(lookAtTargets.currentTarget.transform);
				head.transform.rotation = head.transform.rotation * Quaternion.Euler(0f, 90f, 0f);

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