using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiZone : MonoBehaviour
{
    public Samurai samurai;
    public bool enterZone = false;
    Collider2D collider2d;

    float yPos = 0;

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
        yPos = collision.transform.position.y;

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (samurai.isBossDie) return;


        if (collision.gameObject.CompareTag("Player") && !enterZone && yPos > collision.transform.position.y)
        {
            samurai.CallSamurai();
            enterZone = true;
            collider2d.isTrigger = false;
            CharacterManager.Instance.Player.controller.RunStop();
        }
    }

    public void RemoveWall()
    {
        CameraManager.Instance.CallStage3CameraInfo("Samurai");
        gameObject.SetActive(false);
    }

}
