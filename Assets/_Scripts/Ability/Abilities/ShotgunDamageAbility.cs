using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/ShotgunDamageAbility")]

public class ShotgunDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _lineDamage;
    [SerializeField] private int _scatterDamage;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    private TargetingType _targetingType = TargetingType.NORMAL;
    private AbstractShape _abstractShape;
    private bool _firstCharacterHit = false;

    public override int Range { get => _range; }
    public override string Prompt => _prompt;
    public override AbstractShape Shape
    {
        get
        {
            _abstractShape = new ShotgunShape();
            return _abstractShape;
        }
        set => _abstractShape = value;
    }
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        _firstCharacterHit = false;

        foreach (HexNode node in shape)
        {
            AbstractCharacter character = node.GetCharacterOnNode();
            if (character != null && !_firstCharacterHit)
            {
                DamageInfo dmgInfo = new(_lineDamage, _damageType, CardSelectionManager.Instance.SelectedCharacter, character);
                int damage = DamageManager.Damage(dmgInfo);
                LogManager.Instance.LogDamageAbility(card, dmgInfo, damage);
                _firstCharacterHit = true;
            }
            else if(character != null && _firstCharacterHit)
            {
                DamageInfo dmgInfo = new(_scatterDamage, _damageType, CardSelectionManager.Instance.SelectedCharacter, character);
                int damage = DamageManager.Damage(dmgInfo);
                LogManager.Instance.LogDamageAbility(card, dmgInfo, damage);
            }

        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
