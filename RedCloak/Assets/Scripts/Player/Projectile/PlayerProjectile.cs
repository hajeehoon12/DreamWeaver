using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public float damageRate = 1f;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody2D rb;
    public GameObject[] Detached;
    public bool isGuided = false;
    public float detectRange = 20f;
    public float Mana = 5;

    public float projectileDuration;
    public LayerMask enemyLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }

        if (isGuided)
        {
            RaycastHit2D targetPos = Physics2D.CircleCast(transform.position, detectRange, Vector2.zero,0f, enemyLayer);
            if (targetPos && !targetPos.transform.gameObject.CompareTag(Define.PROJECTILE))
            {
                transform.LookAt(new Vector3(targetPos.transform.position.x, targetPos.transform.position.y + 0.5f, 0));
            }
        
        }

        Destroy(gameObject, projectileDuration);
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed; // no minus
            //transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    }

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        speed = 0;

        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (collision.gameObject.layer == LayerMask.NameToLayer(Define.ENEMY) || collision.gameObject.layer == LayerMask.NameToLayer(Define.BOSS))
        {
            if (collision.transform.gameObject.TryGetComponent(out IDamage monster))
            {
                AudioManager.instance.PlayPitchSFX("GreenSlash", 0.05f);
                float damage = CharacterManager.Instance.Player.controller.attackRate * damageRate;
                monster.GetDamage(damage);
            }
        }




        //Spawn hit effect on collision
        if (hit != null)
        {

            if (hit.CompareTag(Define.PROJECTILE))
            {
                AudioManager.instance.PlayPitchSFX("Parrying", 0.25f);
            }

            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }
}
