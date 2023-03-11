
public class DamageInfo
{
    public int Val { get; private set; }
    public DamageType Type { get; private set; }

    public Character Source { get; private set; }
    public Character Target { get; private set; }

    public DamageInfo(int dmg, DamageType type, Character source, Character target)
    {
        this.Type = type;
        this.Val = dmg;
        this.Source = source;
        this.Target = target;
    }

}

public enum DamageType
{
    Fire,
    Toxic,
    Normal,

}
