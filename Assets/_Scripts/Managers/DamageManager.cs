using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Damage(DamageInfo dmgInfo)
    {
        int damage = dmgInfo.Val;
        Character source = dmgInfo.Source;
        Character target = dmgInfo.Target; 

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
