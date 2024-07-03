using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    
    public float duration = 0.5f;
    public float strength = 0.3f;
    public int vibrato = 10;
    public float randomness = 90f;

    private void LateUpdate()
    {
        
    }

    public void Shake()
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }
}
