using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/DefaultSurface")]
public class DefaultSurface : SurfaceBase
{
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _canAbilitiesPassthrough;
    [SerializeField] private List<Sprite> _surfaceSprites;
    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value;  }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public override List<Sprite> SurfaceSprites => _surfaceSprites;

    public override void OnEnterNode(AbstractCharacter character)
    {
        //Default surfaces don't have touch node implementation
    }

    
}
