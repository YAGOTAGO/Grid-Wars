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
        AbstractCharacter source = dmgInfo.Source;
        AbstractCharacter target = dmgInfo.Target; 

        if(target == null)
        {
            return 0;
        }

        if(source != null)
        {
            foreach (EffectBase ef in source.Effects)
            {
                dmgInfo.Val = ef.OnDamageDeal(dmgInfo);
            }
        }

        foreach (EffectBase ef in target.Effects)
        {
             dmgInfo.Val = ef.OnDamageReceive(dmgInfo);
        }

        dmgInfo.Val = dmgInfo.Val <= 0 ? 0 : dmgInfo.Val;

        target.TakeDamage(dmgInfo.Val);
        
        return dmgInfo.Val;
    }
}
