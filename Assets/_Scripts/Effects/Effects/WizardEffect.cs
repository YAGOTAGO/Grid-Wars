using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effects/Innate/WizardPassive")]
public class WizardEffect : EffectBase
{
    [SerializeField] private Sprite _effectIcon;
    public override StatusType Type => StatusType.INNATE;
    public override string Description => "The Wizard deals +1 damage.";
    public override Sprite EffectIcon => _effectIcon;

    public override int OnDamageDeal(DamageInfo damageInfo)
    {
        return damageInfo.Val + 1;
    }
}
