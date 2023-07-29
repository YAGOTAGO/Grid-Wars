using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISurface
{
    public bool IsWalkable { get; set; } // Determines character move abilities
    public bool CanAbilitiesPassthrough { get; } // Whether abilities can go over it
    public Sprite SurfaceSprite { get; }
    public int Duration { get; }
    public void OnTouchNode(Character character);
}

