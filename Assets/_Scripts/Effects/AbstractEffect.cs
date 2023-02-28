
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



}


public enum StatusType
{
    BUFF,
    DEBUFF
}