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
    public abstract List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability);
    public virtual void RotateShape() { } //does nothing unless implemented
}

public enum Shape { 
    CIRCLE, //Includes all neighboor nodes and center node
    PATHFIND, //path find
    LINE, //Straight line in direction of mouse
    MOUSEHEX, //Mouse hex
    SHOTGUN, //line up to X range then 3 hexes behind
    TENTACLE, //3 lines coming out of character
    THREEBOLT, // 3 bolts going from mouse and directly next to left and right
    WAVE, // 3 Hexes closest to mouse, can keep going
    DASH, // straight line variable length within range
    SKIP, //hex, skip, hex, so on in a straight line
    SHOOTINGSTAR, //line and then 3 nodes at the end
}
