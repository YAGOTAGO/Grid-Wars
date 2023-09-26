using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/TrapSurface")]
public class TrapSurface : SurfaceBase
{
    [SerializeField] private List<Sprite> _surfaceSprites;
    [SerializeField] private int _damage;
    [SerializeField] private DamageType _dmgType;
    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;

    public override Sprite SurfaceSprite
    {
        get
        {
            if (_surfaceSprites.Count > 0) { return _surfaceSprites[Random.Range(0, _surfaceSprites.Count)]; }
            return null;
        }
    }
    
    public override void OnEnterNode(AbstractCharacter character)
    {
        DamageInfo dmgInfo = new(_damage, _dmgType, null, character);
        int damage = DamageManager.Damage(dmgInfo);

        HexNode node = character.GetNodeOn();
        node.SetSurface(Database.Instance.GetSurfaceByName("EmptySurface"));
        node.SetSurfaceWalkable(false);

        LogManager.Instance.LogGenericDamage(character, damage, "Trap");
    }

}
