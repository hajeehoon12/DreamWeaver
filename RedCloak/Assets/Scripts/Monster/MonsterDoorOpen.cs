using System;
using UnityEngine;

public class MonsterDoorOpen : MonoBehaviour
{
    [SerializeField] private Door _door;
    public GameObject DoorCollider;

    private void Start()
    {
        GetComponentInChildren<Monster>().Die += OpenDoor;
    }

    public void OpenDoor()
    {
        _door.OpenDoor();
        DoorCollider.gameObject.SetActive(false);
    }
}