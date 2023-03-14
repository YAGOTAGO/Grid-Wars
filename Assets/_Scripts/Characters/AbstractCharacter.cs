using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public abstract HashSet<AbstractEffect> Effects { get; }
    public abstract List<AbstractAbility> Abilities { get; }
    public abstract HexNode OnNode { get; set; }

    #region Move Stats
    public abstract float MoveSpeed { get; }
    public abstract iTween.EaseType EaseType { get; }
    public abstract int WalkMoves { get; set; }
    #endregion

    #region Stats
    public abstract int Health { get; }
    public abstract int Actions { get; set; }    
    #endregion

    #region Abstract Methods
    public abstract void AddEffect(AbstractEffect ef);
    public abstract void RemoveEffect(AbstractEffect ef);
    public abstract void TakeDamage(int damage);
    public abstract void OnSelect();
    public abstract void OnDeselect();
    #endregion

    #region Concrete Methods
    //OnNode
    public HexNode GetNodeOn() { return OnNode; }
    public void SetNodeOn(HexNode node) { OnNode = node; }

    //Actions
    public int GetNumActions() { return Actions; }
    public void DecrementActions() { Actions--; }
    
    //Walk
    public int GetWalkMoves() { return WalkMoves; }
    public void DecrementWalkMoves() { WalkMoves--; }
    #endregion

}
