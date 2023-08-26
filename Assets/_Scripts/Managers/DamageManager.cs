using UnityEngine;

public class DamageManager : MonoBehaviour
{

    //Could return a struct that contains a bunch of flags such as
    //Whether the character target character died
    public static AbstractCharacter Damage(DamageInfo dmgInfo)
    {
        int damage = dmgInfo.Val;
        AbstractCharacter source = dmgInfo.Source;
        AbstractCharacter target = dmgInfo.Target; 

        if(target == null)
        {
            Debug.Log("Target is null in damage manager");
            return null;
        }

        if(source != null)
        {
            foreach (AbstractEffect ef in source.Effects)
            {
                damage = ef.AtDamageGive(dmgInfo);
            }
        }


        foreach (AbstractEffect ef in target.Effects)
        {
             damage = ef.AtDamageReceive(dmgInfo);
        }
     
        
        target.TakeDamage(damage);
        
        return target;
    }
}
