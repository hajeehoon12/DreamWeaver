using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
        Screen.fullScreen = true;
    }

    public void ChangeMenu(GameObject currentUI, GameObject targetUI)
    {
        currentUI.SetActive(false);
        targetUI.SetActive(true);
    }
}
