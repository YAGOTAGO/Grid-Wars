using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    [HideInInspector] public HexNode NodeMouseIsOver; //Node that mouse is over
    void Awake()
    {
        Instance= this;
    }
    
}
