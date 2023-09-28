using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/DamageHealSurface")]
public class DamageOrHealSurface : SurfaceBase
{   
    [SerializeField] private List<Sprite> _surfaceSprites;
    [SerializeField] private bool _isHeal;
    [SerializeField] private int _amount;
    [SerializeField] private DamageType _dmgType;
    [SerializeField] private string _logName;

    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public override List<Sprite> SurfaceSprites => _surfaceSprites;

    public override void OnEnterNode(AbstractCharacter character)
    {
        if(_isHeal)
        {
            CombatInfo dmgInfo = new(_amount, DamageType.HEAL, null, character);
            int damage = CombatManager.Heal(dmgInfo);

            HexNode node = character.GetNodeOn();
            node.SetSurface(Database.Instance.GetSurfaceByName("EmptySurface"));
            node.SetSurfaceWalkable(false);

            LogManager.Instance.LogGenericHeal(character, damage, _logName);

        }
        else
        {
            CombatInfo dmgInfo = new(_amount, _dmgType, null, character);
            int damage = CombatManager.Damage(dmgInfo);

            HexNode node = character.GetNodeOn();
            node.SetSurface(Database.Instance.GetSurfaceByName("EmptySurface"));
            node.SetSurfaceWalkable(false);

            LogManager.Instance.LogGenericDamage(character, damage, _logName);
        }
        
    }

}
