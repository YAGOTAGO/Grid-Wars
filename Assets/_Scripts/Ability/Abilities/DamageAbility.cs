using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbility : AbstractAbility
{
    private int _range;
    private readonly string _prompt;
    private readonly DamageInfo _damageInfo;
    private readonly TargetingType _targetingType;
    private AbstractShape _shape;

    public override AbstractShape Shape { get => _shape; set => _shape = value; }
    public override string Prompt => _prompt;
    public override int Range { get => _range; set => _range = value; }
    public override void DoAbility(HexNode node)
    { 
        DamageManager.Damage(new DamageInfo(_damageInfo.Val, _damageInfo.Type, CardSelectionManager.Instance.ClickedCharacter, node.CharacterOnNode));
        Debug.Log(node.ToString() + " in damage ability;");
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }

    public DamageAbility(int range, AbstractShape shape, DamageInfo dmgInfo, TargetingType tarType, string prompt)
    {
        _range = range;
        _prompt = prompt;
        _damageInfo = dmgInfo;
        _targetingType = tarType;
        _shape = shape;
    }
}
