using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Character : AbstractCharacter //may need to become network behaviour
{

    [SerializeField] private GameObject _statsUIPrefab;
    
    [Header("Character specific")]
    public EffectBase StartingEffect; //Passive for the unique character
    public Class CharacterClass; 
    public Sprite Icon;
    public List<CardBase> InitialCards = new ();
    
    private readonly Dictionary<EffectBase, GameObject> _effectToUIDict = new();
    private StatsUI _statsUI;
    private GameObject _statsUIGO;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        AddCharactersDB();
        AddEffect(StartingEffect);
        AddStartingCards();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        
        //Log event
        LogManager.Instance.LogOnSlain(this);
        
        //Remove stats UI
        Destroy(_statsUIGO);

        //Remove Character from DB
        RemoveCharactersDB();

        //Free up the hexnode
        HexNode node = GetNodeOn();
        node.SetSurfaceWalkable(true);
        node.SetCharacterOnNode(-1);
        
        if (Database.Instance.AllyCharacters.Count == 0) //means you have lost
        {
            //GameManager.Instance.ChangeState(GameState.EndGame);
        }
    }

    private void Initialize()
    {
        _statsUIGO = Instantiate(_statsUIPrefab);
        _statsUI = _statsUIGO.GetComponent<StatsUI>();
        
        _statsUI.Initialize(this);
        SetHealth(MaxHealth);
        
    }

    private void AddStartingCards()
    {
        if(IsOwner)
        {
            foreach (CardBase card in InitialCards)
            {
                DeckManager.Instance.AddToDeck(card);
            }

            DeckManager.Instance.ShuffleDeck();
        }
    }
    private void AddCharactersDB()
    {
        if (IsOwner)
        {
            Database.Instance.AllyCharacters.Add(this);
        }
        else
        {
            Database.Instance.EnemyCharacters.Add(this);
        }
    }

    private void RemoveCharactersDB()
    {
        Database.Instance.AbstractCharactersDB.Remove(CharacterID.Value);
        if (IsOwner) 
        {
            Database.Instance.AllyCharacters.Remove(this);
        }
        else
        {
            Database.Instance.EnemyCharacters.Remove(this);
        }
    }
    public override void AddEffect(EffectBase ef)
    {
        //Add to set if doesnt exist    
        if (!Effects.Contains(ef)) 
        { 
            Effects.Add(ef);
            SetEffectUI(ef);
            return;
        }

        //If exists in hashset we find the effect and extend duration
        foreach (EffectBase effect in Effects)
        {
            //Equals is overriden so should compare type
            if (effect.Equals(ef))
            {
                effect.AddToDuration(ef.Duration);
                UpdateEffectDescription(effect);
            }
        }

    }

    /// <summary>
    /// Removes effect from character effects list, and also their UI
    /// </summary>
    /// <param name="ef">The effect to be removed</param>
    protected override void RemoveEffect(EffectBase ef)
    {
        //Remove from the character set
        Effects.Remove(ef);

        if (_effectToUIDict.TryGetValue(ef, out GameObject value))
        {
            Destroy(value);
            _effectToUIDict.Remove(ef);
        }

        //Hide any hover tips that could still be showing
        EffectTipWindow.Instance.HideTip();
    }

    /// <summary>
    /// Updates the hover tooltip of the effect UI object
    /// </summary>
    /// <param name="ef">The effect we want to update the description of</param>
    public void UpdateEffectDescription(EffectBase ef)
    {
        _effectToUIDict[ef].GetComponent<HoverTip>().SetDescription(ef.Description);
    }

    //Adds all the components and sets them
    private void SetEffectUI(EffectBase ef)
    {
        //Instatiate the UI element and assign it to the horizontal group
        GameObject efUI = Instantiate(_statsUI.GetEffectPrefab(), _statsUI.GetEffectUIGroup());
        efUI.name = ef.ToString(); //Name the gameobject
        efUI.GetComponent<HoverTip>().SetDescription(ef.Description); //update description of Hover tip
        efUI.GetComponent<Image>().sprite = ef.EffectIcon; //update image

        //Cache this game object for future
        _effectToUIDict[ef] = efUI;
    }
    
    protected override void OnHealthChanged()
    {
        _statsUI.GetHealthBar().SetHealth(Health.Value);

        if (Health.Value <= 0)
        {
            if(IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                DespawnServerRPC();
            }
            
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRPC()
    {
        GetComponent<NetworkObject>().Despawn();
    }

    public GameObject GetEffectGameObject(EffectBase ef)
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
            HighlightManager.Instance.CharacterHighlight(GetNodeOn().GridPos.Value);
        }
        else
        {
            HighlightManager.Instance.CharacterUnhighlight(GetNodeOn().GridPos.Value);
        }
    }

    #region UI Effect flash
    /// <summary>
    /// Call this with effect to flash the corresponding effect in player UI, and potentially remove the effect
    /// </summary>
    /// <param name="ef"></param>
    public void FlashEffect(EffectBase ef, bool destroy)
    {
       StartCoroutine(FlashEffectRoutine(ef, destroy));

    }

    private IEnumerator FlashEffectRoutine(EffectBase ef, bool destroy)
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

        if (destroy)
        {
            RemoveEffect(ef);
        }

    }

    #endregion
}
