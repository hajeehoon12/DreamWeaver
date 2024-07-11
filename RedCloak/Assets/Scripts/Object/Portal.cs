using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalDestination;

    public int whichStage = 0;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            
            CameraManager.Instance.CallStage2CameraInfo();
            AudioManager.instance.PlaySFX("Success", 0.5f);
            collision.gameObject.transform.position = portalDestination.position;


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
}
