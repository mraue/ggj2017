using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2017.Game
{
    class GameLoopManager : MonoBehaviour
    {
        public class HighscoreData
        {
            public string name;
            public string score;
        }

        public List<Player> players;

        public bool acceptingInput;

        public List<HighscoreData> GetHighscoreData()
        {
            var data = new List<HighscoreData>();

            foreach (var player in players)
            {
                data.Add(new HighscoreData { name = string.Format("Player Key {0}", player.assignedKey), score = player.drinksServed.ToString() });
            }

            data.Sort((x, y) => string.Compare(x.score, y.score, StringComparison.Ordinal));

            return data;
        }

        void Update()
        {
            if (acceptingInput)
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