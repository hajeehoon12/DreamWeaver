using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillGroup;
    [SerializeField] private float targetAngle = 90.0f;
    [SerializeField] private float rotateTime = 0.5f;

    private Quaternion rotation;
    private bool isRotating = false;

    public bool skill1 = false;
    public bool skill2 = false;
    public bool skill3 = false;

    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;

    private void Start()
    {
        rotation = Quaternion.Euler(0, 0, targetAngle);
        
        UpdateSkill();
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

    void InitiateSkillInfo()
    {
        PlayerShooting shoot = CharacterManager.Instance.Player.controller.shootProjectile;

        skill1 = shoot.PlayerSkill1;
        skill2 = shoot.PlayerSkill2;
        skill3 = shoot.PlayerSkill3;
    }

    public void UpdateSkill()
    {
        InitiateSkillInfo();
        if (skill1)
        {
            Skill1.GetComponent<Image>().DOFade(1, 1f);
        }
        else
        {
            Skill1.GetComponent<Image>().DOFade(0, 1f);
        }
        if (skill2)
        {
            Skill2.GetComponent<Image>().DOFade(1, 1f);
        }
        else
        {
            Skill2.GetComponent<Image>().DOFade(0, 1f);
        }
        if (skill3)
        {
            Skill3.GetComponent<Image>().DOFade(1, 1f);
        }
        else
        {
            Skill3.GetComponent<Image>().DOFade(0, 1f);
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
