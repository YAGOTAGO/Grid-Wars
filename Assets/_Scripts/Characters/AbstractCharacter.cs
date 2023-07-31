using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public abstract HashSet<AbstractEffect> Effects { get; }
    public abstract List<AbstractAbility> Abilities { get; }
    public abstract HexNode OnNode { get; set; }
    public abstract int Health { get; }



    #region Abstract Methods
    public abstract void AddEffect(AbstractEffect ef);
    public abstract void RemoveEffect(AbstractEffect ef);
    public abstract void TakeDamage(int damage);
    public abstract void OnSelect();
    public abstract void OnDeselect();
    #endregion

}
