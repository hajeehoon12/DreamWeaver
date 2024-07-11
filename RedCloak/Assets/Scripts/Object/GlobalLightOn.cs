using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class GlobalLightOn : MonoBehaviour
{
    private bool isGlobalLightOn = true;
    [SerializeField] private Light2D globalLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isGlobalLightOn)
        {
            globalLight.lightType = Light2D.LightType.Global;
            isGlobalLightOn = true;
        }
        else
        {
            isGlobalLightOn = false;
        }
    }
}