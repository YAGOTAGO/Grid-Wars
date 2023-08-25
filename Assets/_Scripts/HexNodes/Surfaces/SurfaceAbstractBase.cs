using UnityEngine;

public abstract class SurfaceAbstractBase : ScriptableObject
{
    public abstract bool IsWalkable { get; }
    public abstract bool CanAbilitiesPassthrough { get; }
    public abstract Sprite SurfaceSprite { get; }
    
    public abstract void OnTouchNode(AbstractCharacter character);
}
