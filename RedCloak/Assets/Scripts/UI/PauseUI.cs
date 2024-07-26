using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private CanvasGroup pauseUICanvasGroup;
    [SerializeField] private InputActionAsset playerInputAction;

    public static bool isPause { get; private set; }

    private void OnEnable()
    {
        DisablePlayerInput();
        pauseUICanvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
        isPause = true;
        Time.timeScale = 0f;
    }

    public void clickResumeBtn()
    {
        StartCoroutine(FadeOut());
    }

    public void clickOptionBtn()
    {
        pauseUI.SetActive(false);
        optionUI.SetActive(true);
    }

    IEnumerator FadeOut()
    {
        pauseUICanvasGroup.DOFade(0f, 0.5f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1.0f;

        EnablePlayerInput();
        isPause = false;

        pauseUI.SetActive(false);
    }

    private void DisablePlayerInput()
    {
        playerInputAction.Disable();
    }

    private void EnablePlayerInput()
    {
        playerInputAction.Enable();
    }
}
