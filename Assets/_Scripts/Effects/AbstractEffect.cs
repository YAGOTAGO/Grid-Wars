
public abstract class AbstractEffect
{
    /*
     * Making a method abstract mean derived classes MUST implement it
     */

    protected int duration;
    protected StatusType type;
    protected DamageInfo dmg;
        
    public void EndOfTurn() { }
    public void StartOfTurn() { }
    public void OnStepNode() { }
    public void OnLeaveNode() { }

    public int AtDamageGive(DamageInfo damageInfo) 
    { 
        return damageInfo.Val; 
    }

    public int AtDamageReceive(DamageInfo damageInfo)
    {
        return damageInfo.Val;
    }

    public int GetDuration() { return duration; }

    public void AddToDuration(int extraDur) { duration += extraDur; }

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