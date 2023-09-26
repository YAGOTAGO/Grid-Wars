using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Damage/ShotgunDamageAbility")]
public class ShotgunDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _lineDamage;
    [SerializeField] private int _scatterDamage;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    private TargetingType _targetingType = TargetingType.NORMAL;
    private bool _firstCharacterHit = false;

    public override Shape ShapeEnum => global::Shape.SHOTGUN;
    public override int Range { get => _range; }
    public override string Prompt => _prompt;

    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        _firstCharacterHit = false;

        foreach (HexNode node in shape)
        {
            AbstractCharacter character = node.GetCharacterOnNode();
            if (character != null && !_firstCharacterHit)
            {
                CombatInfo dmgInfo = new(_lineDamage, _damageType, CardSelectionManager.Instance.SelectedCharacter, character);
                int damage = CombatManager.Damage(dmgInfo);
                LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);
                _firstCharacterHit = true;
            }
            else if(character != null && _firstCharacterHit)
            {
                CombatInfo dmgInfo = new(_scatterDamage, _damageType, CardSelectionManager.Instance.SelectedCharacter, character);
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
