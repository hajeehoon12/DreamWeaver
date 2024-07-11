using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalDestination;

    public int whichStage = 0;
    public bool isStartPortal = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            if (isStartPortal)
            {
                CameraManager.Instance.CallStage2CameraInfo();
                StartCoroutine(TeleportAfterFade(collision.gameObject));
                isStartPortal = false;
            }


            switch (whichStage)
            {
                case 0:
                    break;
                case 1:
                    CameraManager.Instance.CallStage1CameraInfo();
                    break;
                case 2:
                    CameraManager.Instance.CallStage2CameraInfo();
                    break;
                default:
                    break;
            
            }

        }
    }

    private IEnumerator TeleportAfterFade(GameObject player)
    {
        FadeManager.instance.FadeOut();
        yield return new WaitForSeconds(1f);

        player.transform.position = portalDestination.position;

        FadeManager.instance.FadeIn();
        AudioManager.instance.PlaySFX("Success", 0.2f);
        yield return new WaitForSeconds(2f);

        isStartPortal = true;
    }
}
