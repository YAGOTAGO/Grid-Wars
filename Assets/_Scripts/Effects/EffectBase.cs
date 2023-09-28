
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EffectBase : ScriptableObject
{
    public virtual int Duration { get; set; } //not all effects need duration
    public abstract StatusType Type { get;}
    public abstract string Description { get; }
    public abstract Sprite EffectIcon { get; }

    #region Hooks
    public virtual void EndOfTurn(Character character) { }
    public virtual void StartOfTurn() { }
    public virtual bool CanTakeDamage() { return true; }
    public virtual bool CanBeHealed() { return true; }
    public virtual bool CanMove() { return true; } //modifying if can move
    public virtual int OnMove(int moveAmount) { return moveAmount; } //modifying move amount
    public virtual int OnHealDeal(CombatInfo healInfo) { return healInfo.Value; }
    public virtual int OnHealReceive(CombatInfo healInfo) { return healInfo.Value; }
    public virtual int OnDamageDeal(CombatInfo damageInfo) { return damageInfo.Value; }
    public virtual int OnDamageReceive(CombatInfo damageInfo) { return damageInfo.Value; }
    #endregion

    #region Helper Methods/ vars
    public void AddToDuration(int extraDur) { Duration += extraDur; }

    protected string passive = "<color=#FFA500>Passive:</color>";
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
    BUFF = 0, //buff given or applied
    DEBUFF = 1, //negative effects
    INNATE = 2//effects character comes with
}