using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyKnightZone : MonoBehaviour
{
    public HolyKnight holy;
    public bool enterZone = false;
    Collider2D collider2d;

    float xPos = 0;

    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    private void Start()
    {
        collider2d.isTrigger = true;
    }

    public void EndStageBoss()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        xPos = collision.transform.position.x;
 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (xPos > collision.transform.position.x && !holy.isBossDie)
        {
            holy.CallHolyStage();
            enterZone = true;
            collider2d.isTrigger = false;
            CharacterManager.Instance.Player.controller.RunStop();
        }
    }

    public void RemoveWall()
    {
        CameraManager.Instance.CallStage3CameraInfo();
        gameObject.SetActive(false);
    }


}
