using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Database : PersistentSingleton<Database>
{
    [Header("Effect ScripableObjects")]
    [SerializeField] private List<EffectBase> _effectScriptables = new();

    [Header("Surface ScripableObjects")]
    [SerializeField] private List<SurfaceBase> _surfaceScriptables = new();

    [Header("Card ScriptableObjects")]
    [SerializeField] private List<CardBase> _cardScriptables = new();

    [Header("Characters")]
    [SerializeField] private List<Character> _characters = new();

    #region Databases
    public NumberedDictionary<AbstractCharacter> AbstractCharactersDB { get; private set; } = new();
    [HideInInspector] public List<Character> AllyCharacters = new();
    [HideInInspector] public List<Character> EnemyCharacters = new();
    [HideInInspector] public List<AbstractCharacter> Allies = new();
    [HideInInspector] public List<AbstractCharacter> Enemies = new();
    private readonly Dictionary<string, SurfaceBase> _surfacesDB = new();
    private readonly Dictionary<string, CardBase> _cardsDB = new();
    private readonly Dictionary<string, EffectBase> _effectsDB = new();
    private readonly Dictionary<string, Character> _charactersDB = new();

    //Sorted by rarity
    [Header("Card Distributions")]
    [Range(0, 100)]
    [SerializeField] private int _commonCardChance;
    private readonly List<CardBase> _wizardCardsDB = new();
    private readonly List<CardBase> _monkCardsDB = new();
    private readonly List<CardBase> _clericCardsDB = new();
    private Dictionary<Class, Dictionary<Rarity, List<CardBase>>> _cardsByClassAndRarityDB = new();
    #endregion

    protected override void Awake()
    {
        base.Awake();
        LoadAllSurfaceScriptables();
        LoadAllEffectSprites();
        LoadAllCardScriptables();
        LoadAllCharacters();
        LoadClassAndRarityDB();
    }

    public bool IsAlly(AbstractCharacter character)
    {
        return Allies.Contains(character);
    }

    /// <param name="name">Name of surface</param>
    /// <returns>A instance of a surface scriptable object</returns>
    public SurfaceBase GetSurfaceByName(string name)
    {
        if (_surfacesDB.TryGetValue(name, out SurfaceBase surface))
        {
            return Instantiate(surface); //Use instantiate so each surface is unique
        }
        else
        {
            Debug.LogWarning($"Did not find {name} in Surfaces DB");
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
            Debug.LogWarning($"Did not find {name} in cards DB");
            return null;
        }
    }

    /// <param name="name">Name of effect</param>
    /// <returns>A instance of a card scriptable object</returns>
    public EffectBase GetEffectByName(string name)
    {
        if (_effectsDB.TryGetValue(name, out EffectBase effect))
        {
            return Instantiate(effect); //Use instantiate so each effect is unique
        }
        else
        {
            Debug.LogWarning($"Did not find {name} in effect DB");
            return null;
        }
    }

    public Character GetCharacterByName(string name)
    {
        if (_charactersDB.TryGetValue(name, out Character character))
        {
            return character;
        }
        else
        {
            Debug.LogWarning($"Did not find {name} in character DB");
            return null;
        }
    }

    #region GetRandomCards
    public List<CardBase> GetDifferentClassCards(int amount, Class classType)
    {
        List<CardBase> cardsDB;
        
        switch (classType) 
        {
            case Class.Cleric: cardsDB = _clericCardsDB; break;
            case Class.Monk: cardsDB = _monkCardsDB; break;
            case Class.Wizard: cardsDB = _wizardCardsDB; break;
            default: cardsDB = new(); Debug.LogWarning("GetDifferentClassCards in database didn't find class."); return null;    
        }

        //Warnings if things go wrong
        if (cardsDB.Count == 0) { Debug.LogWarning($"No cards in {classType} card database"); return null; }
        if(amount > cardsDB.Count) { Debug.LogWarning($"Not enough {classType} cards to pull from"); return null; }

        List<CardBase> selectedCards = new(); //contains instatiated version
        List<CardBase> alreadyPicked = new(); //contains direct references

        while (selectedCards.Count < amount)
        {
            // Determine rarity based on probabilities
            float rarityRoll = UnityEngine.Random.Range(0f, 100f);
            Rarity rarity = rarityRoll < _commonCardChance ? Rarity.COMMON : Rarity.RARE;

            // Filter cards by the determined rarity
            List<CardBase> availableCards = _cardsByClassAndRarityDB[classType][rarity].Where(card=> !alreadyPicked.Contains(card)).ToList();

            if (availableCards.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableCards.Count);
                selectedCards.Add(Instantiate(availableCards[randomIndex]));
                alreadyPicked.Add(availableCards[randomIndex]);
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

    private void LoadAllCharacters()
    {

        foreach (Character s in _characters)
        {
            _charactersDB[s.name] = s;

        }
    }

    private void LoadAllCardScriptables()
    {
        foreach (CardBase c in _cardScriptables)
        {
            _cardsDB[c.name] = c;

            switch (c.Class)
            {
                case Class.Wizard: _wizardCardsDB.Add(c); break;
                case Class.Monk: _monkCardsDB.Add(c);break;
                case Class.Cleric: _clericCardsDB.Add(c); break;
            }
        }
    }

    private void LoadClassAndRarityDB()
    {
        List<CardBase> cardsDB; //which class cards

        foreach (Class classType in Enum.GetValues(typeof(Class)))
        {
            switch (classType)
            {
                case Class.Cleric: cardsDB = _clericCardsDB; break;
                case Class.Monk: cardsDB = _monkCardsDB; break;
                case Class.Wizard: cardsDB = _wizardCardsDB; break;
                default: cardsDB = new(); Debug.LogWarning("GetDifferentClassCards in database didn't find class."); break;
            }

            _cardsByClassAndRarityDB[classType] = new();
            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
            {
                _cardsByClassAndRarityDB[classType][rarity] = new();
                _cardsByClassAndRarityDB[classType][rarity] = cardsDB.Where(card => card.Rarity == rarity).ToList();
            }
        }

    }
    #endregion
    

}
