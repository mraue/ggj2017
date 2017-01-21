using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace GGJ2017.Game
{
	class ResultItemView : MonoBehaviour
	{
		public string name { set { nameLabel.text = value;} }
		public string score { set { scoreLabel.text = value; } }

		public Text nameLabel;
		public Text scoreLabel;
	}
}