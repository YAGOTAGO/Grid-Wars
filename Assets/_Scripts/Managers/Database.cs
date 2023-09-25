using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance;

    [Header("Effect ScripableObjects")]
    [SerializeField] private List<EffectBase> _effectScriptables = new();

    [Header("Surface ScripableObjects")]
    [SerializeField] private List<SurfaceBase> _surfaceScriptables = new();

    [Header("Card ScriptableObjects")]
    [SerializeField] private List<CardBase> _cardScriptables = new();

    #region Databases
    public NumberedDictionary<AbstractCharacter> CharactersDB { get; private set; } = new();
    public List<int> AllyPlayers = new();
    public List<int> Allies = new();
    public List<int> Enemies = new();
    private readonly Dictionary<string, SurfaceBase> _surfacesDB = new();
    private readonly Dictionary<string, CardBase> _cardsDB = new();
    private readonly Dictionary<string, EffectBase> _effectsDB = new();

    //Sorted by rarity
    private readonly List<CardBase> _basicCardsDB = new();
    private readonly List<CardBase> _commonCardsDB = new();
    private readonly List<CardBase> _rareCardsDB = new();
    #endregion

    private void Awake()
    {
        Instance = this;

        LoadAllEffectSprites();
        LoadAllSurfaceScriptables();
        LoadAllCardScriptables();
    }

    public bool IsAlly(AbstractCharacter character)
    {
        return Allies.Contains(character.CharacterID.Value);
    }

    /// <param name="name">Name of surface</param>
    /// <returns>A instance of a surface scriptable object</returns>
    public SurfaceBase GetSurface(string name)
    {
        if (_surfacesDB.TryGetValue(name, out SurfaceBase surface))
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
        if (_cardsDB.TryGetValue(name, out CardBase card))
        {
            return Instantiate(card); //Use instantiate so each surface is unique
        }
        else
        {
            Debug.LogWarning("Did not find + " + name + "in cards DB");
            return null;
        }
    }

    /// <param name="name">Name of effect</param>
    /// <returns>A instance of a card scriptable object</returns>
    public EffectBase GetEffectByName(string name)
    {
        if (_effectsDB.TryGetValue(name, out EffectBase effect))
        {
            return Instantiate(effect); //Use instantiate so each surface is unique
        }
        else
        {
            Debug.LogWarning("Did not find + " + name + "in cards DB");
            return null;
        }
    }

    #region GetRandomCards
    public CardBase GetRandomCommonCard()
    {
        if(_commonCardsDB.Count == 0) { Debug.LogWarning("No cards in common card database"); return null; }
        return Instantiate(_commonCardsDB[Random.Range(0, _commonCardsDB.Count)]);
    }

    public List<CardBase> GetDifferentCommons(int amount)
    {
        if (_commonCardsDB.Count == 0) { Debug.LogWarning("No cards in common card database"); return null; }
        if(amount > _commonCardsDB.Count) { Debug.LogWarning("Not enought common cards to pull from"); return null; }

        List<CardBase> selectedCards = new();
        List<int> selectedIndices = new();

        while(selectedCards.Count < amount) 
        {
            int randomIndex = Random.Range(0, _commonCardsDB.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                selectedCards.Add(Instantiate(_commonCardsDB[randomIndex]));
            }
        }
        
        return selectedCards;
    }

    public CardBase GetRandomRareCard()
    {
        if (_rareCardsDB.Count == 0) { Debug.LogWarning("No cards in rare card database"); return null; }
        return Instantiate(_rareCardsDB[Random.Range(0, _rareCardsDB.Count)]);
    }

    public List<CardBase> GetDifferentRares(int amount)
    {
        if (_rareCardsDB.Count == 0) { Debug.LogWarning("No cards in rare card database"); return null; }
        if (amount > _rareCardsDB.Count) { Debug.LogWarning("Not enought rare cards to pull from"); return null; }

        List<CardBase> selectedCards = new();
        List<int> selectedIndices = new();

        while (selectedCards.Count < amount)
        {
            int randomIndex = Random.Range(0, _rareCardsDB.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                selectedCards.Add(Instantiate(_rareCardsDB[randomIndex]));
            }
        }

        return selectedCards;
    }
    
    #endregion

    #region Load Database
    private void LoadAllEffectSprites()
    {

        foreach (EffectBase e in _effectScriptables)
        {
            _effectsDB[e.name] = e;
        }
    }

    private void LoadAllSurfaceScriptables()
    {

        foreach (SurfaceBase s in _surfaceScriptables)
        {
            _surfacesDB[s.name] = s;

        }
    }

    private void LoadAllCardScriptables()
    {
        foreach (CardBase c in _cardScriptables)
        {
            _cardsDB[c.name] = c;

            switch (c.Rarity)
            {
                case Rarity.BASIC: _basicCardsDB.Add(c); break;
                case Rarity.COMMON: _commonCardsDB.Add(c);break;
                case Rarity.RARE: _rareCardsDB.Add(c); break;
            }
        }
    }
    #endregion
    

}
