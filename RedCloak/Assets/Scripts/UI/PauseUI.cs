using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void Toggle()
    {
        if(pauseUI.activeInHierarchy)
        {
            pauseUI.SetActive(false);
        }

        else
        {
            pauseUI.SetActive(true);
        }
    }

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
