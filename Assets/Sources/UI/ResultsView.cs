using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ2017.Game
{
    class ResultsView : MonoBehaviour
    {
        public ResultItemView[] results;

        public void Show(List<GameLoopManager.HighscoreData> data)
        {
            var scoreGroups = data.GroupBy(x => x.score).OrderByDescending(x => x.First().score).ToArray();
            for (int i = 0; i < results.Length && i < data.Count; i++)
            {
                var item = results[i];
                if (i >= scoreGroups.Length)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    item.gameObject.SetActive(true);

                    var scores = scoreGroups[i];

                    string entrie = string.Empty;

                    if (scores.Count() == 1)
                    {
                        entrie += scores.First().name;
                        entrie += " got ";
                    }
                    else
                    {

                        for (var scoreIndex = 0; scoreIndex < scores.Count(); scoreIndex++)
                        {
                            var highscoreData = scores.ElementAt(scoreIndex);
                            if (scoreIndex == scores.Count() - 1)
                            {
                                entrie += " and " + highscoreData.name + " got ";
                            }
                            else if (scoreIndex == 0)
                            {
                                entrie += highscoreData.name;
                            }
                            else
                            {
                                entrie += ", " + highscoreData.name;
                            }

                        }

                    }

                    if (scores.First().score.Equals("1", StringComparison.Ordinal))
                    {
                        entrie += scores.First().score + " Drink!";
                    }
                    else if (scores.First().score.Equals("0", StringComparison.Ordinal))
                    {
                        entrie += "No Drinks!";
                    }
                    else
                    {
                        entrie += scores.First().score + " Drinks!";
                    }

                    item.result = entrie;
                }
            }
        }
    }
}