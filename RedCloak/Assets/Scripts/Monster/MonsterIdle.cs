using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterIdle : MonoBehaviour
{
    [SerializeField] private MonsterState _state;

    private WaitForFixedUpdate afterFixedUpdate = new WaitForFixedUpdate();

    private void Start()
    {
        StartCoroutine(SetIdle());
    }

    private IEnumerator SetIdle()
    {
        while (true)
        {
            if (_state.CheckState(MonsterStateEnum.Patrol))
            {
                yield return new WaitForSeconds(Random.Range(5,9));
            
                if (_state.CheckState(MonsterStateEnum.Patrol))
                {
                    _state.currentState = (int)MonsterStateEnum.Idle;
                }
            
                yield return new WaitForSeconds(Random.Range(2,4));

                if (_state.CheckState(MonsterStateEnum.Idle))
                {
                    _state.RemoveState(MonsterStateEnum.Idle);
                }
            }
            
            yield return afterFixedUpdate;
        }
    }
}
