using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PointLightOn : MonoBehaviour
{
    public bool isPointLightOn = false;
    [SerializeField] private Light2D pointLight;


    private void Start()
    {
        pointLight = CharacterManager.Instance.Player.GetComponent<Light2D>();
    }
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isPointLightOn)
        {
            TurnOnPointLight();
            isPointLightOn = true;
        }
    }

    private void TurnOnPointLight()
    {
        pointLight.lightType = Light2D.LightType.Point;
        pointLight.pointLightInnerAngle = 360f;
        pointLight.pointLightOuterAngle = 360f;
        pointLight.intensity = 1.0f;
        pointLight.pointLightOuterRadius = 6f;
        pointLight.pointLightInnerRadius = 3f;
        AudioManager.instance.PlaySFX("Evil", 0.2f);
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("WrongPlace", 0.2f);
    }
}
