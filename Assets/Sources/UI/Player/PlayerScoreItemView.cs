using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace GGJ2017.Game
{
	public class PlayerScoreItemView : MonoBehaviour
	{
		void OnEnable()
		{
			transform.DOScale(Vector3.one * 3f, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCubic);
		}
	}
}