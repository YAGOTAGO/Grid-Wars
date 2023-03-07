using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Character : MonoBehaviour
{   
    public HashSet<AbstractEffect> Effects {  get; private set; }
    public List<IAbility> Abilities { get; private set; }

    [SerializeField] private int _health;
    [SerializeField] private GameObject _playerHighlight;
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

        //Otherwise add to the set
    }

    //May need to do different behavior based on if ally or enemy
    public void OnSelected()
    {
        //Show ability UI

        //Show moves path
        _pMove.PlayerSelected();
        _playerHighlight.SetActive(true);

    }

    public void OnUnselected()
    {
        _pMove.PlayerUnselected();
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
}
