using Demo_Project;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody2D rb;
    public GameObject[] Detached;
    public float LifeTime;
    //public bool isGuided = false;

    Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.localScale = transform.localScale;
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

        //if (isGuided)
        //{ 
        //    player = CharacterManager.Instance.Player.GetComponent<Player>();
        //}
        
        Destroy(gameObject, LifeTime);
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed; // no minus
            //transform.position += transform.forward * (speed * Time.deltaTime);         
        }

        //if (isGuided)
        //{
            
        //    transform.LookAt(player.transform);
            
        //}
    }

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        speed = 0;

        //Debug.Log(collision.gameObject.name);

        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.transform.gameObject.TryGetComponent(out PlayerBattle battle))
            {
                if (!collision.transform.gameObject.GetComponent<PlayerController>().Rolling)
                {

                    bool dir = transform.position.x - collision.gameObject.transform.position.x > 0 ? false : true;

                    collision.transform.gameObject.GetComponent<PlayerController>().SetHitDir(dir);
                    battle.ChangeHealth(-1);



                }
            }
        }


        //Spawn hit effect on collision
        if (hit != null)
        {
            //hit.gameObject.la
            var hitInstance = Instantiate(hit, pos, rot);
            hitInstance.transform.localScale = transform.localScale;
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
