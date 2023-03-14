using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : AbstractCharacter
{
    #region Visuals
    public abstract GameObject Highlight { get; }
    public abstract GameObject  AbilityCanvas{ get; }
    public abstract GameObject  EffectsUIGroup { get; }
    public abstract GameObject StatsUI { get; }
    public abstract HealthBar HealthBar { get; }
    #endregion


}
