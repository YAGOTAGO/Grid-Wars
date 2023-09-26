using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Damage/ShootingStarDamageAbility")]
public class ShootingStarDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _inRangeDamageAmount;
    [SerializeField] private int _outRangeDamageAmount;
    [SerializeField] private Shape _shape;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    public override Shape ShapeEnum => _shape;
    public override int Range => _range;
    public override string Prompt => _prompt;
    
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        int i = 0;
        foreach(HexNode node in shape)
        {
            int _damage = i < _range ? _inRangeDamageAmount : _outRangeDamageAmount;
            i++;
            CombatInfo dmgInfo = new(_damage, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
            int damageDone = CombatManager.Damage(dmgInfo);
            LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damageDone);
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
