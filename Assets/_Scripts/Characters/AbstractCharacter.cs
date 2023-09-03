using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class AbstractCharacter : NetworkBehaviour
{
    public abstract HashSet<AbstractEffect> Effects { get; }
    public abstract HexNode NodeOn { get; set; }
    public abstract int Health { get; set; }
    public abstract int CharacterID { get; }

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
            NodeOn.SetCharacterOnNode(-1); //-1 will set a null character reference
        }

        target.SetSurfaceWalkable(false);
        target.SetCharacterOnNode(CharacterID);
        NodeOn = target;
        target.OnEnterSurface(this);

        if (positionCharacterOnNode)
        {
           transform.position = target.transform.position;
        }

    }
    #endregion

}
