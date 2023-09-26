using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effects/Innate/WizardPassive")]
public class WizardEffect : EffectBase
{
    [SerializeField] private Sprite _effectIcon;
    public override StatusType Type => StatusType.INNATE;
    public override string Description => $"{passive} The Wizard deals +1 damage.";
    public override Sprite EffectIcon => _effectIcon;

    //Deals +1 on all wizard class cards
    public override int OnDamageDeal(CombatInfo damageInfo)
    {
        if(CardSelectionManager.Instance.SelectedCard.Class == Class.Wizard)
        {
            return damageInfo.Value + 1;
        }
        else
        {
            return damageInfo.Value;
        }
        
    }
}
