using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/DefaultSurface")]
public class DefaultSurface : SurfaceAbstractBase
{
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _canAbilitiesPassthrough;
    [SerializeField] private List<Sprite> _surfaceSprites;
    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value;  }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    
    public override Sprite SurfaceSprite 
    {
        get
        {
            if (_surfaceSprites.Count > 0) { return _surfaceSprites[Random.Range(0, _surfaceSprites.Count)]; }
            return null;
        }
    }

    public override void OnTouchNode(AbstractCharacter character)
    {
        //Default surfaces don't have touch node implementation
    }

    
}
