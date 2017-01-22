using GGJ2017.Shared.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GGJ2017.Shared.Extensions;

namespace GGJ2017.Game
{
	class RotationLateUpdater : MonoBehaviour
	{
		public Quaternion newRotation { set { _newRotation = value; _shouldUpdate = true; }}
		Quaternion _newRotation;
		bool _shouldUpdate;

		void LateUpdate()
		{
			if (_shouldUpdate)
			{
				transform.rotation = _newRotation;
				_shouldUpdate = false;
			}
		}
	}
}