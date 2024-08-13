using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLight : MonoBehaviour
{
    public ItemData RewardData;
    public GameObject parent;

    public GameObject upArrowIcon;

    public void GetItem()
    {
        //TODO Give RewardData to Player
        //Debug.Log("Give Player : " + RewardData.name);
        Player player = CharacterManager.Instance.Player;
        player.itemData = RewardData;
        player.addItem?.Invoke();

        if (RewardData.type == ItemType.Antique)
        {
            switch (RewardData.itemName)
            {
                case "AirShoes":
                    player.controller.canDJ = true;
                    player.controller.canDash = true;
                    player.controller.canDoubleJump = true;
                    //Debug.Log("AirShoes");
                    //TODO CanMake PlayerDash
                    break;
                case "SharpGloves":
                    player.controller.canWallJump = true;
                    //Debug.Log("SharpGloves");
                    break;
                case "Swordsmanship":
                    player.controller.canComboAttack = true;
                    break;
                default:
                    break;
            
            }
        }

        if (RewardData.type == ItemType.Attack)
        {
            switch (RewardData.itemName)
            {
                case "Weapon Enchant":
                    CharacterManager.Instance.Player.stats.attackDamage += 1;
                    break;
                
                default:
                    break;

            }
        }

        if (RewardData.type == ItemType.Skill)
        {
            switch (RewardData.skillType)
            {

                case SkillType.Skill1:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill1 = true;
                    //Debug.Log("Green");
                    break;
                case SkillType.Skill2:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill2 = true;
                    break;
                case SkillType.Skill3:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill3 = true;
                    break;
                default:
                    break;
            }
            
        }

        
        if (RewardData.type == ItemType.Health)
        {
            ItemManager.Instance.healthPartNum++;
            ItemManager.Instance.synchroniztion();
            UIManager.Instance.uiBar.UpdateMaxHP(1);
        }




        
        ItemManager.Instance.synchroniztion();
        

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER))
        {
            upArrowIcon.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER))
        {
            upArrowIcon.SetActive(false);
        }
    }


}
