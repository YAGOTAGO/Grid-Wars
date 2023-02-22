using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [HideInInspector] public static SelectionManager Instance;
    [HideInInspector] public GameObject selected;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public bool IsThisSelected(GameObject go)
    {
        return go.Equals(selected);
    }

}
