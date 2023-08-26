using DG.Tweening;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/WalkAbility")]
public class WalkAbility : AbilityBase
{
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;
    
    private AbstractShape _abstractShape;

    public override string Prompt => _prompt;
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
        AbstractCharacter character = CardSelectionManager.Instance.ClickedCharacter;

        character.PutOnHexNode(target, false); //logic for setting surface variables

        Tween characterMove = TweenManager.Instance.CharacterMove(character.gameObject, target.transform.position);
        yield return characterMove.WaitForCompletion();
    }

    public override void DoAbility(HexNode node)
    {
        ActionQueue.Instance.EnqueueMethod(() => WalkRoutine(node));
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }
}
