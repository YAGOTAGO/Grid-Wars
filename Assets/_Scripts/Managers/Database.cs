using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance;

    [Header("Card Art Sprites")]
    [SerializeField] private List<Sprite> _cardSprites = new();
    
    [Header("Effect Sprites")]
    [SerializeField] private List<Sprite> _effectSprites = new();

    [Header("Shape Sprites")]
    [SerializeField] private List<Sprite> _shapeSprites = new();

    [Header("Surface ScripableObjects")]
    [SerializeField] private List<SurfaceAbstractBase> _surfaceScriptables = new();

    #region Databases
    public Dictionary<string, Sprite> EffectSpritesDB { get; private set; } = new();
    public Dictionary<string, Sprite> CardSpritesDB { get; private set; } = new();
    public Dictionary<string, Sprite> ShapeSpritesDB { get; private set; } = new();
    public Dictionary<string, SurfaceAbstractBase> SurfaceScriptablesDB { get; private set; } = new();
    #endregion

    private void Awake()
    {
        Instance = this;

        LoadAllCardSprites();
        LoadAllShapeSprites();
        LoadAllEffectSprites();
        LoadAllSurfaceScriptables();
    }

    private void LoadAllCardSprites()
    {

        foreach (Sprite s in _cardSprites)
        {
            CardSpritesDB[s.name] = s;
        }
    }

    private void LoadAllShapeSprites()
    {

        foreach (Sprite s in _shapeSprites)
        {
            ShapeSpritesDB[s.name] = s;
        }
    }

    private void LoadAllEffectSprites()
    {

        foreach (Sprite s in _effectSprites)
        {
            EffectSpritesDB[s.name] = s;
        }
    }

    private void LoadAllSurfaceScriptables()
    {

        foreach (SurfaceAbstractBase s in _surfaceScriptables)
        {
            SurfaceScriptablesDB[s.name] = s;
        }
    }
}
