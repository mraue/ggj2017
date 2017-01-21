using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;

namespace GGJ2017.Game
{
	class GameLoopManager : MonoBehaviour
	{
		public Player[] players;

		public bool acceptingInput;

		void Update()
		{
			if(acceptingInput)
			{
				foreach (var player in players)
				{
					if (Input.GetKeyDown(player.assignedKey))
					{
						player.ShouldWave();
					}
				}
			}
		}
	}
}