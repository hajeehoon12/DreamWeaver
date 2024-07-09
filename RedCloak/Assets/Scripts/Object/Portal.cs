using Demo_Project;
using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalDestination;
    public bool isStartPortal = true;
    public FadeManager fadeManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PLAYER))
        {
            if (isStartPortal)
            {
                //collision.transform.position = portalDestination.position;

                StartCoroutine(TeleportAfterFade(collision.gameObject));
                isStartPortal = false;
            }
        }
    }

    private IEnumerator TeleportAfterFade(GameObject player)
    {
        fadeManager.FadeOut();
        yield return new WaitForSeconds(1f);

        player.transform.position = portalDestination.position;

        fadeManager.FadeIn();
        AudioManager.instance.PlaySFX("Success", 0.2f);
        yield return new WaitForSeconds(2f);

        isStartPortal = true;
    }
}
