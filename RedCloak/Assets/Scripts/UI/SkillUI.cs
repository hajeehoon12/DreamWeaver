using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillGroup;
    [SerializeField] private float targetAngle = 90.0f;
    [SerializeField] private float rotateTime = 0.5f;

    private Quaternion rotation;
    private bool isRotating = false;

    private void Start()
    {
        rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isRotating)// 
        {
            StartRotation(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !isRotating)// 
        {
            StartRotation(-1);
        }
    }

    public void InitateRotation()
    {
        skillGroup.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0f);       
    }

    public void StartRotation(int num)
    {
        if (isRotating) return;

        float newZRotation = skillGroup.transform.eulerAngles.z + targetAngle * num;
        rotation = Quaternion.Euler(0, 0, newZRotation);

        skillGroup.transform.DORotateQuaternion(rotation, rotateTime).OnComplete(() =>
        {
            isRotating = false;
            CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().isRotating = true;
        });

        isRotating = true;
        CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().isRotating = true;

        if (num == 1)
        {
            CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().UpCount();
        }
        else
        {
            CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().DownCount();
        }
    }
}
