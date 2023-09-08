
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EffectBase : ScriptableObject
{

    public abstract int Duration { get; set; }
    public abstract StatusType Type { get;}
    public abstract string Description { get; }
    public abstract Sprite EffectIcon { get; }


    #region Hooks
    public virtual void EndOfTurn(Character character) { }
    public virtual void StartOfTurn() { }
    public virtual void OnStepNode() { }
    public virtual void OnLeaveNode() { }
    public virtual int AtDamageGive(DamageInfo damageInfo) { return damageInfo.Val; }
    public virtual int AtDamageReceive(DamageInfo damageInfo) { return damageInfo.Val; }
    #endregion

    #region Helper Methods
    public void AddToDuration(int extraDur) { Duration += extraDur; }

    
    #endregion

    /// <summary>
    /// Equal if are same derived class type
    /// </summary>
    /// <param name="obj">The Abstract Effect we are comparing</param>
    /// <returns>Whether two Abstract effects are of the same derived class</returns>
    public override bool Equals(object obj)
    {
        return base.GetType() == obj.GetType();
            
    }

    /// <summary>
    /// HashCode for C# HashSet
    /// </summary>
    /// <returns>The HashCode based on the type</returns>
    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }
}


public enum StatusType
{
    BUFF,
    DEBUFF
}