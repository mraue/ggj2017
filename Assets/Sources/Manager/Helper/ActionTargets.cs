using GGJ2017.Shared.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GGJ2017.Shared.Extensions;

namespace GGJ2017.Game
{
	class ActionTargets : MonoBehaviour
	{
		public int multiplier;

		public List<GameObject> targets;

		public float duration;
		public float currentDuration;

		public GameObject currentTarget;

		List<GameObject> _currentTargets = new List<GameObject>();

		public void NextTarget()
		{
			if (currentTarget == null && _currentTargets.Count > 0)
			{
				currentTarget = _currentTargets[0];
			}
			else
			{
				if (_currentTargets.Count <= 1)
				{
					InitializeTargets();
				}

				_currentTargets.RemoveAt(0);

				currentTarget = _currentTargets[0];
			}
		}

		void InitializeTargets()
		{
			_currentTargets.Clear();

			for (int i = 0; i < multiplier; i++)
			{
				_currentTargets.AddRange(targets);
			}

			_currentTargets.Shuffle();
		}
	}
}