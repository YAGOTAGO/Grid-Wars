using System.Collections.Generic;

public class CircleShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability)
    {
        List<HexNode> circle = new(mouseNode.Neighboors) { mouseNode };
        return circle;
  
    }

}
