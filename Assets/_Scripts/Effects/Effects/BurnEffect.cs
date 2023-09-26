using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effects/Debuff/BurnEffect")]
public class BurnEffect : EffectBase
{
    [SerializeField] private int _duration;
    [SerializeField] private int _damage;
    [SerializeField] private Sprite _effectIcon;

    #region Override Abstract Var
    public override int Duration { get => _duration;  set => _duration = value;  }
    public override StatusType Type => StatusType.DEBUFF; 
    public override string Description => $"<b><color=#FF4E01>BURN: </color></b> At the end of the take <color=red> {_damage} damage.</color> Lasts {_duration} turns.";
    public override Sprite EffectIcon => _effectIcon;
    #endregion

    public override void EndOfTurn(Character character) 
    {
        CombatManager.Damage(new CombatInfo(_damage, DamageType.FIRE, null, character));
        _duration--;
        character.UpdateEffectDescription(this);

        if (_duration <= 0) 
        { 
            character.FlashEffect(this, true); 
        } 
        else
        {
            character.FlashEffect(this, false);
        }

    }
}
