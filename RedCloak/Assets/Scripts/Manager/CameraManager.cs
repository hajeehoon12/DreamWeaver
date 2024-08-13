using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Transform _player;
    //public MeshRenderer render;

    public GameObject map;
    public GameObject Sky;
    public GameObject Cave;

    private Vector2 _firstPos;

    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    Vector2 center;

    public Vector2 tempCenter;
    
    public Vector2 mapSize;

    public Vector2 tempMapSize;

    float screenHeight;
    float screenWidth;

    // 진동할 카메라의 transform
    public Transform shakeCamera;
    // 회전시킬 것인지를 판단할 변수
    public bool shakeRotate = false;
    // 초기 좌표와 회전값을 저장할 변수
    public Vector3 originPos;
    public Quaternion originRot;

    public bool isCameraShaking = false;

    public int stageNum = 0;

    public int saveStage;

    private float currentfov = 0;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        shakeCamera = transform;
    }

    private void Start()
    {
        _firstPos = _player.position;

        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Screen.width / Screen.height;
        saveStage = stageNum;
        SelectStage();
        
        //mapSize = map.GetComponent<Collider2D>().bounds.extents + new Vector3(0, 2, 0);
    }

    private void Update()
    {
        if (isCameraShaking) return;

        transform.position = _player.position + cameraPosition;//_player.position + cameraPosition;
        //render.material.mainTextureOffset = new Vector2((_firstPos.x - _player.position.x) / 300, 0);

        LimitCameraArea();
    }

    public void SaveStage()
    { 
        saveStage = stageNum;
    }

    public void SelectStage()
    {


        switch (saveStage)
        {
            case 0:
                break;
            case 1:
                CallStage1CameraInfo();
                break;
            case 2:
                CallStage2CameraInfo();
                break;
            case 3:
                CallStage3CameraInfo();
                break;
            case 4:
                CallStage4CameraInfo();
                break;
            default:
                break;

        }
    }

    public void SelectStage(int stage)
    {


        switch (stage)
        {
            case 0:
                break;
            case 1:
                CallStage1CameraInfo();
                break;
            case 2:
                CallStage2CameraInfo();
                break;
            case 3:
                CallStage3CameraInfo();
                break;
            case 4:
                CallStage4CameraInfo();
                break;
            default:
                break;

        }
    }

    void LimitCameraArea()
    {
        
        float borderx = mapSize.x - screenWidth;
        float clampX = Mathf.Clamp(transform.position.x, -borderx + center.x, borderx + center.x);

        float bordery = mapSize.y - screenHeight;
        float clampY = Mathf.Clamp(transform.position.y, -bordery + center.y, bordery + center.y);

        transform.position = new Vector3(clampX, clampY, -200f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }

    public void ModifyCameraInfo(Vector2 newMapSize, Vector2 newCenter)
    {
        tempMapSize = mapSize;
        tempCenter = center;

        mapSize = newMapSize;
        center = newCenter;

    }

    public void CallBackCameraInfo()
    {
        mapSize = tempMapSize;
        center = tempCenter;
    }

    IEnumerator StageUI(string name)
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.changeStage.FadeInStageUI(3f, name);
    }

    public void CallStage1CameraInfo()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("SadStory", 0.05f);

        mapSize = new Vector2(220, 40);
        center = new Vector2(70, -10);

        stageNum = 1;

        //UIManager.Instance.changeStage.FadeInStageUI(3f, "Forest of the Beginning");
        StartCoroutine(StageUI("Forest of the Beginning"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);

        UIManager.Instance.miniMap.SetMinimap(stageNum);
        MonsterDataManager.ToggleMonsters(stageNum);
    }
    public void CallStage1CameraInfo(string boss)
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("SadStory", 0.05f);

        mapSize = new Vector2(220, 40);
        center = new Vector2(70, -10);

        stageNum = 1;
        //UIManager.Instance.changeStage.FadeInStageUI(3f, "Forest of the Beginning");
        StartCoroutine(StageUI("Stage Boss Clear!"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);
    }

    public void CallStage2CameraInfo()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Chloe", 0.10f);

        mapSize = new Vector2(265, 65);
        center = new Vector2(120, -165);

        stageNum = 2;

        StartCoroutine(StageUI("Sky GrassLand"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);

        UIManager.Instance.miniMap.SetMinimap(stageNum);
        
        MonsterDataManager.ToggleMonsters(stageNum);
    }

    public void CallStage2CameraInfo(string boss)
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Chloe", 0.10f);

        mapSize = new Vector2(265, 65);
        center = new Vector2(120, -165);

        stageNum = 2;
        //StartCoroutine(StageUI("Sky GrassLand"));
        StartCoroutine(StageUI("Stage Boss Clear!"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);
    }

    public void CallStage3CameraInfo()
    {
        CharacterManager.Instance.Player.GetComponent<Light2D>().lightType = Light2D.LightType.Global;

        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Time", 0.1f);

        //UIManager.Instance.changeStage.FadeInStageUI(3f, "Crystal Cave");
        StartCoroutine(StageUI("Crystal Cave"));

        mapSize = new Vector2(266, 120);
        center = new Vector2(180, -380);

        stageNum = 3;

        Sky.gameObject.SetActive(false);
        Cave.gameObject.SetActive(true);

        UIManager.Instance.miniMap.SetMinimap(stageNum);
        
        MonsterDataManager.ToggleMonsters(stageNum);
    }

    public void CallStage3CameraInfo(string boss)
    {
        CharacterManager.Instance.Player.GetComponent<Light2D>().lightType = Light2D.LightType.Global;

        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Time", 0.1f);

        //UIManager.Instance.changeStage.FadeInStageUI(3f, "Crystal Cave");
        //StartCoroutine(StageUI("Crystal Cave"));
        StartCoroutine(StageUI("Stage Boss Clear!"));

        mapSize = new Vector2(266, 120);
        center = new Vector2(180, -380);

        stageNum = 3;

        Sky.gameObject.SetActive(false);
        Cave.gameObject.SetActive(true);
    }

    public void CallStage4CameraInfo()
    {
        CharacterManager.Instance.Player.GetComponent<Light2D>().lightType = Light2D.LightType.Global;

        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Promise", 0.1f);

        mapSize = new Vector2(280, 220);
        center = new Vector2(-350, -20);

        stageNum = 4;
        //UIManager.Instance.changeStage.FadeInStageUI(3f, "The Mountain");
        StartCoroutine(StageUI("The Mountain"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);

        UIManager.Instance.miniMap.SetMinimap(stageNum);
        
        MonsterDataManager.ToggleMonsters(stageNum);
    }

    public void CallStage4CameraInfo(string boss)
    {
        CharacterManager.Instance.Player.GetComponent<Light2D>().lightType = Light2D.LightType.Global;

        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayBGM("Promise", 0.1f);

        mapSize = new Vector2(280, 220);
        center = new Vector2(-350, -20);

        stageNum = 4;
        //UIManager.Instance.changeStage.FadeInStageUI(3f, "The Mountain");
        //StartCoroutine(StageUI("The Mountain"));
        StartCoroutine(StageUI("Stage Boss Clear!"));

        Sky.gameObject.SetActive(true);
        Cave.gameObject.SetActive(false);
    }


    public void ChangeFOV(float fov)
    {
        currentfov = GetComponent<Camera>().fieldOfView;
        DOTween.To(() => currentfov, x => currentfov =x, fov, 1f);
        StartCoroutine(ChangingFOV());
    }

    IEnumerator ChangingFOV()
    {
        float time = 0f;
        while (time < 1f)
        {
            GetComponent<Camera>().fieldOfView = currentfov;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    public void MakeCameraShake(Vector3 cameraPos, float duration, float Position, float Rotation)
    {
        StartCoroutine(Shake(cameraPos, duration, Position, Rotation));
    }


    public IEnumerator Shake(Vector3 cameraPos, float duration = 5f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        isCameraShaking = true;
        originPos = transform.position;
        originRot = transform.rotation;

        // 지나간 시간을 누적할 변수
        float passTime = 0.0f;
        // 진동시간동안 루프 돌림
        while (passTime < duration)
        {
            // 불규칙한 위치를 산출
            Vector3 shakePos = Random.insideUnitCircle;
            // 카메라의 위치를 변경
            shakePos.z = originPos.z / magnitudePos;
            shakeCamera.localPosition = shakePos * magnitudePos + cameraPos;

            // 불규칙한 회전을 사용할 경우
            if (shakeRotate)
            {
                // 펄린노이즈함수로 불규칙한 회전값 생성
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                // 카메라 회전값 변경
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            // 진동시간 누적
            passTime += Time.deltaTime;
            yield return null;
        }
        // 진동 후 원상복구
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;

        isCameraShaking = false;
    }

}
