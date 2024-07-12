using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private float targetAngle = 60.0f;
    [SerializeField] private float speed = 50f;

    private Quaternion rotation;
    private bool isRotating = false;

    private void Start()
    {
        rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void Update()
    {
        if(isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);

            if(Quaternion.Angle(transform.rotation, rotation) < 0.1f)
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
        rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + targetAngle * num);

        //transform.DORotateQuaternion(Quaternion.Euler(0,0,transform.eulerAngles.z + targetAngle), 1f);

        isRotating = true;
        CharacterManager.Instance.Player.GetComponentInChildren<PlayerShooting>().isRotating = true;
    }
}
