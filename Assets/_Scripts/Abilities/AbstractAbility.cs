using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractAbility : MonoBehaviour
{
    public abstract void Display(List<HexNode> shape);
    public abstract int GetRange();
    public abstract void DoAbility(HexNode node);
    public abstract List<HexNode> GetShape(HexNode mouseNode);

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
