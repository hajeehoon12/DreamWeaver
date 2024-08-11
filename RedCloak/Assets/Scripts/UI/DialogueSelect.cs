using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSelect : MonoBehaviour
{
    public GameObject Shop;

    public void OpenShop()
    {
        UIManager.Instance.OpenUI(Shop);
    }

    public void CloseSelectMenu()
    {
        UIManager.Instance.CloseCurrentUI();
    }
}
