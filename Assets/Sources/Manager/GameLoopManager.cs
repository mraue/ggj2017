using System;
using System.Collections.Generic;
using System.Linq;
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

		public BarkeeperManager barkeeperManager;

        public List<Player> players;

        public bool acceptingInput;

		void Awake()
		{
			foreach (var player in players)
			{
				player.onStartedWaving += OnStartedWaving;
			}
		}

		void OnStartedWaving(int id)
		{
			barkeeperManager.CustomerStartedWaving(id);
		}

		public List<HighscoreData> GetHighscoreData()
        {
            return
                players.OrderByDescending(x => x.drinksServed)
                    .Select(
                        player =>
                            new HighscoreData
                            {
                                name = string.Format("Player Key {0}", player.assignedKey),
                                score = player.drinksServed.ToString()
                            })
                    .ToList();
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

        public void Reset()
        {
            foreach (Player player in players)
            {
				player.Reset();
            }
        }
    }
}