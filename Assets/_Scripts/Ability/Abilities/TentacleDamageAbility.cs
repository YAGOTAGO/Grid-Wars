using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/TentacleDamageAbility")]
public class TentacleDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
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
    /// <summary>
    /// Deals damage equal to number of characters targeted
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="card"></param>
    /// <returns></returns>
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        int damageAmount = 0;
        foreach (HexNode node in shape)
        {
            if(node.GetCharacterOnNode() != null)
            {
                damageAmount++;
            }
        }

        foreach (HexNode node in shape)
        {
            DamageInfo dmgInfo = new(damageAmount, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
            int damage = DamageManager.Damage(dmgInfo);
            LogManager.Instance.LogDamageAbility(card, dmgInfo, damage);

        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
