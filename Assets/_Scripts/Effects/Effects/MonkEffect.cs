using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effects/Innate/MonkPassive")]
public class MonkEffect : EffectBase
{
    [SerializeField] private Sprite _icon;
    public override StatusType Type => StatusType.INNATE;

    public override string Description => "The Monk has +1 move.";

    public override Sprite EffectIcon => _icon;

    public override int OnMove(int moveAmount)
    {
        return moveAmount + 1;
    }
}
