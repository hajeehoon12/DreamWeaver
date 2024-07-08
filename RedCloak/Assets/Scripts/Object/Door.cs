using UnityEngine;

public class Door : MonoBehaviour
{
    public bool bossClear = false;
    [SerializeField] private GameObject door;

    [SerializeField] private Archer archerBoss;

    private void OpenDoor()
    {
        //if (archerBoss.isBossDie)
        //{
        //    AudioManager.instance.PlaySFX("BoxOpen", 1f);
        //    door.SetActive(false);
        //}
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
