using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RewardChestData
{
    public bool isOnceOpened;
}

public class RewardChest : MonoBehaviour
{
    [SerializeField] SpriteRenderer chestIcon;
    [SerializeField] GameObject upArrowIcon;
    public Sprite openedChestSprite;

    public ItemData dropData;
    public GameObject ItemLight;

    //public bool isOnceOpened = false;

    RewardChestData data;
    
    

    private void Start()
    {
        InitOpen();
    }

    void ChangeBoxState()
    { 
        //TODO modify JSon box state
    }

    public void InitOpen()
    {
        if (data.isOnceOpened)
        {
            chestIcon.sprite = openedChestSprite;
        }
    }

    public void OpenChest()
    {
        chestIcon.sprite = openedChestSprite;
        AudioManager.instance.PlaySFX("Boxopen", 0.2f);
        upArrowIcon.SetActive(false);
        ChangeBoxState();
        //GameManager.Instance.spawnManager.SpawnBoxRewardItem(this.transform);
    }

    public void ThrowItem()
    {
        StartCoroutine(ItemLoad());
    }

    IEnumerator ItemLoad()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject Items = Instantiate(ItemLight, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX("ItemThrow", 0.2f);
        Items.GetComponentInParent<Rigidbody2D>().AddForce(new Vector3(0,10,0), ForceMode2D.Impulse);
        Items.GetComponentInChildren<ItemLight>().RewardData = dropData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER) && !data.isOnceOpened)
        {
            upArrowIcon.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER))
        {
            if (Input.GetKey(KeyCode.UpArrow) && !data.isOnceOpened)
            {

                OpenChest();
                GetComponent<Collider2D>().enabled = false;
                ThrowItem();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER) && data.isOnceOpened)
        {
            upArrowIcon.SetActive(false);
        }
    }
}
