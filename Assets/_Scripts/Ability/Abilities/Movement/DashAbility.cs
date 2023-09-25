using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Movement/DashAbility")]
public class DashAbility : AbilityBase
{
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override string Prompt => $"Dash up to {Range} hexes.";
    public override int Range => GetRange();
    private int GetRange()
    {
        AbstractCharacter character = CardSelectionManager.Instance.SelectedCharacter;
        int range = _range;
        foreach (EffectBase ef in character.Effects)
        {
            if (!ef.CanMove()) { return 0; } //any effect makes us unable to move then return 0 range
            range = ef.OnMove(range);
        }
        return range;
    }

    public override AbstractShape Shape
    {
        get
        {
            _abstractShape ??= EnumToShape(global::Shape.DASH); //If abstract shape is null we set it
            return _abstractShape;
        }
        set => _abstractShape = value;
    }

    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        AbstractCharacter character = CardSelectionManager.Instance.SelectedCharacter;
        HexNode lastNode = shape[^1]; //get last element

        character.PutOnHexNode(lastNode, false);
        Tween tween = TweenManager.Instance.CharacterDash(character.gameObject, lastNode.transform.position);
        
        //Wait until Tween is done
        yield return tween.WaitForCompletion();

        LogManager.Instance.LogMovementAbility(card, CardSelectionManager.Instance.SelectedCharacter, shape.Count);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }
}
