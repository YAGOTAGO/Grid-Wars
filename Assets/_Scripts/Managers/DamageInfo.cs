
public class DamageInfo
{
    public int Val { get; private set; }
    public DamageType Type { get; private set; }

    public AbstractCharacter Source { get; private set; }
    public AbstractCharacter Target { get; private set; }

    public DamageInfo(int dmg, DamageType type, AbstractCharacter source, AbstractCharacter target)
    {
        this.Type = type;
        this.Val = dmg;
        this.Source = source;
        this.Target = target;
    }

}
public enum DamageType
{
    Normal,
    Fire,
    Magic
}

