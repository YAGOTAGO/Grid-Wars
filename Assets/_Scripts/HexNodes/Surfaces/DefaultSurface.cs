using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/DefaultSurface")]
public class DefaultSurface : SurfaceAbstractBase
{
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _canAbilitiesPassthrough;
    [SerializeField] private List<Sprite> _surfaceSprites;
    private Sprite _sprite;
    public override bool IsWalkable => _isWalkable;
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    
    public override Sprite SurfaceSprite 
    { 
        get
        {
            if (_sprite == null) { ChooseRandomSprite(); }
            return _sprite;
        }
    }

    public override void OnTouchNode(AbstractCharacter character)
    {
        //Default surfaces don't have touch node implementation
    }

    private void ChooseRandomSprite()
    {
        if (_surfaceSprites.Count > 0)
        {
            _sprite = _surfaceSprites[Random.Range(0, _surfaceSprites.Count)];
        }
        else
        {
            _sprite= null;
        }
    }
}
