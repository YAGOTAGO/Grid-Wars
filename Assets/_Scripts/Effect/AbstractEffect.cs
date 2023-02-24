
public abstract class AbstractEffect
{
    /*
     * Making a method abstract mean derived classes MUST implement it
     */

    protected int duration;
    protected StatusType type;
    protected Damage dmg;

    public void EndOfTurn() { }
    public void StartOfTurn() { }


}


public enum StatusType
{
    BUFF,
    DEBUFF
}