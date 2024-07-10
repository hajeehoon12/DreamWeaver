using BehaviorDesigner.Runtime;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamage
{
    public bool GetHit = false;
    
    public float maxHealth;
    public float currentHealth;

    [SerializeField] private GameObject light1Prefab;
    [SerializeField] private GameObject light2Prefab;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BehaviorTree _behavior;
    
    private ParticleSystem light1Particle;
    private GameObject light1;
    private GameObject light2;

    private WaitForSeconds lightDelay = new WaitForSeconds(3f);
    
    private void Start()
    {
        currentHealth = maxHealth;

        light1 = Instantiate(light1Prefab);
        light2 = Instantiate(light2Prefab);

        light1Particle = light1.GetComponent<ParticleSystem>();
        light1.SetActive(false);
        light2.SetActive(false);
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
            _behavior.DisableBehavior();
            _behavior.EnableBehavior();
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

    public IEnumerator SpawnLight()
    {
        AudioManager.instance.PlayPitchSFX("Twinkle", 0.5f);
        light1.transform.position += transform.position;
        light2.transform.position += transform.position;
        light1.SetActive(true);
        yield return lightDelay;
        light2.SetActive(true);
        light1Particle.Stop();
    }
}
