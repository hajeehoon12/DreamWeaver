using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeMenu(GameObject currentUI, GameObject targetUI)
    {
        currentUI.SetActive(false);
        targetUI.SetActive(true);
    }
}
