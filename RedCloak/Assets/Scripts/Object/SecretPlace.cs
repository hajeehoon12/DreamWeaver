using UnityEngine;

public class SecretPlace : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            originColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1, 1, 1, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            spriteRenderer.color = originColor;
        }
    }
}
