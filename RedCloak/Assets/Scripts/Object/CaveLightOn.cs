using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaveLightOn : MonoBehaviour
{
    public bool isCaveLightOn = false;
    [SerializeField] private Light2D caveLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isCaveLightOn)
        {
            TurnOnCaveLight();
            isCaveLightOn = true;
        }
        else
        {
            isCaveLightOn = false;
        }
    }

    private void TurnOnCaveLight()
    {
        caveLight.lightType = Light2D.LightType.Freeform;
    }
}
