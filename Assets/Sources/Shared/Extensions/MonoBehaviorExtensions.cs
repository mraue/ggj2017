using System;
using System.Collections;
using UnityEngine;

namespace GGJ2017.Shared.Extensions
{
    public static class MonoBehaviorExtensions
    {
        public static void ExecuteDelayed(this MonoBehaviour mb, float delay, Action executeAfterDelay)
        {
            if (delay > 0f)
            {
                mb.StartCoroutine(ExecuteDelayed(delay, executeAfterDelay));
            }
            else
            {
                executeAfterDelay();
            }
        }

        public static IEnumerator ExecuteDelayed(float delay, Action executeAfterDelay)
        {
            yield return new WaitForSeconds(delay);
            executeAfterDelay();
        }
    }
}