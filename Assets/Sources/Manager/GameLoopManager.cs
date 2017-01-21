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
		public class HighscoreData
		{
			public string name;
			public string score;
		}

		public Player[] players;

		public bool acceptingInput;

		public List<HighscoreData> GetHighscoreData()
		{
			var data = new List<HighscoreData>();

			foreach (var player in players)
			{
				data.Add(new HighscoreData { name = string.Format("Player Key {0}", player.assignedKey), score = player.drinksServed.ToString() });
			}

			data.Sort((x, y) => x.score.CompareTo(y.score));

			return data;
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