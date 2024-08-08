using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundary : MonoBehaviour
{
    [SerializeField] private int StartStage = 1;
    [SerializeField] private int EndStage = 1;


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.PLAYER))
        {
            if (StartStage == 0)
            {
                Debug.Log("Do Initiate");
            }

            switch (EndStage)
            {
                case 0:
                    break;
                case 1:
                    CameraManager.Instance.CallStage1CameraInfo();
                    break;
                case 2:
                    CameraManager.Instance.CallStage2CameraInfo();
                    CharacterManager.Instance.Player.transform.position = new Vector3(264, -80, 1);
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




}
