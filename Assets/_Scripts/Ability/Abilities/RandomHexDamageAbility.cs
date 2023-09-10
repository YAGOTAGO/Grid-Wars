using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/RandomHexDamageAbility")]
public class RandomHexDamageAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damageAmount;
    [SerializeField] private Shape _shape;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;
    [SerializeField] private int _numHexesToDamage;

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
        ShuffleList(shape);

        for(int i=0; i<_numHexesToDamage; i++)
        {
            DamageInfo dmgInfo = new(_damageAmount, _damageType, CardSelectionManager.Instance.SelectedCharacter, shape[i].GetCharacterOnNode());
            int damage = DamageManager.Damage(dmgInfo);
            LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);

        }
        yield break;
    }

    /// <summary>
    /// Fisher-Yates algorithm to shuffle a list
    /// </summary>
    private void ShuffleList(List<HexNode> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
