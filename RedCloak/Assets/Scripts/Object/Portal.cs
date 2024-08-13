using Demo_Project;
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
                
                collision.gameObject.transform.position = portalDestination.position;
                StartCoroutine(TeleportAfterFade());//collision.gameObject
                //isStartPortal = false;
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
                case 3:
                    CameraManager.Instance.CallStage3CameraInfo();
                    break;
                case 4:
                    CameraManager.Instance.CallStage4CameraInfo();
                    break;

                default:
                    break;
            
            }
        }
    }

    private IEnumerator TeleportAfterFade()//GameObject player
    {
        FadeManager.instance.FadeOut(0f);
        //yield return new WaitForSeconds(1f);

        FadeManager.instance.FadeIn(2f);
        AudioManager.instance.PlaySFX("Success", 0.2f);
        yield return new WaitForSeconds(2f);

        isStartPortal = true;

        ShowStageName();
    }

    private void ShowStageName()
    {
        //string stageName = "";
        switch(whichStage)
        {
            case 1:
                //stageName = "Stage 1";
                break;
            case 2:
                //stageName = "Stage 2";
                break;
            case 3:
                //stageName = "Stage 3";
                break;
            default:
                break;
        }

        //if(!string.IsNullOrEmpty(stageName))
        //{
        //    UIManager.Instance.changeStage.FadeInStageUI(1f, stageName);
        //}
    }
}
