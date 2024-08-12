using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject FirePoint;
    public Camera Cam;
    public float MaxLength;
    public GameObject[] Prefabs;

    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;

    [Header("GUI")]
    private float windowDpi;
    private int Prefab;
    private GameObject Instance;
    private float hSliderValue = 0.1f;
    private float fireCountdown = 0f;

    //Double-click protection
    private float buttonSaver = 0f;

    public GameObject currentSkill;

    public bool isRotating = false;

    public bool PlayerSkill1 = true;
    public bool PlayerSkill2 = true;
    public bool PlayerSkill3 = true;

    void Start()
    {
        Counter(0);
    }

    void Update()
    {

        fireCountdown -= Time.deltaTime;

        ////To change projectiles
        //if ((Input.GetKey(KeyCode.Q) && !isRotating))// left button
        //{
        //    buttonSaver = 0f;
        //    Counter(-1);
        //}
        //if ((Input.GetKey(KeyCode.E) && !isRotating))// right button // buttonSaver >= 0.4f
        //{
        //    buttonSaver = 0f;
        //    Counter(+1);
        //}
        buttonSaver += Time.deltaTime;
        currentSkill = Prefabs[Prefab];
    }

    public void UpCount()
    {
        buttonSaver = 0f;
        Counter(+1);
    }

    public void DownCount()
    {
        buttonSaver = 0f;
        Counter(-1);
    }

    public bool CheckSkill(int num) // Can Play Current Skill
    {
        bool result = true;

        switch (num)
        {
            case 1:
                if (!PlayerSkill1)
                    result = false;
                break;
            case 2:
                if(!PlayerSkill2)
                    result = false;
                break;
            case 3:
                if(!PlayerSkill3)
                    result = false;
                break;
            default:
                break;
        }
        //Debug.Log("Check");
        return result;
    }


    public void FireProjectile()
    {
        if (Prefab == 0) return;
        else
        {
            if (CharacterManager.Instance.Player.stats.playerMP >= Prefabs[Prefab].GetComponent<PlayerProjectile>().Mana && CheckSkill(Prefab))
            {
                CharacterManager.Instance.Player.stats.playerMP -= Prefabs[Prefab].GetComponent<PlayerProjectile>().Mana;
            }
            else return;
        }

        GameObject obj = Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);
        obj.transform.localEulerAngles += new Vector3(0, 180, 0);
        obj.transform.localScale *= 4;
    }

    public void RapidFireProjectile()
    {
        if (Prefab == 0) return;
        GameObject obj = Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);
        obj.transform.localEulerAngles += new Vector3(0, 180, 0);
        obj.transform.localScale *= 4;
        fireCountdown = 0;
        fireCountdown += hSliderValue;
    }


    // To change prefabs (count - prefab number)
    void Counter(int count)
    {
        Prefab += count;
        if (Prefab > Prefabs.Length - 1)
        {
            Prefab = 0;
        }
        else if (Prefab < 0)
        {
            Prefab = Prefabs.Length - 1;
        }
    }



}
