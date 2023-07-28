using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public HashSet<AbstractEffect> Effects { get; private set; } = new();
    
    #region Visuals
    [Header("Visuals")]
    [SerializeField] private GameObject _playerHighlight;
    [SerializeField] private GameObject _playerAbilityUI;
    [SerializeField] private GameObject _effectsUIGroup; //This contains the horizontal layout group UI
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private GameObject _playerStatsUI;
    [SerializeField] private GameObject _effectUIPrefab;
    #endregion
    
    #region Stats
    [Header("Stats")]
    [SerializeField] private int _health;
    #endregion

    #region Private Vars
    private Dictionary<AbstractEffect, GameObject> _effectToUIDict = new();
    #endregion

    #region Move Stats
    [Header("Movement stats")]
    public float PlayerSpeed = 3f;
    public iTween.EaseType EaseType = iTween.EaseType.spring;
    public int WalkMoves = 5;
    #endregion
    
    [HideInInspector] public HexNode NodeOn;

    // Start is called before the first frame update
    void Start()
    {
        InitVars();
        //Some spawn action
        NodeOn = GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)];
        GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)].CharacterOnNode = this;
        AddEffect(new BurnEffect());
    }

    private void InitVars()
    {
        _healthBar.InitHealthBarUI(_health, _health);
        PlayersUIManager.Instance.SetPlayerUI(_playerStatsUI);
    }

    public void AddEffect(AbstractEffect ef)
    {
        //Add to set if doesnt exist
        if (!Effects.Contains(ef)) 
        { 
            Effects.Add(ef);
            SetEffectUI(ef);
            return;
        }

        //If exists in hashset we find the effect and extend duration
        foreach (AbstractEffect effect in Effects)
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
    public void RemoveEffect(AbstractEffect ef)
    {
        //Remove from the character set
        Effects.Remove(ef);

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
        GameObject go = _effectToUIDict[ef];
        go.GetComponent<HoverTip>().SetDescription(ef.Description);
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

    //May need to do different behavior based on if ally or enemy
    public void OnSelect()
    {
        //Show ability UI
        _playerAbilityUI.SetActive(true);

        //Sets highlight
        _playerHighlight.SetActive(true);

    }

    public void OnDeselect()
    {
        //Removes player UI
        _playerAbilityUI.SetActive(false);

        //Cannot make move
        MovementManager.Instance.SetCanMoveToFalse();

        //Unhighlights player
        _playerHighlight.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        //Take the damage and update health bar
        _health -= damage;
        _healthBar.SetHealth(_health);

        if (_health <= 0)
        {
            Destroy(this);
        }
    }


}
