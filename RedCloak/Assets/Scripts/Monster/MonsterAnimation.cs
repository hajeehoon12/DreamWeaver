using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int run = Animator.StringToHash("Run");

    public void SetRunAnim(bool state)
    {
        _animator.SetBool(run, state);
    }
}
