using UnityEngine;

public class AStarNode
{
    public bool MoveAble { get; set; }
    public Vector2 NodePos { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }
    
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get { return gCost + hCost; } }

    public AStarNode parentNode { get; set; }

    public AStarNode(bool _moveAble, Vector2 _nodePos, int _gridX, int _gridY)
    {
        MoveAble = _moveAble;
        NodePos = _nodePos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
