using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using GGJ2017.CrossContext.Services;

namespace GGJ2017.Game
{
	class GameLoopManager : MonoBehaviour
	{
		public Player[] players;

		public bool acceptingInput;

		void Start()
		{
			Setup();
		}

		void Setup()
		{
			Log.logHandler = Debug.Log;
			AudioService.instance.Setup();
		}

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