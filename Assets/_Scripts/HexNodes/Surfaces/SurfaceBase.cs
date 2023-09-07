using UnityEngine;

public abstract class SurfaceBase : ScriptableObject
{
    public abstract bool IsWalkable { get; set; }
    public abstract bool CanAbilitiesPassthrough { get; }
    public abstract Sprite SurfaceSprite { get; }
    
    public abstract void OnEnterNode(AbstractCharacter character);
}
