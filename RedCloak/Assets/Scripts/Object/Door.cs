using UnityEngine;

public class Door : MonoBehaviour
{
    //[SerializeField] private GameObject door;
    //[SerializeField] private Archer archerBoss;

    private static readonly int isDoorOpen = Animator.StringToHash("IsDoorOpen");
    Animator animator;

    public bool cameraHold = false;
    public bool isOpenStart = false;

    public bool selfObjective = false;
    public int selfNum = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (selfObjective)
        {
            CharacterManager.Instance.doors[selfNum] = this;
            isOpenStart = CharacterManager.Instance.GetDoorOpenStat(selfNum);
        }
    }

    private void OnEnable()
    {
        if (isOpenStart)
        {
            animator.SetBool(isDoorOpen, true); // for the test
        }
    }

    public void OpenDoor()
    {
        animator.SetBool(isDoorOpen, true);

        if (cameraHold)
        {
            CameraManager.Instance.MakeCameraShake(transform.position, 4f, 0.1f, 0.1f);
            AudioManager.instance.PlaySFX("DoorOpen", 0.2f);
            AudioManager.instance.PlaySamurai("EarthQuake", 0.2f);
        }
    }

    public void CloseDoor()
    {
        AudioManager.instance.PlaySFX("DoorClose", 0.2f);
        animator.SetBool(isDoorOpen, false);
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(Define.PLAYER_TAG))
    //    {
    //        door.SetActive(true);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag(Define.PLAYER_TAG))
    //    {
    //        door.SetActive(false);
    //        bossClear = true;
    //    }
    //}
}
