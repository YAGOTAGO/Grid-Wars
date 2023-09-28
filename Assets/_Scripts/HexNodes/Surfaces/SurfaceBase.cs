using System.Collections.Generic;
using UnityEngine;

public abstract class SurfaceBase : ScriptableObject
{   
    public abstract bool IsWalkable { get; set; }
    public abstract bool CanAbilitiesPassthrough { get; }
    public virtual List<Sprite> SurfaceSprites { get; }
    public virtual Sprite SurfaceSprite
    {
        get
        {
            if (SurfaceSprites.Count > 0) { return SurfaceSprites[Random.Range(0, SurfaceSprites.Count)]; }
            return null;
        }
    }

    public HexNode NodeOn;
    public abstract void OnEnterNode(AbstractCharacter character);
}
