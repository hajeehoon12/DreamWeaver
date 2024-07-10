using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    public void OnOpenUI(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (pauseUI.activeInHierarchy)
        {
            pauseUI.SetActive(false);
        }

        else
        {
            pauseUI.SetActive(true);
        }
    }
}
