using System.Collections;
using UnityEngine;

public class SwingEffect : MonoBehaviour
{
    private float shakeDuration = 1f;
    private float returnDuration = 1f;

    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;

    private Quaternion leftRotation;
    private Quaternion rightRotation;

    private Quaternion targetRotation;

    private bool isColliding = false;

    private void Start()
    {
        originalRotation = transform.rotation;

        leftRotation = Quaternion.Euler(0, 0, originalRotation.eulerAngles.z - 120f);
        rightRotation = Quaternion.Euler(0, 0, originalRotation.eulerAngles.z + 120f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            isColliding = true;

            Vector3 localContactPoint = transform.InverseTransformPoint(collision.contacts[0].point);
            Vector2 direction = new Vector2(Mathf.Sign(localContactPoint.x), 0);

            if (direction.x > 0)
            {
                targetRotation = leftRotation;
            }
            else
            {
                targetRotation = rightRotation;
            }

            if (shakeCoroutine == null)
            {
                shakeCoroutine = StartCoroutine(ShakeAndReturn(direction));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            isColliding = false;
        }
    }

    IEnumerator ShakeAndReturn(Vector2 direction)
    {
        float elapsed = 0.0f;

        Quaternion startRotation = transform.rotation;

        while (elapsed < shakeDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / shakeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;

        while (isColliding)
        {
            yield return null;
        }

        elapsed = 0.0f;
        startRotation = transform.rotation;
        while (elapsed < returnDuration)
        {
            float damper = Mathf.Cos(Mathf.PI * elapsed / returnDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, (1 + damper) / 2);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        shakeCoroutine = null;
    }
}