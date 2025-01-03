using System.Collections;
using UnityEngine;

public class RewardChest : MonoBehaviour
{
    [SerializeField] SpriteRenderer chestIcon;
    [SerializeField] GameObject upArrowIcon;
    public Sprite openedChestSprite;

    public ItemData dropData;
    public GameObject ItemLight;

    public int index;
    public bool isOpen;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (isOpen)
        {
            chestIcon.sprite = openedChestSprite;
        }
    }

    public void OpenChest()
    {
        chestIcon.sprite = openedChestSprite;
        AudioManager.instance.PlaySFX("Boxopen", 0.2f);
        upArrowIcon.SetActive(false);
        RewardBoxDataManager.ChangeOpenStat(index);
        isOpen = true;
        //GameManager.Instance.spawnManager.SpawnBoxRewardItem(this.transform);
    }

    public void ThrowItem()
    {
        StartCoroutine(ItemLoad());
    }

    IEnumerator ItemLoad()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject Items = Instantiate(ItemLight, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX("ItemThrow", 0.2f);
        Items.GetComponentInParent<Rigidbody2D>().AddForce(new Vector3(0,10,0), ForceMode2D.Impulse);
        Items.GetComponentInChildren<ItemLight>().RewardData = dropData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER) && !isOpen)
        {
            upArrowIcon.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER))
        {
            if (Input.GetKey(KeyCode.UpArrow) && !isOpen)
            {

                OpenChest();
                GetComponent<Collider2D>().enabled = false;
                ThrowItem();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER) && isOpen)
        {
            upArrowIcon.SetActive(false);
        }
    }
}
