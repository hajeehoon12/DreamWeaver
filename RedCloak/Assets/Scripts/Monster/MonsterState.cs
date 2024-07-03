using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterStateEnum {
    Idle = 1 << 0,
    Patrol = 1 << 1,
    Detect = 1 << 2,
    Chase = 1 << 3,
    Attack = 1 << 4
}

public class MonsterState : MonoBehaviour
{
    [field:SerializeField]public int currentState { get; set; } = 0;

    public bool CheckState(MonsterStateEnum states)
    {
        if ((currentState & (int)states) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddState(MonsterStateEnum state)
    {
        currentState |= (int)state;
    }

    public void RemoveState(MonsterStateEnum state)
    {
        currentState &= ~(int)state;
    }
}
