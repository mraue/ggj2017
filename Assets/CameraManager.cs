using DG.Tweening;
using GGJ2017.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    private Camera camera;
    private float originalFov;

    void Awake()
    {
        camera = GetComponent<Camera>();
        originalFov = camera.fieldOfView;

        if (SceneManager.GetSceneByName("Game") != null)
        {
            OnMainMenu();
        }

        GameManager.OnGameStarted += OnGameStart;
        GameManager.OnMainMenuStarted += OnMainMenu;

    }

    void OnDestroy()
    {
        GameManager.OnGameStarted -= OnGameStart;
        GameManager.OnMainMenuStarted -= OnMainMenu;

    }

    private void OnGameStart()
    {
        camera.DOFieldOfView(originalFov, 0.5f).Play();
    }

    private void OnMainMenu()
    {
        camera.DOFieldOfView(originalFov * 1.1f, 0.5f).Play();
    }
}
