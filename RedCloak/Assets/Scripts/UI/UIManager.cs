using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    public static void ChangeMenu(GameObject currentUI, GameObject targetUI)
    {
        currentUI.SetActive(false);
        targetUI.SetActive(true);
    }
}
