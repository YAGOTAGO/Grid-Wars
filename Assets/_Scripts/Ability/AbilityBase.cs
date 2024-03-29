using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    public abstract IEnumerator DoAbility(List<HexNode> shape, CardBase card); //Will do its ability using given hexnode if needed
    public abstract string Prompt { get; } //What to tell the player before confirming
    public abstract TargetingType GetTargetingType();
    public virtual Shape ShapeEnum { get; }
    public virtual bool IsSpecialPathfind { get => false; }
    public virtual int Range { get => -1; }
    public List<HexNode> GetShape(HexNode mouseNode, HexNode startNode)
    {
        return Shape.GetShape(mouseNode, startNode, this);
    }

    private AbstractShape _shape;
    public AbstractShape Shape 
    {
        get
        {
            _shape ??= EnumToShape(ShapeEnum);
            return _shape;
        }
    }

    public AbstractShape EnumToShape(Shape shapeEnum)
    {
        switch (shapeEnum)
        {
            case global::Shape.CIRCLE: return new CircleShape();
            case global::Shape.PATHFIND: return new PathfindShape();
            case global::Shape.LINE: return new LineShape();
            case global::Shape.MOUSEHEX: return new SingleHexShape();
            case global::Shape.SHOTGUN: return new ShotgunShape();
            case global::Shape.TENTACLE: return new TentacleShape();
            case global::Shape.THREEBOLT: return new ThreeBoltsShape();
            case global::Shape.WAVE: return new WaveShape();
            case global::Shape.DASH: return new DashShape();
            case global::Shape.SKIP: return new SkipHexShape();
            case global::Shape.SHOOTINGSTAR: return new ShootingStarShape();
        }

        Debug.LogWarning("Shape enum exists but hasnt been assigned in AbilityBase script");
        return null;
    }

}

public enum TargetingType
{
    NORMAL=0, //Any tile that ability can passthrough
    AIREAL=1, //Any tile
    WALKABLE=2, //Only tiles that are walkable
    NONE=3, //For abilities that dont care about surfaces (ie: draw)
    SELF=4, //Abilities that apply on character chosen
    ENEMYCHARACTERS =5, //All the hexes that enemy characters are on
    ALLYCHARACTERS = 6, //All hexes that ally characters are on
}