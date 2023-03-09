using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Character : MonoBehaviour
{   
    public HashSet<AbstractEffect> Effects {  get; private set; }
    public List<AbstractAbility> Abilities { get; private set; }

    [SerializeField] private int _health;
    [SerializeField] private GameObject _playerHighlight;
    [SerializeField] private GameObject _playerUI;
    private PlayerMovement _pMove;
        
    // Start is called before the first frame update
    void Start()
    {
        Effects = new();
        _pMove = GetComponent<PlayerMovement>();
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

        //Show moves path
        _pMove.PlayerSelected();

        //Sets highlight
        _playerHighlight.SetActive(true);

    }

    public void OnUnselected()
    {
        //Removes player UI
        _playerUI.SetActive(false);

        //Removes moves maps
        _pMove.PlayerUnselected();

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
        return _pMove.OnNode;
    }

}
