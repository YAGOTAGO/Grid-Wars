using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/TrapPlacementAbility")]
public class TrapPlacementAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;
    [SerializeField] private SurfaceBase _surface;

    private AbstractShape _abstractShape;
    public override int Range { get => _range; }
    public override string Prompt => _prompt;
    public override AbstractShape Shape
    {
        get
        {
            _abstractShape = EnumToShape(global::Shape.MOUSEHEX); //If abstract shape is null we set it
            return _abstractShape;
        }
        set => _abstractShape = value;
    }
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
