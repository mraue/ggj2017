using UnityEngine;
using UnityEngine.UI;

namespace GGJ2017.Game
{
    class ResultItemView : MonoBehaviour
    {
        public string result { set { resultLabel.text = value; } }

        public Text resultLabel;
    }
}