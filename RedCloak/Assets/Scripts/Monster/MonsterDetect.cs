using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetect : MonoBehaviour
{
    [SerializeField] private MonsterState _state;
    
    [SerializeField] private Vector2 detectSize;
    [SerializeField] private Vector2 detectBoxPos;
    
    private void FixedUpdate()
    {
        Detecting();
    }

    private void Detecting()
    {
        bool detected = Physics2D.BoxCast((Vector2)transform.position + detectBoxPos, detectSize, 0, Vector2.zero, 0, 1 << 11);
        //bool detected = Physics2D.Raycast(transform.position + wallRayPos, transform.right, detectRange,
        //1 << 11);
        if (detected)
        {
            _state.currentState = (int)MonsterStateEnum.Chase;
        }
        else
        {
            _state.RemoveState(MonsterStateEnum.Chase);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube((Vector2)transform.position + detectBoxPos, detectSize);
        Gizmos.color = new Color(1, 1, 1, 0.2f);
    }
}
