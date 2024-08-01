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
        return Physics2D.CircleCast(transform.position + transform.right + transform.up, radius, Vector2.zero, 0,
            1 << LayerMask.NameToLayer(Define.PLAYER));
    }
}
