using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Movement/WalkAbility")]
public class WalkAbility : AbilityBase
{
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override string Prompt => $"Move up to {_range} hexes.";
    public override int Range => _range;


    public override AbstractShape Shape {
        get
        {
            _abstractShape ??= EnumToShape(global::Shape.PATHFIND); //If abstract shape is null we set it
            return _abstractShape;
        }
        set => _abstractShape = value;
    }

    private IEnumerator WalkRoutine(HexNode target)
    {
        AbstractCharacter character = CardSelectionManager.Instance.SelectedCharacter;

        character.PutOnHexNode(target, false); //logic for setting surface variables

        Tween characterMove = TweenManager.Instance.CharacterMove(character.gameObject, target.transform.position);
        yield return characterMove.WaitForCompletion();
    }

    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        foreach (HexNode node in shape)
        {
            ActionQueue.Instance.EnqueueMethod(() => WalkRoutine(node));
        }

        //Wait until queue is done

        yield return new WaitUntil(() => ActionQueue.Instance.IsQueueStopped());

        LogManager.Instance.LogMovementAbility(card, CardSelectionManager.Instance.SelectedCharacter, shape.Count);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }
}
