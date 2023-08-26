using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/DrawAbility")]
public class DrawAbility : AbilityBase
{
    [SerializeField] private int _drawAmount;
    [SerializeField] private string _prompt;

    public override string Prompt => _prompt;

    public override void DoAbility(HexNode node)
    {
        DeckManager.Instance.DeckDraw(_drawAmount);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.NONE;
    }
}
