using BehaviorDesigner.Runtime;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamage
{
    public bool GetHit = false;
    [SerializeField] private bool invincible;
    
    public float maxHealth;
    public float currentHealth;
    public int point;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BehaviorTree _behavior;
    
    private ParticleSystem light1Particle;
    private GameObject light1;
    private GameObject light2;

    private WaitForSeconds flashDelay = new WaitForSeconds(0.05f);
    private WaitForSeconds lightDelay = new WaitForSeconds(3f);
    
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
        if (invincible)
            return;
        StartCoroutine(FlashWhite());
        if (currentHealth > damage)
        {
            GetHit = true;
            //_animator.SetTrigger("GetHit");
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
        light1 = ObjectPool.Instance.GetFromPool(Define.OP_MonsterLightOut);
        light2 = ObjectPool.Instance.GetFromPool(Define.OP_MonsterLight);
        light1.transform.position = transform.position;
        light2.transform.position = transform.position + new Vector3(0, 0.5f);
        light1Particle = light1.GetComponent<ParticleSystem>();
        if (light2.TryGetComponent<LightEffect>(out LightEffect e))
        {
            e.point = point;
            //Debug.Log("give light point");
        }
        
        AudioManager.instance.PlayPitchSFX("Twinkle", 0.5f);
        light1.SetActive(true);
        yield return lightDelay;
        light2.SetActive(true);
        light1Particle.Stop();
        ObjectPool.Instance.ReleaseToPool(Define.OP_MonsterLightOut, light1);
    }

    private IEnumerator FlashWhite()
    {
        _renderer.material.SetFloat("_FlashAmount", 1.0f);
        yield return flashDelay;
        _renderer.material.SetFloat("_FlashAmount", 0f);
    }
}
