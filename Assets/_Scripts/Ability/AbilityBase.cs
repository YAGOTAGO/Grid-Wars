using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    public abstract void DoAbility(HexNode node); //Will do its ability using given hexnode if needed
    public abstract string Prompt { get; } //What to tell the player before confirming
    public virtual AbstractShape Shape { get; set; }
    public abstract TargetingType GetTargetingType();
    public virtual int Range { get => _range; set => _range = (value >= -1) ? value : -1; }

    private int _range = -1; //default range of ability is -1 (aka no range)

    public List<HexNode> GetShape(HexNode mouseNode)
    {
        return Shape.GetShape(mouseNode, this);
    }

    public AbstractShape EnumToShape(Shape shapeEnum)
    {
        switch (shapeEnum)
        {
            case global::Shape.CIRCLE: return new CircleShape();
            case global::Shape.PATHFIND: return new PathfindShape();
            case global::Shape.LINE: return new LineShape();
            case global::Shape.MOUSEHEX: return new SingleHexShape();
        }

        Debug.LogWarning("Shape enum exists but hasnt been assigned in AbilityBase script");
        return null;
    }

}

public enum TargetingType
{
    NORMAL, //Any tile that ability can passthrough
    AIREAL, //Any tile
    WALKABLE, //Only tiles that are walkable
    NONE, //For abilities that dont care about surfaces (ie: draw)
    SELF //Abilities that apply on character chosen
}