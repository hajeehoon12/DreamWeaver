using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject optionBtn;

    public void clickResumeBtn()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void clickOptionBtn()
    {
        pauseUI.SetActive(false);
        optionUI.SetActive(true);
    }
}
