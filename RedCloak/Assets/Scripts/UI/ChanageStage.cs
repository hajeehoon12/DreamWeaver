using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ChanageStage : MonoBehaviour
{
    public GameObject stageUI;
    public TextMeshProUGUI stageName;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = stageUI.GetComponent<CanvasGroup>();
        if(canvasGroup == null )
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        stageUI.SetActive(false);
    }

    public void FadeInStageUI(float duration, string stage)
    {
        //SetStageText();
        stageUI.SetActive(true);
        stageName.text = stage;
        canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            StartCoroutine(WaitFadeOut(2f, 1f));
        });
    }

    private IEnumerator WaitFadeOut(float waitTime, float fadeOutDuration)
    {
        yield return new WaitForSeconds(waitTime);
        FadeOutStageUI(fadeOutDuration);
    }

    public void FadeOutStageUI(float duration)
    {
        canvasGroup.DOFade(0f, duration).SetEase(Ease.InOutQuad).OnComplete(() => stageUI.SetActive(false));
    }
}
