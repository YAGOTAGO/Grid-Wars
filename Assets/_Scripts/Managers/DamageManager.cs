using UnityEngine;

public class DamageManager : MonoBehaviour
{

    /// <summary>
    /// Potentially damages, returning true if did
    /// </summary>
    /// <param name="dmgInfo"></param>
    /// <returns></returns>
    public static int Damage(DamageInfo dmgInfo)
    {
        int damage = dmgInfo.Val;
        AbstractCharacter source = dmgInfo.Source;
        AbstractCharacter target = dmgInfo.Target; 

        if(target == null)
        {
            Debug.Log("Target is null in damage manager");
            return 0;
        }

        if(source != null)
        {
            foreach (EffectBase ef in source.Effects)
            {
                damage = ef.AtDamageGive(dmgInfo);
            }
        }


        foreach (EffectBase ef in target.Effects)
        {
             damage = ef.AtDamageReceive(dmgInfo);
        }
     
        damage = damage <= 0 ? 0 : damage;

        target.TakeDamage(damage);
        
        return damage;
    }
}
