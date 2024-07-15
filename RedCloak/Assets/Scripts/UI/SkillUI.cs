using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillGroup;
    [SerializeField] private float targetAngle = 60.0f;
    [SerializeField] private float speed = 50f;

    private Quaternion rotation;
    private bool isRotating = false;

    private void Start()
    {
        rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    private void Update()
    {
        if(isRotating)
        {
            skillGroup.transform.rotation = Quaternion.Lerp(skillGroup.transform.rotation, rotation, Time.deltaTime * speed);

            if(Quaternion.Angle(skillGroup.transform.rotation, rotation) < 0.1f)
            {
                isRotating = false;
                CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().isRotating = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isRotating)// 
        {
            StartRotation(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !isRotating)// 
        {
            StartRotation(-1);
        }
    }

    public void StartRotation(int num)
    {
        skillGroup.transform.rotation = Quaternion.Euler(0, 0, skillGroup.transform.eulerAngles.z + targetAngle * num);
        
        //transform.DORotateQuaternion(Quaternion.Euler(0,0,transform.eulerAngles.z + targetAngle), 1f);

        isRotating = true;
        CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().isRotating = true;
    }
}
