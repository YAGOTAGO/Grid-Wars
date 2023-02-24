
public class Damage
{
    public int dmg { get; private set; }
    public DamageType type { get; private set; }


    public Damage(int dmg, DamageType type)
    {
        this.type = type;
        this.dmg = dmg;
    }


   
}

public enum DamageType
{
    Fire,
    Toxic,
    Normal,

}
