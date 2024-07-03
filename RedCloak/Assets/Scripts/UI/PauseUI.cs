using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject resumeBtn;

    public void clickResumeBtn()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
