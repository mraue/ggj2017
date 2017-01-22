using System.Collections.Generic;
using UnityEngine;

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
                item.result = string.Format("Player {0} got {1} points!", highscore.name, highscore.score);
            }
        }
    }
}