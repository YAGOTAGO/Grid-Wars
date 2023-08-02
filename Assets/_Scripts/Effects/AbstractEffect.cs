
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbstractEffect
{
    /*
     * Abstract means derived classes MUST implement it
     * Virtual means has default but can be overriden
     */
    #region Stats
    public abstract int Duration { get; set; }
    public abstract StatusType Type { get;}
    #endregion

    #region UI Display
    public abstract string Description { get; }
    public abstract Sprite EffectIcon { get; }
    #endregion

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

    //May need to adjust this when adding npcs
    public virtual IEnumerator FlashEffect(Character character)
    {
        GameObject effectObj = character.GetEffectGameObject(this);
        if(effectObj == null) { yield break; }

        for(int i =0; i<2; i++)
        {
            Tween scaleUp = effectObj.transform.DOScale(new Vector3(2, 2), .5f);
            yield return scaleUp.WaitForCompletion();

            Tween scaleDown = effectObj.transform.DOScale(new Vector3(1, 1), .5f);
            yield return scaleDown.WaitForCompletion();
        }
        
    }
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