using UnityEngine;

public class Door : MonoBehaviour
{
    public bool bossClear = false;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(false);
            bossClear = true;
        }
    }
}
