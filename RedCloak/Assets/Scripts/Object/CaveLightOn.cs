using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaveLightOn : MonoBehaviour
{
    public bool isCaveLightOn = false;
    [SerializeField] private Light2D caveLight;

    private void Start()
    {
        caveLight = CharacterManager.Instance.Player.GetComponent<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isCaveLightOn)
        {
            TurnOnCaveLight();
            isCaveLightOn = true;
            
            if (CameraManager.Instance.stageNum != 4)
            {
                AudioManager.instance.StopBGM();
                
            }
            AudioManager.instance.PlayBGM2("CaveDrop", 0.2f);
        }
        else
        {
            isCaveLightOn = false;
            AudioManager.instance.StopBGM2();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER) && !isCaveLightOn)
        {
            TurnOnCaveLight();
            isCaveLightOn = true;
            if (CameraManager.Instance.stageNum != 4)
            {
                AudioManager.instance.StopBGM();
                
            }
            AudioManager.instance.PlayBGM2("CaveDrop", 0.2f);
        }
    }

    private void TurnOnCaveLight()
    {
        caveLight.lightType = Light2D.LightType.Freeform;

        if (CameraManager.Instance.stageNum == 2)
        {
            caveLight.shapeLightFalloffSize = 0.6f;
        }
        else if (CameraManager.Instance.stageNum == 4)
        {
            caveLight.shapeLightFalloffSize = 1f;
        }
    }
}
