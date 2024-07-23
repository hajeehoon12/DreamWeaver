using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject inventory;

    private void Start()
    {
        pauseUI.SetActive(false);
        inventory.SetActive(false);
    }

    public void OnPauseUI(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started)
        {
            Toggle(pauseUI);
        }
    }

    public void OnInventory(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started)
        {
            Toggle(inventory);
        }
    }

    public void Toggle(GameObject uiElement)
    {
        if (!uiElement.activeInHierarchy)
        {
            uiElement.SetActive(true);
        }
    }
}
