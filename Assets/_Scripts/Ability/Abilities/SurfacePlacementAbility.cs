using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/SurfacePlacementAbility")]
public class SurfacePlacementAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;
    [SerializeField] private SurfaceBase _surface;

    public override Shape ShapeEnum => global::Shape.MOUSEHEX;
    public override int Range => _range;
    public override string Prompt => _prompt;
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        foreach (HexNode node in shape)
        {
            node.SetSurface(_surface);
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }
}
