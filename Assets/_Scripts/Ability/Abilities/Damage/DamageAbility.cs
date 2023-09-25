using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Damage/DamageAbility")]
public class DamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damageAmount;
    [SerializeField] private Shape _shape;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override int Range => _range;
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
        foreach (HexNode node in shape)
        {
            DamageInfo dmgInfo = new(_damageAmount, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
            int damage = DamageManager.Damage(dmgInfo);
            LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);
            
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
