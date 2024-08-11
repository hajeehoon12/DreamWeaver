using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class MonsterDataManager : MonoBehaviour
{
    [SerializeField] private TextAsset mobDataJson;
    [SerializeField] private TextAsset mapDataJson;
    private MonsterData[] monsterDatas;
    private MapData[] mapDatas;
    private GameObject mapParent;
    private static List<GameObject> maps = new List<GameObject>();

    private void Awake()
    {
        LoadJsonData();
    }

    private void OnEnable()
    {
        if (mapParent != null)
        {
            Destroy(mapParent);
            mapParent = null;
            maps.Clear();
        }
            
        LoadMapData();
        LoadMonsterData();
        gameObject.SetActive(false);
    }

    public void LoadJsonData()
    {
        if (mobDataJson != null)
        {
            MonsterDataArray array = JsonUtility.FromJson<MonsterDataArray>("{\"data\":" + mobDataJson.text + "}");
            monsterDatas = array.data;
        }

        if (mapDataJson != null)
        {
            MapDataArray array = JsonUtility.FromJson<MapDataArray>("{\"data\":" + mapDataJson.text + "}");
            mapDatas = array.data;
        }
    }
    
    public void LoadMonsterData()
    {
        foreach (var monster in monsterDatas)
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
                m.point = monster.dropPoint;
        }
    }

    public void LoadMapData()
    {
        mapParent = new GameObject("Monsters");
        foreach (var map in mapDatas)
        {
            GameObject m = new GameObject(map.rcode);
            m.transform.parent = mapParent.transform;
            maps.Add(m);
        }
    }

    public void LoadSaveData()
    {
        
    }

    public static void ToggleMonsters(int stageNum)
    {
        for (int i = 0; i < maps.Count; i++)
            maps[i].SetActive(i == stageNum-1);
    }

    [Serializable]
    private class MonsterDataArray
    {
        public MonsterData[] data;
    }

    [Serializable]
    private class MapDataArray
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