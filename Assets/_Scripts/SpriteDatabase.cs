using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDatabase : MonoBehaviour
{
    public static SpriteDatabase Instance;
    public Dictionary<string, Sprite> EffectSprites {  get; private set; }
    public Dictionary<string, Sprite> CardSprites { get; private set; }
    public Dictionary<string, Sprite> ShapeSprites { get; private set; }

    private void Awake()
    {
        Instance = this;
        EffectSprites = new();
        CardSprites = new();
        ShapeSprites = new();

        LoadAllCardSprites();
        LoadAllShapeSprites();
        LoadAllEffectSprites();
    }

    private void LoadAllCardSprites()
    {
        Sprite[] cardSprites = Resources.LoadAll<Sprite>("CardArt");

        foreach (Sprite s in cardSprites)
        {
            CardSprites[s.name] = s;
        }
    }

    private void LoadAllShapeSprites()
    {
        Sprite[] shapeSprites = Resources.LoadAll<Sprite>("ShapeArt");

        foreach (Sprite s in shapeSprites)
        {
            ShapeSprites[s.name] = s;
        }
    }

    private void LoadAllEffectSprites()
    {
        Sprite[] effectSprites = Resources.LoadAll<Sprite>("Effects");

        foreach (Sprite s in effectSprites)
        {
            EffectSprites[s.name] = s;
        }
    }
}
