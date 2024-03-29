using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/DrawAbility")]
public class DrawAbility : AbilityBase
{
    [SerializeField] private int _drawAmount;
    [SerializeField] private string _prompt;

    public override string Prompt => _prompt;

    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {

        DeckManager.Instance.DeckDraw(_drawAmount);

        yield return new WaitUntil(()=> ActionQueue.Instance.IsQueueStopped());
        LogManager.Instance.LogDrawAbility(card);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.NONE;
    }
}
