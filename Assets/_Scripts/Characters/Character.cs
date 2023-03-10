using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public HashSet<AbstractEffect> Effects { get; private set; }
    public List<AbstractAbility> Abilities { get; private set; }

    #region Visuals
    [SerializeField] private GameObject _playerHighlight;
    [SerializeField] private GameObject _playerUI;
    #endregion

    #region Stats
    [SerializeField] private int _health;

    #endregion
    
    private HexNode OnNode;

    #region Move Stats
    [Header("Movement stats")]
    public float PlayerSpeed = 10f;
    public iTween.EaseType EaseType;
    public int Moves = 5;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Effects = new();
        Abilities = new();

        //Some spawn action
        SetNodeOn(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)]);
        GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)].SetCharacter(this);
        GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)].IsWalkable = false;
        
    }

    //TO:DO
    public void AddEffect(AbstractEffect ef)
    {
        //If already in set should increase the duration
        if(Effects.Contains(ef))
        {
            //Figure out how to increment duration in hashset
        }
        else
        {
            Effects.Add(ef);
        }

        //Otherwise add to the set
    }

    //May need to do different behavior based on if ally or enemy
    public void OnSelected()
    {
        //Show ability UI
        _playerUI.SetActive(true);

        //Sets highlight
        _playerHighlight.SetActive(true);

    }

    public void OnUnselected()
    {
        //Removes player UI
        _playerUI.SetActive(false);

        //Cannot make move
        MovementManager.Instance.SetCanMoveToFalse();

        //Unhighlights player
        _playerHighlight.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(this);
        }

    }

    public HexNode GetNodeOn()
    {
        return OnNode;
    }

    public void SetNodeOn(HexNode node)
    {
        OnNode = node;
    }


}
