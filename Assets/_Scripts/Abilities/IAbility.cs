using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IAbility
{
    int Cooldown { get; }
    int Range { get; }
    void Display();

    void DoAbility(HexNode node);
    List<HexNode> GetShape();

}
