using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            AudioManager.instance.PlayPitchSFX("BubblePop", 0.2f);
            gameObject.SetActive(false);
        }
    }

    IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(true);
    }
}
