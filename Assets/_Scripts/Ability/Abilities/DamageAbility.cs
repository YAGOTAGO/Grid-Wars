using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/DamageAbility")]
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
    public override IEnumerator DoAbility(List<HexNode> shape)
    {
        foreach (HexNode node in shape)
        {
            DamageManager.Damage(new DamageInfo(_damageAmount, _damageType, CardSelectionManager.Instance.ClickedCharacter, node.GetCharacterOnNode()));
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
