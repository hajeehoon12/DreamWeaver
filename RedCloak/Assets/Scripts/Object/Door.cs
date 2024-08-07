using UnityEngine;

public class Door : MonoBehaviour
{
    //[SerializeField] private GameObject door;
    //[SerializeField] private Archer archerBoss;

    private static readonly int isDoorOpen = Animator.StringToHash("IsDoorOpen");
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //OpenDoor();
        animator.SetBool(isDoorOpen, true); // for the test
    }

    public void OpenDoor()
    {
        animator.SetBool(isDoorOpen, true);
        CameraManager.Instance.MakeCameraShake(transform.position, 4f, 0.1f, 0.1f);
        AudioManager.instance.PlaySFX("DoorOpen", 0.2f);
        AudioManager.instance.PlaySamurai("EarthQuake", 0.2f);
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
