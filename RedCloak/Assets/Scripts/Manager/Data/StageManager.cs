using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;

    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("StageManager").AddComponent<StageManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    StageDatas stageDatas;
    StageData currentStage;

    public int currentStageNum = 0;

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
        // get Datas from JSON
    }

    public void LoadStage() // Load CurrentStage Datas
    { 
        currentStage = stageDatas.GetStageData(currentStageNum);

        // TODO UnPack Datas
    }


    public void SaveData() // Save CurrentStage Datas
    {
        // TODO Pack Datas

        stageDatas.SetStageData(currentStageNum, currentStage);
    }


}
