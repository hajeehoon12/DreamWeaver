using UnityEngine;

public class FloorSlimeAttack : MonoBehaviour, IMobAttack
{
    [SerializeField] private Vector2 attackScope;
    [SerializeField] private MonsterController _controller;

    private void OnEnable()
    {
        _controller.MobAttack = this;
    }

    public bool PerformAttack()
    {
        AudioManager.instance.PlayMonsterPitch("Whip1", 0.1f);
        return Physics2D.BoxCast(transform.position, attackScope, 0f, Vector2.zero, 0,
            1 << LayerMask.NameToLayer(Define.PLAYER));
    }
}
