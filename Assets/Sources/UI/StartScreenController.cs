using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;

namespace GGJ2017.Game
{
	class StartScreenController : ViewControllerBase
	{
		public Action onStartGame;

		public void StartGame()
		{
			onStartGame();
		}
	}
}