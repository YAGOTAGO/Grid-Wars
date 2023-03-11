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
    public override int Duration { get { return _duration; } set { _duration = 2; } }
    public override StatusType Type { get { return _type; } set { _type = StatusType.DEBUFF; } }
    public override string Description 
    { 
        get { return _description; } 
        set { _description = "<b>< color=#FF4E01>BURN: </color></b>" + "At the end of the take <color=red>" + Damage + ".</color> Lasts " + _duration + "turns."; } 
    }
    public override Sprite EffectIcon { get { return _effectIcon; } set { _effectIcon = Resources.Load<Sprite>("burnIcon"); } }

    #endregion
    
    //Constructor
    public BurnEffect()
    {
        _duration = 2;
        _description = "<b><color=#FF4E01>BURN: </color></b>" + "At the end of the take <color=red>" + Damage + " damage.</color> Lasts " + _duration + " turns.";
        _type = StatusType.DEBUFF;
        _effectIcon = Resources.Load<Sprite>("burnIcon");
    }


    public override void EndOfTurn(Character character) 
    {
        DamageManager.Instance.Damage(new DamageInfo(Damage, DamageType.Fire, null, character));
    }
}
