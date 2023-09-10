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

    private AbstractShape _abstractShape;
    public override int Range { get => _range; }
    public override string Prompt => _prompt;
    public override AbstractShape Shape
    {
        get
        {
            _abstractShape ??= EnumToShape(_shape); //If abstract shape is null we set it
            return _abstractShape;
        }
        set => _abstractShape = value;
    }
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        int i = 0;
        foreach(HexNode node in shape)
        {
            int _damage = i < _range ? _inRangeDamageAmount : _outRangeDamageAmount;
            i++;
            DamageInfo dmgInfo = new(_damage, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
            int damageDone = DamageManager.Damage(dmgInfo);
            LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damageDone);
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
