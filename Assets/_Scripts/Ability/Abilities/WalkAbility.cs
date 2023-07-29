using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAbility : AbstractAbility
{
    private int _range;
    public override int Range { get => _range; set => _range = value; }
    public override string Prompt => "Select a node to walk to, right click to add breakpoint.";
    public override void DoAbility(HexNode node)
    {
        ActionQueue.Instance.EnqueueMethod(()=>WalkRoutine(node));
    }

    private IEnumerator WalkRoutine(HexNode target)
    {
        Character character = CardSelectionManager.Instance.ClickedCharacter;

        character.NodeOn.SetSurfaceWalkable(true); //Node character is on becomes walkable
        character.NodeOn.CharacterOnNode = null; //Character on that node is now none

        target.SetSurfaceWalkable(false); //Target node becomes non walkable
        target.CharacterOnNode = character; //Target nodes character becomes the character
        character.NodeOn = target;

        //change this later to after the character moves another coroutine goes that does animation
        target.SurfaceOnEnter(character);

        Tween characterMove = TweenManager.Instance.CharacterMove(character.gameObject, target.transform.position);
        yield return characterMove.WaitForCompletion();
    }

    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        return PathFinding.FindPath(CardSelectionManager.Instance.ClickedCharacter.NodeOn, mouseNode);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }

    public WalkAbility(int range)
    {
        Range = range;
    }
}
