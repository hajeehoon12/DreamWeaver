using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public SpriteRenderer fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       
    }

    public void FadeIn()
    {
        fadeImage = CameraManager.Instance.GetComponentsInChildren<SpriteRenderer>()[1];
        StartCoroutine(Fade(1f, 0f));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeIn(float duration)
    {
        fadeImage = CameraManager.Instance.GetComponentsInChildren<SpriteRenderer>()[1];
        StartCoroutine(Fade(1f, 0f, duration));
    }


    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha) // , float fadeDurations
    {
        fadeImage = CameraManager.Instance.GetComponentsInChildren<SpriteRenderer>()[1];
        float leadTime = 0f;
        Color color = fadeImage.color;

        //fadeDuration = fadeDurations;

        while (leadTime < fadeDuration)
        {
            leadTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, leadTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float fadeDurations) // ,
    {
        fadeImage = CameraManager.Instance.GetComponentsInChildren<SpriteRenderer>()[1];
        float leadTime = 0f;
        Color color = fadeImage.color;

        //fadeDuration = fadeDurations;

        while (leadTime < fadeDuration)
        {
            leadTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, leadTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}
