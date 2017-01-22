using DG.Tweening;
using UnityEngine;

public class BarLogo : MonoBehaviour
{
    private Material mat;
    private Color originalColor;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.GetColor("_EmissionColor");
        OnMainMenu();

    }

    public void OnGameStart()
    {
        mat.DOColor(originalColor, "_EmissionColor", 0.3f)
            .SetEase(Ease.InBounce)
            .SetLoops(5, LoopType.Yoyo)
            .Play();
    }

    public void OnMainMenu()
    {
        mat.SetColor("_EmissionColor", originalColor * Mathf.LinearToGammaSpace(0));
    }

    private void OnDestroy()
    {
        mat.SetColor("_EmissionColor", originalColor);
    }

}
