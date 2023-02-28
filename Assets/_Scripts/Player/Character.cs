using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{   
    public HashSet<AbstractEffect> effects;
    [SerializeField] private int _health;

    // Start is called before the first frame update
    void Start()
    {
        effects = new();
        
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
