using UnityEngine;

public class GolemFlyAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private MonsterController _controller;
    [SerializeField] private float radius;

    private void OnEnable()
    {
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
        AudioManager.instance.PlayMonster("GolemHit", 0.1f);
        return Physics2D.CircleCast(transform.position + transform.right + transform.up, radius, Vector2.zero, 0,
            1 << LayerMask.NameToLayer(Define.PLAYER));
    }

    void TelePortSound()
    {
        AudioManager.instance.PlayHoly("JumpDash", 0.15f);
    }

}
