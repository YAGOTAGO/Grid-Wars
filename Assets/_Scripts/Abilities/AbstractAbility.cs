using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractAbility : MonoBehaviour
{
    public abstract void DoAbility(HexNode node);
    public abstract List<HexNode> GetShape(HexNode mouseNode);
    public abstract bool CanAbilityPassthrough { get; }
    public abstract int Range { get; }

}
