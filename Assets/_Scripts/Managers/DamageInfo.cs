
public class DamageInfo
{
    public int Val;
    public DamageType Type { get; private set; }

    public AbstractCharacter Source { get; private set; }
    public AbstractCharacter Target { get; private set; }

    public DamageInfo(int dmg, DamageType type, AbstractCharacter source, AbstractCharacter target)
    {
        Type = type;
        Val = dmg;
        Source = source;
        Target = target;
    }

}
public enum DamageType
{
    NORMAL,
    FIRE,
    MAGIC,
    WATER
}

