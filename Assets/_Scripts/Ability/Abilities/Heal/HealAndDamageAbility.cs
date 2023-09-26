using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Heal/HealAndDamageAbility")]
public class HealAndDamageAbility : AbilityBase
{
    [SerializeField] private string _prompt;
    [SerializeField] private int _healAmount;
    [SerializeField] private int _damageAmount;
    [SerializeField] private DamageType _dmgType;
    [SerializeField] private int _range;
    [SerializeField] private Shape _shape;
    [SerializeField] private TargetingType _targetingType;
    public override string Prompt => _prompt;
    public override Shape ShapeEnum => _shape;
    public override int Range => _range;
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        foreach (HexNode node in shape)
        {
            AbstractCharacter sourceCharacter = CardSelectionManager.Instance.SelectedCharacter;
            AbstractCharacter targetCharacter = node.GetCharacterOnNode();

            if(targetCharacter == null) { continue; } //target null means move onto next node

            if (Database.Instance.IsAlly(targetCharacter))
            {
                CombatInfo healInfo = new(_healAmount, DamageType.HEAL, sourceCharacter, targetCharacter);
                int heal = CombatManager.Heal(healInfo);
                LogManager.Instance.LogCardHealAbility(card, healInfo, heal);
            }
            else
            {
                CombatInfo dmgInfo = new(_damageAmount, _dmgType, sourceCharacter, targetCharacter);
                int damage = CombatManager.Damage(dmgInfo);
                LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);
            }
         
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
