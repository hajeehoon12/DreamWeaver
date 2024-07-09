using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
public class StageChangeManager : MonoBehaviour
{
    private static StageChangeManager _instance;

    public PlayerStat savePlayer;
    
    public static StageChangeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("StageChangeManager").AddComponent<StageChangeManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
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


    public void SaveScene()
    {
        savePlayer = CharacterManager.Instance.Player.stats;    
    }

    public void LoadScene()
    {
        CharacterManager.Instance.Player.stats = savePlayer;
    }

    public void SceneChange(int num)
    {
        StartCoroutine(SceneChanger(num));
    }


    IEnumerator SceneChanger(int num)
    {
        SaveScene();
        Debug.Log(savePlayer.playerSpeed);
        SceneManager.LoadScene(num);
        
        yield return new WaitForSeconds(1f);
        CameraManager.Instance._player = CharacterManager.Instance.Player.GetComponent<Transform>();
        Debug.Log(savePlayer.playerSpeed);
        LoadScene();
    }





}