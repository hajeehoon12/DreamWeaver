using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public GameObject miniMap;
    public Camera miniMapCamera;

    public List<GameObject> miniMaps;
    private void Start()
    {
        SetMinimap(CameraManager.Instance.stageNum);
        //StartCoroutine(InitialSetPlayer());
    }

    private void LateUpdate()
    {
        if(player != null)
        {
            Vector2 newPosition = new Vector3(player.position.x,player.position.y);
            miniMapCamera.transform.position = newPosition;
        }
    }

    public void SetMinimap(int stageNum)
    {
        //Debug.Log(stageNum);
        for(int i = 0; i < miniMaps.Count; i++)
        {
            if(i == stageNum - 1)
            {
                miniMaps[i].SetActive(true);
            }

            else
                miniMaps[i].SetActive(false);
        }
    }

    IEnumerator InitialSetPlayer()
    {
        while(player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if(playerObject != null)
            {
                player = playerObject.transform;
            }

            yield return null;
        }
    }
}
