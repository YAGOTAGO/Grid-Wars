using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Damage/TentacleDamageAbility")]
public class TentacleDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private Shape _shape;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    public override Shape ShapeEnum => _shape;
    public override int Range { get => _range; }
    public override string Prompt => _prompt;
   
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
            CombatInfo dmgInfo = new(damageAmount, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
            int damage = CombatManager.Damage(dmgInfo);
            LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);

        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
