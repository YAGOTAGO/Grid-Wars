using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShape
{
    protected Vector3Int _up = new(1, 0);
    protected Vector3Int _down = new(-1, 0);
    protected Vector3Int _upRight = new(0, 1);
    protected Vector3Int _downRight = new(-1, 1);
    protected Vector3Int _upLeft = new(0, -1);
    protected Vector3Int _downLeft = new(-1, -1);
    public abstract List<HexNode> GetShape(HexNode mouseNode);
    public virtual void RotateShape() { } //does nothing unless implemented
}