using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    public Vector3 SavePoint;
    public int SaveStage = 1;

    public bool haveSave = false;

    public bool isLoadScene = true;
    public int playerInitHealth = 5;

    public Door[] doors = new Door[5];
    public DoorDataArray doorData;

    public PlayerStat tempStat;

    public bool canSwapSkill = true;

    public float playerSpeed = 10;
    public float jumpPower = 15;
    public float attackDamage = 5;
    public float playerHP = 5;
    public float playerMaxHP = 5;
    public float playerMP = 50;
    public float playerMaxMP = 50;
    public float playerGold = 0;

    public bool canRoll = true;
    public bool canDash = true;
    public bool canComboAttack = true;
    public bool canWallJump = true;
    public bool canDJ = true;

    public bool Skill1 = true;
    public bool Skill2 = true;
    public bool Skill3 = true;

    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if (doorData == null)
            doorData = SaveLoad.Load<DoorDataArray>("DoorData");
        
        if (_instance != null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        if (!haveSave)
        {
            SavePoint = Player.transform.position;
        }
    }

    public void SaveInfo()
    { 
        

        PlayerController PC = Player.controller;
        PlayerStat stat = Player.stats;

        playerSpeed = stat.playerSpeed;
        jumpPower = stat.jumpPower;
        attackDamage = stat.attackDamage;
        playerHP = stat.playerHP;
        playerMaxHP = stat.playerMaxHP;
        playerMP = stat.playerMP;
        playerMaxMP = stat.playerMaxMP;
        playerGold = stat.playerGold;

        canRoll = PC.canRoll;
        canDash = PC.canDash;
        canComboAttack = PC.canComboAttack;
        canWallJump = PC.canWallJump;
        canDJ = PC.canDJ;
        Skill1 = PC.shootProjectile.PlayerSkill1;
        Skill2 = PC.shootProjectile.PlayerSkill2;
        Skill3 = PC.shootProjectile.PlayerSkill3;

        haveSave = true;

        SavePoint = Player.transform.position;
        SaveStage = CameraManager.Instance.stageNum;
    }

    public void LoadInfo()
    { 
        
        PlayerController PC = Player.controller;
        PlayerStat stat = Player.stats;

        stat.playerSpeed = playerSpeed;
        stat.jumpPower = jumpPower;
        stat.attackDamage = attackDamage;
        stat.playerHP = playerHP;
        stat.playerMaxHP = playerMaxHP;
        stat.playerMP = playerMP;
        stat.playerMaxMP = playerMaxMP;
        stat.playerGold = playerGold;

        PC.canRoll = canRoll;
        PC.canDash = canDash;
        PC.canComboAttack = canComboAttack;
        PC.canWallJump = canWallJump;
        PC.canDJ = canDJ;
        PC.shootProjectile.PlayerSkill1 = Skill1;
        PC.shootProjectile.PlayerSkill2 = Skill2;
        PC.shootProjectile.PlayerSkill3 = Skill3;

        AudioManager.instance.StopBGM2();

        UIManager.Instance.skillUI.UpdateSkill();
    }

    public void CallDeath()
    {
        FadeManager.instance.FadeOut(1f);
        
        
        StartCoroutine(CallSave());
    }

    IEnumerator CallSave()
    {
        canSwapSkill = false;
        isLoadScene = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return new WaitUntil(() => isLoadScene);

        //yield return new WaitForSeconds(0.3f);
        LoadInfo();
        CharacterManager.Instance.Player.stats.playerHP = Player.stats.playerMaxHP;
        
        
        Player.transform.position = SavePoint;
        UIManager.Instance.uiBar.SetCurrentHP();
        UIManager.Instance.skillUI.InitateRotation();
        canSwapSkill = true;
        UIManager.Instance.uiBar.CallBackBossBar();
        CameraManager.Instance.SelectStage(SaveStage);
        AudioManager.instance.PlaySFX("HeartUp", 0.2f);

        
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.TRAP), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.ENEMY), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.BOSS), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MONSTERPROJECTILE), LayerMask.NameToLayer(Define.PLAYER), false);
        
    }

    public bool GetDoorOpenStat(int index)
    {
        return doorData.data[index].isOpen;
    }

    public void ChangeDoorOpenStat(int index)
    {
        doorData.data[index].isOpen = true;
    }

    [Serializable]
    public class DoorDataArray
    {
        public DoorData[] data;
    }
}

[Serializable]
public struct DoorData
{
    public string rcode;
    public string displayName;
    public string MapData;
    public string description;
    public bool isOpen;
}