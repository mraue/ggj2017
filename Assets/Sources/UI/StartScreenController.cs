using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;

namespace GGJ2017.Game
{
	class StartScreenController : MonoBehaviour
	{
		public Action onStartGame;

		public void StartGame()
		{
			onStartGame();
		}

		public void Show()
		{
			
		}

		public void Hide()
		{
			
		}
	}
}