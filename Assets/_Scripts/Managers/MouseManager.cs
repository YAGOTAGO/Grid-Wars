using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    public HexNode NodeMouseIsOver;
    void Awake()
    {
        Instance= this;
    }
    
}
