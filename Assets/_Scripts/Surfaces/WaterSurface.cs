using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : ISurface
{
    [SerializeField] private bool _isWalkable = false;

    [SerializeField] private bool _canAbilitiesPassthrough = true;

    [SerializeField] private Sprite _surfaceSprite = null;

    [SerializeField] private int _duration = -1;

    public bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public Sprite SurfaceSprite => _surfaceSprite;
    public int Duration => _duration;
    public bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public void OnTouchNode(Character character) { }
}
