using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuide : MonoBehaviour, IDamage
{
    Player player;
    ParticleSystem children;

    private void Start()
    {
        player = CharacterManager.Instance.Player.GetComponent<Player>();
        children = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        
        transform.LookAt(player.transform.position + new Vector3(0,1,0));
        if (children == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = children.transform.position;
        children.transform.position = transform.position;
    }

    public void GetDamage(float attackRate)
    {
        AudioManager.instance.PlaySamurai("DefendSuccess", 0.2f);
        Destroy(gameObject);
    }
}
