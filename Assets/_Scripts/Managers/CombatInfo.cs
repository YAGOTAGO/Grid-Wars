
public class CombatInfo
{
    public int Value;
    public DamageType Type { get; private set; }
    public AbstractCharacter Source { get; private set; }
    public AbstractCharacter Target { get; private set; }

    public CombatInfo(int dmg, DamageType type, AbstractCharacter source, AbstractCharacter target)
    {
        Type = type;
        Value = dmg;
        Source = source;
        Target = target;
    }

}
public enum DamageType
{
    NORMAL = 0,
    FIRE = 1,
    MAGIC = 2,
    WATER = 3,
    HEAL = 4
}

