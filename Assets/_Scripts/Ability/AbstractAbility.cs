using System.Collections;
using System.Collections.Generic;

public abstract class AbstractAbility
{
    //Should probably trigger a coroutine
    public abstract void DoAbility(HexNode node); //Will do its ability using given hexnode if needed
    public abstract string Prompt { get; } //What to tell the player before confirming
    public virtual AbstractShape Shape { get; set; }
    public abstract TargetingType GetTargetingType();
    public virtual int Range { get=> _range; set=> _range = (value >= -1) ? value : -1; }

    private int _range = -1; //default range of ability is -1 (aka no range)

    public List<HexNode> GetShape(HexNode mouseNode)
    {
        //return Shape.GetShape(mouseNode);
        return null;
    }
}


