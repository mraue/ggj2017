﻿using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;

namespace GGJ2017.Game
{
	class ViewControllerBase : MonoBehaviour
	{
		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}