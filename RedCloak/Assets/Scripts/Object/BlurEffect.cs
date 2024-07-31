using UnityEngine;

public class BlurEffect : MonoBehaviour
{
    private float collisionTime = 0f;
    private float releaseTime = 0f;
    private bool isColliding = false;
    private bool isRelease = false;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            isColliding = true;
            isRelease = false;
            releaseTime = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            if (collisionTime <= 3f)
            {
                isRelease = false;
            }
            else
            {
                isRelease = true;
            }

            isColliding = false;
            collisionTime = 0f;
        }
    }

    private void Update()
    {
        if (isColliding)
        {
            collisionTime += Time.deltaTime;

            if (collisionTime > 3f)
            {
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;
                isColliding = false;
                AudioManager.instance.PlayPitchSFX("PlatformOff", 0.2f);
                collisionTime = 0f;
            }
        }

        if (isRelease)
        {
            releaseTime += Time.deltaTime;

            if (releaseTime > 8f)
            {
                AudioManager.instance.PlayPitchSFX("PlatformOn", 0.2f);
                spriteRenderer.enabled = true;
                boxCollider.enabled = true;
                isRelease = false;
                releaseTime = 0f;
            }
        }
    }
}
