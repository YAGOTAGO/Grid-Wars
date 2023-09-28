using UnityEngine;

public static class CombatManager
{

    /// <summary>
    /// Potentially damages, returning true if did
    /// </summary>
    /// <param name="dmgInfo"></param>
    /// <returns></returns>
    public static int Damage(CombatInfo dmgInfo)
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
                dmgInfo.Value = ef.OnDamageDeal(dmgInfo);
            }
        }

        foreach (EffectBase ef in target.Effects)
        {
            if (!ef.CanTakeDamage()) { dmgInfo.Value = 0; break; }
             dmgInfo.Value = ef.OnDamageReceive(dmgInfo);
        }

        dmgInfo.Value = dmgInfo.Value <= 0 ? 0 : dmgInfo.Value;

        target.TakeDamage(dmgInfo.Value);
        
        return dmgInfo.Value;
    }

    public static int Heal(CombatInfo healInfo)
    {
        AbstractCharacter source = healInfo.Source;
        AbstractCharacter target = healInfo.Target;

        if (target == null)
        {
            return 0;
        }

        if (source != null)
        {
            foreach (EffectBase ef in source.Effects)
            {
                healInfo.Value = ef.OnHealDeal(healInfo);
            }
        }

        foreach (EffectBase ef in target.Effects)
        {
            if (!ef.CanBeHealed()) { healInfo.Value = 0;  break; } //if character has heal negating effect we don't heal
            healInfo.Value = ef.OnHealReceive(healInfo);
        }

        healInfo.Value = healInfo.Value <= 0 ? 0 : healInfo.Value; //make sure not negative
        int maxHeal = target.MaxHealth - target.Health.Value; //find what max healing can be
        healInfo.Value = Mathf.Min(maxHeal, healInfo.Value); //make sure cant heal more than max health

        target.Heal(healInfo.Value);

        return healInfo.Value;
    }
}
