using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLight : MonoBehaviour
{
    public ItemData RewardData;
    public GameObject parent;

    public void GetItem()
    {
        //TODO Give RewardData to Player
        Debug.Log("Give Player : " + RewardData.name);
        CharacterManager.Instance.Player.itemData = RewardData;
        CharacterManager.Instance.Player.addItem?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                GetItem();
                AudioManager.instance.PlaySFX("ItemPickup", 0.2f);
                Destroy(parent);
            }
        }
    }

}
