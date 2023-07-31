using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public abstract HashSet<AbstractEffect> Effects { get; }
    public abstract HexNode NodeOn { get; set; }
    public abstract int Health { get; set; }

    #region Abstract Methods
    public abstract void AddEffect(AbstractEffect ef);
    public abstract void RemoveEffect(AbstractEffect ef);
    public abstract void TakeDamage(int damage);

    /// <summary>
    /// Puts the character on given hexnode
    /// </summary>
    /// <param name="target">HexNode we are placing character on</param>
    /// <param name="positionCharacterOnNode">Whether to change the characters position</param>
    public virtual void PutOnHexNode(HexNode target, bool positionCharacterOnNode)
    {
        if(NodeOn != null)
        {
            //Set node we are leaving to be free
            NodeOn.SetSurfaceWalkable(true);
            NodeOn.CharacterOnNode = null;
        }

        target.SetSurfaceWalkable(false);
        target.CharacterOnNode = this;
        NodeOn = target;
        target.SurfaceOnEnter(this);

        if (positionCharacterOnNode)
        {
           gameObject.transform.position = target.transform.position;
        }
    }
    #endregion

}
