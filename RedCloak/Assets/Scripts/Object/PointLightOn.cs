using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PointLightOn : MonoBehaviour
{
    public bool isPointLightOn = false;
    [SerializeField] private Light2D pointLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isPointLightOn)
        {
            TurnOnPointLight();
            isPointLightOn = true;
        }
        else
        {
            isPointLightOn = false;
        }
    }

    private void TurnOnPointLight()
    {
        pointLight.lightType = Light2D.LightType.Point;
        pointLight.pointLightInnerAngle = 360f;
        pointLight.pointLightOuterAngle = 360f;
        pointLight.intensity = 1.0f;
        pointLight.pointLightOuterRadius = 6f;
        pointLight.pointLightInnerAngle = 4f;
    }
}