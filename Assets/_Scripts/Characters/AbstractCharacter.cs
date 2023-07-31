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

    #endregion

}
