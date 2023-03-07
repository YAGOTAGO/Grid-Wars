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

    public void Damage(DamageInfo dmg)
    {
        int damage = dmg.Val;
        Character source = dmg.Source;
        Character target = dmg.Target; 

        foreach (AbstractEffect ef in source.Effects)
        {
            damage = ef.AtDamageGive(dmg);
        }

        foreach(AbstractEffect ef in target.Effects)
        {
            damage = ef.AtDamageReceive(dmg);
        }

        target.TakeDamage(damage);

    }
}
