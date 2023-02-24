
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

    public int AtDamageGive(DamageInfo damageInfo) 
    { 
        return dmg.dmg; 
    }

}


public enum StatusType
{
    BUFF,
    DEBUFF
}