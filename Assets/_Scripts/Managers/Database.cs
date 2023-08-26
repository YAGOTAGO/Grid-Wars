using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance;
    
    [Header("Effect Sprites")]
    [SerializeField] private List<Sprite> _effectSprites = new();

    [Header("Surface ScripableObjects")]
    [SerializeField] private List<SurfaceBase> _surfaceScriptables = new();

    [Header("Card ScriptableObjects")]
    [SerializeField] private List<CardBase> _cardScriptables = new();

    #region Databases
    public Dictionary<string, Sprite> EffectSpritesDB { get; private set; } = new();
    private readonly Dictionary<string, SurfaceBase> _surfaceScriptablesDB = new();
    private readonly Dictionary<string, CardBase> _cardScriptablesDB = new();
    #endregion

    private void Awake()
    {
        Instance = this;

        LoadAllEffectSprites();
        LoadAllSurfaceScriptables();
        LoadAllCardScriptables();
    }

    /// <param name="name">Name of surface</param>
    /// <returns>A instance of a surface scriptable object</returns>
    public SurfaceBase GetSurface(string name)
    {
        if (_surfaceScriptablesDB.TryGetValue(name, out SurfaceBase surface))
        {
            return Instantiate(surface); //Use instantiate so each surface is unique
        }
        else
        {
            Debug.LogWarning("Did not find + " + name + "in Surfaces DB");
            return null;
        }
    }

    /// <param name="name">Name of card</param>
    /// <returns>A instance of a card scriptable object</returns>
    public CardBase GetCardByName(string name)
    {
        if (_cardScriptablesDB.TryGetValue(name, out CardBase card))
        {
            return Instantiate(card); //Use instantiate so each surface is unique
        }
        else
        {
            Debug.LogWarning("Did not find + " + name + "in cards DB");
            return null;
        }
    }

    #region Load Database
    private void LoadAllEffectSprites()
    {

        foreach (Sprite s in _effectSprites)
        {
            EffectSpritesDB[s.name] = s;
        }
    }

    private void LoadAllSurfaceScriptables()
    {

        foreach (SurfaceBase s in _surfaceScriptables)
        {
            _surfaceScriptablesDB[s.name] = s;
        }
    }

    private void LoadAllCardScriptables()
    {
        foreach (CardBase c in _cardScriptables)
        {
            _cardScriptablesDB[c.name] = c;
        }
    }
    #endregion
    

}
