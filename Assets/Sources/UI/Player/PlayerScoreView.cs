using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

namespace GGJ2017.Game
{
	class PlayerScoreView : MonoBehaviour
	{
		public int score;
		public float radius;

		public GameObject prefab;

		public List<GameObject> _pool = new List<GameObject>();

		public GameObject camera;

		void Start()
		{
			SetScoreAmount(score);
			transform.DOLocalRotate(new Vector3(0f, 360f, 0), 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
		}

		public void SetScoreAmount(int value)
		{
			score = value;

			for (int i = 0; i < score; i++)
			{
				GameObject go;

				if (i < _pool.Count)
				{
					go = _pool[i];
				}
				else
				{					
					go = Instantiate(prefab) as GameObject;
					_pool.Add(go);
				}

				go.transform.SetParent(this.transform);
				go.transform.localScale = Vector3.one;

				var angle = 2f * Mathf.PI / score * i;
				var x = Mathf.Sin(angle) * radius;
				var y = Mathf.Cos(angle) * radius;

				go.transform.localPosition = new Vector3(x, 0f, y);

				go.SetActive(true);

				go.transform.localEulerAngles = Vector3.zero;
			}

			for (int i = score; i < _pool.Count; i++)
			{
				_pool[i].SetActive(false);
			}
		}

		void Update()
		{
			for (int i = 0; i < _pool.Count; i++)
			{
				var item = _pool[i];

				if (item.activeSelf)
				{
					item.transform.LookAt(camera.transform);
				}
			}
		}
	}
}