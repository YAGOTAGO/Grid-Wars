using System.Collections.Generic;

public class CircleShape : AbstractShape
{
    private bool _includeOrigin;
    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        if (_includeOrigin)
        {
            List<HexNode> circle = new(mouseNode.Neighboors) { mouseNode };
            return circle;
        }
        else
        {
            return mouseNode.Neighboors;
        }
    }

    public CircleShape(bool includeOrigin)
    {
       _includeOrigin = includeOrigin;
    }
}
