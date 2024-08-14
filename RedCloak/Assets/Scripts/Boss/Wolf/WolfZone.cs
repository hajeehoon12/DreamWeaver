using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfZone : MonoBehaviour
{
    private Collider2D collider2d;
    public Wolf wolf;

    float xPos = 0;

    public GameObject[] childWall;
    public GameObject sakura;
    public ParticleSystem rain;

    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        //wolf = GetComponentInParent<Wolf>();
        
    }

    private void Start()
    {
        sakura.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            xPos = collision.transform.position.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            if (xPos < collision.transform.position.x && !wolf.isBossDie)
            {
                wolf.CallWolfBoss();
                collider2d.isTrigger = false;
                StartCoroutine(TransferChar(collision.gameObject));
                gameObject.layer = LayerMask.NameToLayer(Define.FLOOR);
                childWall[0].layer = LayerMask.NameToLayer(Define.FLOOR);
                childWall[1].layer = LayerMask.NameToLayer(Define.FLOOR);
                collision.transform.position = new Vector3(xPos, collision.transform.position.y, collision.transform.position.z);
                sakura.SetActive(true);
                CharacterManager.Instance.Player.controller.RunStop();
            }
        }
    }

    public void RemoveWall()
    {
        
        CameraManager.Instance.CallStage2CameraInfo("Wolf");
        gameObject.SetActive(false);

    }

    IEnumerator TransferChar(GameObject player)
    {
        yield return new WaitForSeconds(4f);
        player.transform.position = new Vector3(xPos+4, player.transform.position.y-4, player.transform.position.z);

    }

    public void RainOn()
    {
        AudioManager.instance.PlayWolf("Thunder", 0.5f);
        rain.Play();
        sakura.SetActive(false);
    }
    public void RainOff() 
    { 
        rain.Stop(); 
        sakura.SetActive(true);
    }

}
