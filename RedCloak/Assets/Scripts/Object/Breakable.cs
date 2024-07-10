using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            AudioManager.instance.PlaySFX("Success", 0.2f);
            gameObject.SetActive(false);
        }
    }
}
