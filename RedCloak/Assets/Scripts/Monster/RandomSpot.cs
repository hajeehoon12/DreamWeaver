using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RandomSpot : Action
{
    public SharedVector2 BottomLeft;
    public SharedVector2 UpperRight;
    public SharedVector2 RandomPoint;
    public SharedBool NeedRandom;
    private bool result;
    
	public override TaskStatus OnUpdate()
    {
        result = false;
        if (NeedRandom.Value)
        {
            float randomX = Random.Range(BottomLeft.Value.x, UpperRight.Value.x);
            float randomY = Random.Range(BottomLeft.Value.y, UpperRight.Value.y);
            RandomPoint.Value = new Vector2(randomX, randomY);
            NeedRandom.Value = false;
            result = true;
        }
        return result ? TaskStatus.Success : TaskStatus.Failure;
    }
}