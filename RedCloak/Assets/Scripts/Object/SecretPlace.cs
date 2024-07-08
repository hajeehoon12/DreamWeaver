using UnityEngine;

public class SecretPlace : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER_TAG))
        {
            originColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER_TAG))
        {
            spriteRenderer.color = originColor;
        }
    }
}
