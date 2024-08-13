using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class BossLightEffect : MonoBehaviour
{

    //Transform CharacterManager.Instance.Player.pet.transform;
    public int point = 300;
    public ItemData RewardData;
    private WaitForSeconds interval = new WaitForSeconds(0.2f);
    private WaitForSeconds boundInterval = new WaitForSeconds(0.015f);
    Coroutine thisCoroutine;
    private Coroutine boundCoroutine;
    public string codeNum;

    void Start()
    {
        transform.position += new Vector3(0, 0, -2);
        AudioManager.instance.PlaySFX("Twinkle", 0.5f);
        CharacterManager.Instance.Player.controller.cantMove = true;
        CharacterManager.Instance.Player.controller.RunStop();
        MoveToPlayer();
        
    }

    void MoveToPlayer()
    {
        //CharacterManager.Instance.Player.pet.transform = CharacterManager.Instance.Player.pet.transform;
        boundCoroutine = StartCoroutine(Bound());
        thisCoroutine = StartCoroutine(LifeTime());
    }

    IEnumerator Bound()
    {
        float Dir = CharacterManager.Instance.Player.GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        float time = 0f;
        float timeInterval = 0.015f;

        while (time < 0.5f)
        {
            time += timeInterval;
            transform.position += new Vector3(Dir * 0.02f, 0.02f);
            yield return boundInterval;
        }


        yield return null;

    }

    IEnumerator LifeTime()
    {
        //Destroy(gameObject, 2.2f);
        yield return new WaitForSeconds(0.5f);

        float distance = 10f;
        float firstDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.pet.transform.position);
        float time = 0f;
        float totalTime = 3f;

        while (distance > 0.2f && time < 2f)
        {

            transform.DOKill();
            time += 0.2f;
            float fraction = time / totalTime;

            transform.DOMove(transform.position + (CharacterManager.Instance.Player.pet.transform.position - transform.position) * fraction, 0.2f);
            distance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.pet.transform.position);



            transform.DOMove(CharacterManager.Instance.Player.pet.transform.position, 2 * distance / firstDistance);

            yield return interval;
        }
        transform.DOMove(CharacterManager.Instance.Player.pet.transform.position, 0.2f);
        //transform.DOKill();
        //CharacterManager.Instance.Player.stats.AddGold(2); Add Gold To Player
        yield return interval;
        StartCoroutine(LifeTimeEnd());
    }

    private IEnumerator LifeTimeEnd()
    {
        AudioManager.instance.PlaySFX("LevelUp", 0.2f);
        GetItem();
       
        yield return interval;
        CharacterManager.Instance.Player.stats.playerGold += point;
        //Debug.Log("Point추가 : " + point);
        CharacterManager.Instance.Player.controller.cantMove = false;
        UIManager.Instance.uiBar.UpdateGold();
        
        Destroy(gameObject, 0.5f);
    }

    public void GetItem()
    {
        //TODO Give RewardData to Player
        //Debug.Log("Give Player : " + RewardData.name);
        Player player = CharacterManager.Instance.Player;
        player.itemData = RewardData;
        player.addItem?.Invoke();
        DataSave();

        if (RewardData.type == ItemType.Skill)
        {
            switch (RewardData.skillType)
            {

                case SkillType.Skill1:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill1 = true;
                    UIManager.Instance.skillUI.skill1 = true;
                    //Debug.Log("Green");
                    break;
                case SkillType.Skill2:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill2 = true;
                    UIManager.Instance.skillUI.skill2 = true;
                    break;
                case SkillType.Skill3:
                    player.GetComponentInChildren<PlayerShooting>().PlayerSkill3 = true;
                    UIManager.Instance.skillUI.skill3 = true;
                    break;
                default:
                    break;
            }
            UIManager.Instance.skillUI.UpdateSkill();

        }

        ItemManager.Instance.synchroniztion();
        //UIManager.Instance.itemPopup.PopupGetItem();

    }

    public void DataSave()
    {
        MonsterDataManager.ChangeCatchStat(codeNum);
    }

}
