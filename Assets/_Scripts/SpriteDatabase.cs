using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDatabase : MonoBehaviour
{
    public static SpriteDatabase Instance;

    [SerializeField] private List<Sprite> _cardSprites = new();
    [SerializeField] private List<Sprite> _effectSprites = new();
    [SerializeField] private List<Sprite> _shapeSprites = new();

    public Dictionary<string, Sprite> EffectSpritesDB { get; private set; } = new();
    public Dictionary<string, Sprite> CardSpritesDB { get; private set; } = new();
    public Dictionary<string, Sprite> ShapeSpritesDB { get; private set; } = new();

    private void Awake()
    {
        Instance = this;

        LoadAllCardSprites();
        LoadAllShapeSprites();
        LoadAllEffectSprites();
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
}
