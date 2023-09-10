using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Movement/DashAbility")]
public class DashAbility : AbilityBase
{
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override string Prompt => $"Dash up to {_range} hexes.";
    public override int Range => _range;

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
