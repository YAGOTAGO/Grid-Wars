using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractAbility
{
    public abstract void DoAbility(HexNode node);
    public abstract List<HexNode> GetShape(HexNode mouseNode);
    public abstract TargetingType GetTargetingType();
    public abstract int Range { get; }

}

public enum TargetingType
{
    NORMAL, //Any tile that ability can passthrough
    AIREAL, //Any tile
    WALKABLE //Only tiles that are walkable
}