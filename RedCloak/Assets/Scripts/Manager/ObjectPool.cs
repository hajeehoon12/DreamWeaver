using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static Transform thisTransform;
    [SerializeField] private List<PoolObjects> ObjectsList;
    private static Dictionary<string, GameObject> list = new Dictionary<string, GameObject>();
    private static Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        thisTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        list.Clear();
        pool.Clear();
        
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
    
    public static GameObject GetFromPool(string key)
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
            go = Instantiate(list[key], thisTransform);
            return go;
        }
    }

    public static void ReleaseToPool(string key, GameObject go)
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