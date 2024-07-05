using BehaviorDesigner.Runtime;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamage
{
    public bool GetHit = false;
    
    public float maxHealth;
    public float currentHealth;

    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BehaviorTree _behavior;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        _behavior.enabled = true;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _collider.enabled = true;
    }

    public void GetDamage(float damage)
    {
        if (currentHealth > damage)
        {
            GetHit = true;
            _animator.SetTrigger("GetHit");
            currentHealth -= damage;
        }
        else
        {
            _behavior.enabled = false;
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _collider.enabled = false;
            _animator.SetTrigger("Die");
        }
    }
}
