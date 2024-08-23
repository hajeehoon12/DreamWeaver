using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

[System.Serializable]
public struct PlayerData
{
    public float playerSpeed;
    public float jumpPower;
    public float attackDamage;
    public float playerHP;
    public float playerMaxHP;
    public float playerMP;
    public float playerMaxMP;
    public float playerGold;

    public bool canRoll; // true
    public bool canDash;
    public bool canComboAttack;
    public bool canWallJump;
    public bool canDJ;

    public bool Skill1;
    public bool Skill2;
    public bool Skill3;

    public bool haveSave;
    public int SaveStage;
    public Vector3 lastPos;
}


public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    public bool isLoadScene = true;
    public int playerInitHealth = 5;

    public Door[] doors = new Door[5];
    public DoorDataArray doorData;

    public PlayerStat tempStat;

    public bool canSwapSkill = true;

    public PlayerDataSet playerData { get; set; }
    
    public float playerSpeed = 10;
    public float jumpPower = 15;
    public float attackDamage = 5;
    public float playerHP = 5;
    public float playerMaxHP = 5;
    public float playerMP = 50;
    public float playerMaxMP = 50;
    public float playerGold = 0;

    public bool canRoll = true;
    public bool canDash = false;
    public bool canComboAttack = false;
    public bool canWallJump = false;
    public bool canDJ = false;

    public bool Skill1 = false;
    public bool Skill2 = false;
    public bool Skill3 = false;

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

        if (playerData == null)
            playerData = SaveLoad.Load<PlayerDataSet>("PlayerData");
        
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
        if (!playerData.data.haveSave)
        {
            playerData.data.lastPos = Player.transform.position;
            //playerData.data.SaveStage = 1;
            //SavePoint = Player.transform.position;
        }
        LoadInfo();
    }

    public void SaveInfo()
    { 
        PlayerController PC = Player.controller;
        PlayerStat stat = Player.stats;

        playerData.data.playerSpeed = stat.playerSpeed;
        playerData.data.jumpPower = stat.jumpPower;
        playerData.data.attackDamage = stat.attackDamage;
        playerData.data.playerHP = stat.playerHP;
        playerData.data.playerMaxHP = stat.playerMaxHP;
        playerData.data.playerMP = stat.playerMP;
        playerData.data.playerMaxMP = stat.playerMaxMP;
        playerData.data.playerGold = stat.playerGold;

        playerData.data.canRoll = PC.canRoll;
        playerData.data.canDash = PC.canDash;
        playerData.data.canComboAttack = PC.canComboAttack;
        playerData.data.canWallJump = PC.canWallJump;
        playerData.data.canDJ = PC.canDJ;
        playerData.data.Skill1 = PC.shootProjectile.PlayerSkill1;
        playerData.data.Skill2 = PC.shootProjectile.PlayerSkill2;
        playerData.data.Skill3 = PC.shootProjectile.PlayerSkill3;

        playerData.data.haveSave = true;

        playerData.data.lastPos = Player.transform.position;
        playerData.data.SaveStage = CameraManager.Instance.stageNum;

        SaveLoad.Save("MobData", MonsterDataManager.mobArray);
        SaveLoad.Save("DoorData", CharacterManager.Instance.doorData);
        SaveLoad.Save("RewardBoxData", RewardBoxDataManager.array);
        SaveLoad.Save("PlayerData", CharacterManager.Instance.playerData);
    }

    public void LoadInfo()
    { 
        
        PlayerController PC = Player.controller;
        PlayerStat stat = Player.stats;

        

        stat.playerSpeed = playerData.data.playerSpeed;
        stat.jumpPower = playerData.data.jumpPower;
        stat.attackDamage = playerData.data.attackDamage;
        stat.playerHP = playerData.data.playerHP;
        stat.playerMaxHP = playerData.data.playerMaxHP;
        stat.playerMP = playerData.data.playerMP;
        stat.playerMaxMP = playerData.data.playerMaxMP;
        stat.playerGold = playerData.data.playerGold;

        PC.canRoll = playerData.data.canRoll;
        PC.canDash = playerData.data.canDash;
        PC.canComboAttack = playerData.data.canComboAttack;
        PC.canWallJump = playerData.data.canWallJump;
        PC.canDJ = playerData.data.canDJ;
        PC.shootProjectile.PlayerSkill1 = playerData.data.Skill1;
        PC.shootProjectile.PlayerSkill2 = playerData.data.Skill2;
        PC.shootProjectile.PlayerSkill3 = playerData.data.Skill3;
        //Debug.Log("Load");
        AudioManager.instance.StopBGM2();

        Player.transform.position = playerData.data.lastPos;
        CameraManager.Instance.SelectStage(playerData.data.SaveStage);
        UIManager.Instance.uiBar.UpdateGold();
        //UIManager.Instance.skillUI.UpdateSkill();
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


        Player.transform.position = playerData.data.lastPos;
        UIManager.Instance.uiBar.SetCurrentHP();
        UIManager.Instance.skillUI.InitateRotation();
        canSwapSkill = true;
        UIManager.Instance.uiBar.CallBackBossBar();
        CameraManager.Instance.SelectStage(playerData.data.SaveStage);
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
        public string version;
    }

    [Serializable]
    public class PlayerDataSet
    {
        public PlayerData data;
        public string version;
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