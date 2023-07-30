using UnityEngine;

public class DamageManager : MonoBehaviour
{

    //Could return a struct that contains a bunch of flags such as
    //Whether the character target character died
    public static void Damage(DamageInfo dmgInfo)
    {
        int damage = dmgInfo.Val;
        Character source = dmgInfo.Source;
        Character target = dmgInfo.Target; 

        if(target == null)
        {
            Debug.Log("Target is null in damage manager");
            return;
        }

        if(source != null)
        {
            foreach (AbstractEffect ef in source.Effects)
            {
                damage = ef.AtDamageGive(dmgInfo);
            }
        }

        if(target != null)
        {
            foreach (AbstractEffect ef in target.Effects)
            {
                damage = ef.AtDamageReceive(dmgInfo);
            }
        }
        
        target.TakeDamage(damage);

    }
}
