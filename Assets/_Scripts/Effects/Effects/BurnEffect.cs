using UnityEngine;

public class BurnEffect : AbstractEffect
{
    #region Private Vars
    private int _duration = 2;
    #endregion 

    private readonly int _damage = 3;

    #region Override Abstract Var
    public override int Duration { get => _duration;  set => _duration = value;  }
    public override StatusType Type => StatusType.DEBUFF; 
    public override string Description => "<b><color=#FF4E01>BURN: </color></b>" + "At the end of the take <color=red>" +  _damage + " damage.</color> Lasts " + _duration + " turns.";
    public override Sprite EffectIcon => SpriteDatabase.Instance.EffectSpritesDB["burnIcon"];

    #endregion

    public override void EndOfTurn(Character character) 
    {
        DamageManager.Damage(new DamageInfo(_damage, DamageType.Fire, null, character));
        _duration--;
        character.UpdateEffectDescrip(this);

        if (_duration <= 0) 
        { 
            character.RemoveEffect(this); 
        } 
        else
        {
            character.FlashEffect(this);
        }

    }
}
