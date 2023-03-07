using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IAbility
{
    int Cooldown { get; }
    TextMeshPro Description { get; }
    int Range { get; }
    Sprite Sprite { get; }

    void DoAbility();
    

}
