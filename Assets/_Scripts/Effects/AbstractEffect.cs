
using UnityEngine;

public abstract class AbstractEffect
{
    /*
     * Making a method abstract mean derived classes MUST implement it
     * Virtual means has deafult but can be overriden
     */
    #region Stats
    public abstract int Duration { get; set; }
    public abstract StatusType Type { get; set; }
    #endregion

    #region UI Display
    public abstract string Description { get; set; }
    public abstract Sprite EffectIcon { get; set; }
    #endregion

    //Hooks
    public virtual void EndOfTurn(Character character) { }
    public virtual void StartOfTurn() { }
    public virtual void OnStepNode() { }
    public virtual void OnLeaveNode() { }
    public virtual int AtDamageGive(DamageInfo damageInfo) { return damageInfo.Val; }
    public virtual int AtDamageReceive(DamageInfo damageInfo) { return damageInfo.Val; }

    //Helper methods
    public void AddToDuration(int extraDur) { Duration += extraDur; }

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