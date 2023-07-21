using UnityEngine;

public class BurnEffect : AbstractEffect
{
    #region Private Vars
    private string _description;
    private int _duration;
    private Sprite _effectIcon;
    private StatusType _type;
    #endregion 

    public int Damage = 3;

    #region Override Abstract Var
    public override int Duration { get => _duration;  set => _duration = value;  }
    public override StatusType Type { get => _type; } 
    public override string Description { get => _description; } 
    public override Sprite EffectIcon { get => _effectIcon; }

    #endregion
    
    //Constructor
    public BurnEffect()
    {
        _duration = 2;
        _description = "<b><color=#FF4E01>BURN: </color></b>" + "At the end of the take <color=red>" + Damage + " damage.</color> Lasts " + _duration + " turns.";
        _type = StatusType.DEBUFF;
        _effectIcon = LoadSprite("burnIcon");
    }

    /// <summary>
    /// Updates the description to match new damage and duration values
    /// </summary>
    public override void UpdateDescription()
    {
        _description = "<b><color=#FF4E01>BURN: </color></b>" + "At the end of the take <color=red>" + Damage + " damage.</color> Lasts " + _duration + " turns.";
    }

    public override void EndOfTurn(Character character) 
    {
        DamageManager.Instance.Damage(new DamageInfo(Damage, DamageType.Fire, null, character));
        _duration--;
        character.UpdateEffectDescrip(this);
        if (_duration <= 0) { character.RemoveEffect(this); }
    }
}
