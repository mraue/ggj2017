using DG.Tweening;
using UnityEngine;

public class MenuLogo : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScale(1.1f, 0.75f)
            .SetEase(Ease.InOutFlash)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }
}
