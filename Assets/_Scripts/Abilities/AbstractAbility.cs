using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractAbility : MonoBehaviour
{
    public void Display(List<HexNode> shape)
    {
        foreach (HexNode node in shape)
        {
            HighlightManager.Instance.PathHighlight(node.GridPos);
        }
    }

    public abstract int GetRange();
    public abstract void DoAbility(HexNode node);
    public abstract List<HexNode> GetShape(HexNode mouseNode);
    public abstract bool ShouldDisplayRange();

    //This means an ability script must live on a player game object
    public HexNode GetHexPlayerIsOn() 
    { 
        return SelectionManager.Instance.SelectedChar.GetNodeOn(); 
    }

    public void SetCurrentAbility()
    {
        AbilityManager.Instance.SetSelectedAbility(this);
    }
   
}
