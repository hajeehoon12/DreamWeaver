using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    public Vector3 SavePoint;
    public bool isLoadScene = true;
    public int playerInitHealth = 5;

    public Door[] doors = new Door[5];

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
        SavePoint = Player.transform.position;
    }

    public void CallDeath()
    {
        FadeManager.instance.FadeOut(1f);
        
        
        StartCoroutine(CallSave());
    }

    IEnumerator CallSave()
    {
        isLoadScene = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return new WaitUntil(() => isLoadScene);

        CameraManager.Instance.SelectStage();
        CharacterManager.Instance.Player.stats.playerHP = Player.stats.playerMaxHP;
        
        
        Player.transform.position = SavePoint;
        UIManager.Instance.uiBar.SetCurrentHP();
        UIManager.Instance.skillUI.InitateRotation();
        UIManager.Instance.uiBar.CallBackBossBar();
        AudioManager.instance.PlaySFX("HeartUp", 0.2f);
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.TRAP), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.ENEMY), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.BOSS), LayerMask.NameToLayer(Define.PLAYER), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Define.MONSTERPROJECTILE), LayerMask.NameToLayer(Define.PLAYER), false);
        
    }
    

}