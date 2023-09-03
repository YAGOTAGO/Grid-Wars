using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class AbstractCharacter : NetworkBehaviour
{
    public abstract HashSet<AbstractEffect> Effects { get; }
    public abstract int Health { get; }
    public abstract int CharacterID { get; }

    public NetworkVariable<Vector3Int> HexGridPosition = new(new Vector3Int(-1,-1,-1));

    private HexNode NodeOn;
    public HexNode GetNodeOn() { return NodeOn; }

    public void SetNodeOn(HexNode node)
    {
        Debug.Log("Set node on called");
        HexGridPosition.Value = node.GridPos.Value;
    }

    public void UpdateNodeOn(Vector3Int preVal, Vector3Int newVal)
    {
        Debug.Log("Update Node On called");

        NodeOn = GridManager.Instance.GridCoordTiles[newVal];
    }

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
        SetNodeOn(target);
        target.OnEnterSurface(this);

        if (positionCharacterOnNode)
        {
           transform.position = target.transform.position;
        }

    }
    #endregion

}
