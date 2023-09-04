using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Character : AbstractCharacter //may need to become network behaviour
{
    private HashSet<AbstractEffect> _effects = new();
    private static readonly int _startingHealth = 20;

    public NetworkVariable<int> HealthNetVar = new();
    public override HashSet<AbstractEffect> Effects { get=> _effects; }
    public override int Health { get => HealthNetVar.Value; } //this will likely be network variable
    public override int CharacterID => CharacterIDNetVar.Value;

    #region Visuals
    [Header("Visuals")]
    [SerializeField] private GameObject _effectsUIGroup; //This contains the horizontal layout group UI
    [SerializeField] private HealthBar _healthBar; //info about player healthbar
    [SerializeField] private GameObject _characterStatsUI; //UI object that holds everything else
    [SerializeField] private GameObject _effectUIPrefab;
    #endregion

    private readonly Dictionary<AbstractEffect, GameObject> _effectToUIDict = new();

    public NetworkVariable<int> CharacterIDNetVar = new(-1);

    void Start()
    {
        InitVars();
        AddEffect(new BurnEffect());
    }

    public override void OnNetworkSpawn()
    {
        HealthNetVar.Value = _startingHealth;
        CharacterIDNetVar.OnValueChanged += AddThisToCharacterDB;
        HealthNetVar.OnValueChanged += UpdateClientsHealthBar;
        HexGridPosition.OnValueChanged += UpdateNodeOn;
    }

    public override void OnNetworkDespawn()
    {
        CharacterIDNetVar.OnValueChanged -= AddThisToCharacterDB;
        HealthNetVar.OnValueChanged += UpdateClientsHealthBar;
        HexGridPosition.OnValueChanged -= UpdateNodeOn;

    }
    private void AddThisToCharacterDB(int preVal, int newVal)
    {
        Database.Instance.PlayerCharactersDB[newVal] = this;
        Database.Instance.debugcheck.Add(this);
    }

    private void InitVars()
    {
        _healthBar.InitHealthBarUI(_startingHealth, _startingHealth);
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
        if(IsServer)
        {
            HealthNetVar.Value -= damage;
        }
        else
        {
            TakeDamageServerRPC(damage);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRPC(int damage)
    {
        HealthNetVar.Value -= damage;
    }

    private void UpdateClientsHealthBar(int preVal, int newVal)
    {
        _healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Destroy(this);
        }
    }

    public GameObject GetEffectGameObject(AbstractEffect ef)
    {
        if (_effectToUIDict.TryGetValue(ef, out GameObject value))
        {
            return value;
        }

        Debug.LogWarning("Could not find effect GO in Character");
        return null;
    }

    public void HighlightCharacter(bool highlight)
    {
        if(highlight)
        {
            HighlightManager.Instance.RangeHighlight(GetNodeOn().GridPos.Value);
        }
        else
        {
            HighlightManager.Instance.RangeUnhighlight(GetNodeOn().GridPos.Value);
        }
    }

    #region UI Effect flash
    /// <summary>
    /// Call this with effect to flash the corresponding effect in player UI
    /// </summary>
    /// <param name="ef"></param>
    public void FlashEffect(AbstractEffect ef)
    {
       StartCoroutine(FlashEffectRoutine(ef));

    }

    //May need to adjust this when adding npcs
    private IEnumerator FlashEffectRoutine(AbstractEffect ef)
    {
        GameObject effectObj = _effectToUIDict[ef];
        if (effectObj == null) { Debug.Log("No effect for flash effect"); yield break; }

        for (int i = 0; i < 2; i++)
        {
            Tween scaleUp = effectObj.transform.DOScale(new Vector3(2, 2), .5f);
            yield return scaleUp.WaitForCompletion();

            Tween scaleDown = effectObj.transform.DOScale(new Vector3(1, 1), .5f);
            yield return scaleDown.WaitForCompletion();
        }

    }

    #endregion
}
