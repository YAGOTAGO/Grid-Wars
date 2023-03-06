using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Character : MonoBehaviour
{   
    public HashSet<AbstractEffect> effects;
    [SerializeField] private int _health;
    private PlayerMovement _pMove;

    // Start is called before the first frame update
    void Start()
    {
        effects = new();
        _pMove = GetComponent<PlayerMovement>();
    }

    //TO:DO
    public void AddEffect(AbstractEffect ef)
    {
        
    }

    //May need to do different behavior based on if ally or enemy
    public void OnSelected()
    {
        //Show ability UI

        //Show moves path
        _pMove.PlayerSelected();

    }

    public void OnUnselected()
    {
        _pMove.PlayerUnselected();
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
