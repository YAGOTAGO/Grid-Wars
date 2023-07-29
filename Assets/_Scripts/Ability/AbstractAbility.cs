using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractAbility
{
    //Should probably trigger a coroutine
    public abstract void DoAbility(HexNode node);
    public abstract string Prompt { get; }
    public virtual List<HexNode> GetShape(HexNode mouseNode) { return new List<HexNode>(); }
    public abstract TargetingType GetTargetingType();
    public virtual int Range { get=> _range; set=> _range = (value >= -1) ? value : -1; }

    private int _range = -1;
}

public enum TargetingType
{
    NORMAL, //Any tile that ability can passthrough
    AIREAL, //Any tile
    WALKABLE, //Only tiles that are walkable
    NONE, //For abilities that dont care about surfaces (ie: draw)
    SELF //Abilities that apply on character chosen
}