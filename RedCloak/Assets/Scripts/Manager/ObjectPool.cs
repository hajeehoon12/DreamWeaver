using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private List<PoolObjects> ObjectsList;
    private Dictionary<string, GameObject> list = new Dictionary<string, GameObject>();
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (ObjectsList != null)
        {
            for (int i = 0; i < ObjectsList.Count; i++)
            {
                Queue<GameObject> objects = new Queue<GameObject>();
                for (int j = 0; j < ObjectsList[i].PoolAmount; j++)
                {
                    GameObject go = Instantiate(ObjectsList[i].ObjectPrefab, transform);
                    go.SetActive(false);
                    objects.Enqueue(go);
                }
                pool.Add(ObjectsList[i].ObjectName, objects);
                list.Add(ObjectsList[i].ObjectName, ObjectsList[i].ObjectPrefab);
            }
        }
    }

    public GameObject GetFromPool(string key)
    {
        GameObject go;
        try
        {
            if (pool[key].TryDequeue(out go))
            {
                return go;
            }
            else
            {
                throw new AvaliableObjectNotFound() { NotFound = true };
            }
        }
        catch (AvaliableObjectNotFound e) when (e.NotFound)
        {
            Debug.Log(e.ExceptionMessage);
            go = Instantiate(list[key], transform);
            return go;
        }
    }

    public void ReleaseToPool(string key, GameObject go)
    {
        go.SetActive(false);
        pool[key].Enqueue(go);
    }
}

[Serializable]
public struct PoolObjects
{
    public string ObjectName;
    public GameObject ObjectPrefab;
    public int PoolAmount;
}

class AvaliableObjectNotFound : Exception
{
    public bool NotFound { get; set; } = false;
    public string ExceptionMessage = "사용 가능한 오브젝트가 없어 새로운 오브젝트를 생성합니다.. Pool의 Size를 늘려주세요.";
}