using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainSurface : MonoBehaviour, ISurface
{
    [SerializeField] private bool _isWalkable = false;

    [SerializeField] private bool _canAbilitiesPassthrough = false;

    [SerializeField] private Sprite _surfaceSprite = null;

    [SerializeField] private int _duration = -1;

    public bool IsWalkable => _isWalkable;
    public bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public Sprite SurfaceSprite => _surfaceSprite;
    public int Duration => _duration;

    public void OnTouchNode() {}
}
