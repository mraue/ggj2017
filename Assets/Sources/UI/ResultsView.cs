using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace GGJ2017.Game
{
	class ResultsView : MonoBehaviour
	{
		public ResultItemView[] results;

		public void Show(List<GameLoopManager.HighscoreData> data)
		{
			for (int i = 0; i < results.Length && i < data.Count; i++)
			{
				var item = results[i];
				var highscore = data[i];
				item.name = highscore.name;
				item.score = highscore.score;
			}
		}
	}
}