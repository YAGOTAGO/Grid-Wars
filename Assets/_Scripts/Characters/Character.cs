using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : AbstractCharacter //may need to become network behaviour
{
    private HexNode _nodeOn;
    private HashSet<AbstractEffect> _effects = new();
    private int _health = 20;
    public override HashSet<AbstractEffect> Effects { get=> _effects; }
    public override HexNode NodeOn { get => _nodeOn; set => _nodeOn = value; }
    public override int Health { get => _health; set => _health = value; } //this will likely be network variable
   
    #region Visuals
    [Header("Visuals")]
    [SerializeField] private GameObject _effectsUIGroup; //This contains the horizontal layout group UI
    [SerializeField] private HealthBar _healthBar; //info about player healthbar
    [SerializeField] private GameObject _characterStatsUI; //UI object that holds everything else
    [SerializeField] private GameObject _effectUIPrefab;
    #endregion

    private readonly Dictionary<AbstractEffect, GameObject> _effectToUIDict = new();

    // Start is called before the first frame update
    void Start()
    {
        InitVars();
    }

    private void InitVars()
    {
        _healthBar.InitHealthBarUI(Health, Health);
        PlayersUIManager.Instance.SetPlayerUI(_characterStatsUI);
    }

    public override void AddEffect(AbstractEffect ef)
    {
        //Add to set if doesnt exist    
        if (!_effects.Contains(ef)) 
        { 
            _effects.Add(ef);
            SetEffectUI(ef);
            return;
        }

        //If exists in hashset we find the effect and extend duration
        foreach (AbstractEffect effect in _effects)
        {
            //Equals is overriden so should compare type
            if (effect.Equals(ef))
            {
                effect.AddToDuration(ef.Duration);
                UpdateEffectDescrip(effect);
            }
        }

    }

    /// <summary>
    /// Removes effect from character effects list, and also their UI
    /// </summary>
    /// <param name="ef">The effect to be removed</param>
    public override void RemoveEffect(AbstractEffect ef)
    {
        //Remove from the character set
        _effects.Remove(ef);

        if (_effectToUIDict.TryGetValue(ef, out GameObject value))
        {
            Destroy(value);
            _effectToUIDict.Remove(ef);
        }

    }

    /// <summary>
    /// Updates the hover tooltip of the effect UI object
    /// </summary>
    /// <param name="ef">The effect we want to update the description of</param>
    public void UpdateEffectDescrip(AbstractEffect ef)
    {
        _effectToUIDict[ef].GetComponent<HoverTip>().SetDescription(ef.Description);
    }

    //Adds all the components and sets them
    private void SetEffectUI(AbstractEffect ef)
    {

        //Instatiate the UI element and assign it to the horizontal group
        GameObject efUI = Instantiate(_effectUIPrefab, _effectsUIGroup.transform);
        efUI.name = ef.ToString(); //Name the gameobject
        efUI.GetComponent<HoverTip>().SetDescription(ef.Description); //update description of Hover tip
        efUI.GetComponent<Image>().sprite = ef.EffectIcon; //update image

        //Cache this game object for future
        _effectToUIDict[ef] = efUI;

    }

    public override void TakeDamage(int damage)
    {
        //Take the damage and update health bar
        Health -= damage;
        _healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Destroy(this);
        }
    }


}
