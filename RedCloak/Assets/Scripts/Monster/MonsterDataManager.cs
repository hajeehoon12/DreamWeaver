using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    [SerializeField] private TextAsset mobDataJson;
    [SerializeField] private TextAsset mapDataJson;
    
    private GameObject mapParent;
    private static List<GameObject> maps = new List<GameObject>();
    public static MonsterDataArray mobArray { get; private set; }
    public static MapDataArray mapArray { get; private set; }

    private void Awake()
    {
        if (mobArray == null)
            LoadJsonData();
    }

    private void OnEnable()
    {
        if (mapParent != null)
        {
            Destroy(mapParent);
            mapParent = null;
        }
        
        if (maps.Count != 0)
            maps.Clear();
            
        LoadMapData();
        LoadMonsterData();
        gameObject.SetActive(false);
    }

    public void LoadJsonData()
    {
        string path = $"{Application.persistentDataPath}/data.json";

        if (File.Exists(path))
        {
            string data = AEScrypt.Decrypt(File.ReadAllText(path));
            LoadSaveData(data);
        }
        else
        {
            if (mobDataJson != null)
                mobArray = JsonUtility.FromJson<MonsterDataArray>("{\"data\":" + mobDataJson.text + "}");
        }

        if (mapDataJson != null)
            mapArray = JsonUtility.FromJson<MapDataArray>("{\"data\":" + mapDataJson.text + "}");
    }
    
    public void LoadSaveData(string text)
    {
        if (text != null)
            mobArray = JsonUtility.FromJson<MonsterDataArray>(text);
    }
    
    public void LoadMonsterData()
    {
        foreach (var monster in mobArray.data)
        {
            if (monster is { isRegen: false, isCatch: true })
                continue;
            
            string[] pos = monster.pos.Split(",");
            float[] floatpos = new float[pos.Length];
            for (int i = 0; i < pos.Length; i++)
                floatpos[i] = float.Parse(pos[i]);

            int mapIndex = int.Parse(monster.mapData[^1].ToString()) - 1;
            GameObject mob = Instantiate(Resources.Load<GameObject>($"Monster/{monster.enemyName}"), maps[mapIndex].transform);
            mob.transform.position = new Vector3(floatpos[0], floatpos[1], floatpos[2]);

            if (mob.TryGetComponent<Monster>(out Monster m))
                m.data = monster;
        }
    }

    public void LoadMapData()
    {
        mapParent = new GameObject("Monsters");
        foreach (var map in mapArray.data)
        {
            GameObject m = new GameObject(map.rcode);
            m.transform.parent = mapParent.transform;
            maps.Add(m);
        }
    }

    public static void SaveMobData()
    {
        string json = AEScrypt.Encrypt(JsonUtility.ToJson(mobArray, true));

        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);

        string filePath = $"{Application.persistentDataPath}/data.json";
        File.WriteAllText(filePath, json);
    }

    public static void ToggleMonsters(int stageNum)
    {
        for (int i = 0; i < maps.Count; i++)
            maps[i].SetActive(i == stageNum-1);
    }

    public static void ChangeCatchStat(string rcode)
    {
        int index = int.Parse(rcode.Substring(rcode.Length - 5)) - 1;

        mobArray.data[index].isCatch = true;
    }

    [Serializable]
    public class MonsterDataArray
    {
        public MonsterData[] data;
    }

    [Serializable]
    public class MapDataArray
    {
        public MapData[] data;
    }
}

[Serializable]
public struct MapData
{
    public string rcode;
    public string displayName;
    public string description;
    public string pos;
}

[Serializable]
public struct MonsterData
{
    public string rcode;
    public string enemyCode;
    public string enemyName;
    public bool isRegen;
    public string mapData;
    public string pos;
    public bool isCatch;
    public int dropPoint;
}