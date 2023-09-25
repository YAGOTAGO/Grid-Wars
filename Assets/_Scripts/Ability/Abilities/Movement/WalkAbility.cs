using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Movement/WalkAbility")]
public class WalkAbility : AbilityBase
{
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override string Prompt => $"Move up to {Range} hexes.";
    public override int Range => GetRange();
    public override bool IsSpecialPathfind => true;

    //Takes into consideration any movement effects
    private int GetRange()
    {
        AbstractCharacter character = CardSelectionManager.Instance.SelectedCharacter;
        int range = _range;
        foreach(EffectBase ef in character.Effects)
        {
            if (!ef.CanMove()) { return 0; } //any effect makes us unable to move then return 0 range
            range = ef.OnMove(range);
        }
        return range;
    }

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
