using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShape
{
    protected static Vector3Int up = new(1, -1, 0);
    protected static Vector3Int down = new(-1, 1, 0);
    protected static Vector3Int upRight = new(0, -1, 1);
    protected static Vector3Int downRight = new(-1, 0, 1);
    protected static Vector3Int upLeft = new(1, 0, -1);
    protected static Vector3Int downLeft = new(0, 1, -1);

    protected List<Vector3Int> cuberCoordSides = new() { up, down, upRight, downRight, upLeft, downLeft};
    public abstract List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability);
    
    /// <summary>
    /// Will give the nodes that the ability can potentially target
    /// </summary>
    /// <param name="startNode">Node that this range starts at</param>
    /// <param name="range">The range</param>
    /// <returns>A set of nodes that are the range</returns>
    public abstract List<HexNode> Range(HexNode startNode, AbilityBase ability);
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
