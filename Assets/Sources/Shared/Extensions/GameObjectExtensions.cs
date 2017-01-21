using UnityEngine;

namespace GGJ2017.Shared.Extensions
{
    public static class GameObjectExtensions
    {
        public static void ActivateAndAttach(this GameObject go, GameObject parent)
        {
            go.SetActive(true);
            go.transform.SetParent(parent.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
        }
    }
}
