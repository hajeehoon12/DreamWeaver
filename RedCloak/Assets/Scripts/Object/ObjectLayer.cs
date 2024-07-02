using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originColor;

    void Start()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            originColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.color = originColor;
        }
    }
}
