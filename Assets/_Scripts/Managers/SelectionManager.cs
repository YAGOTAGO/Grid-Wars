using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [HideInInspector] public static SelectionManager Instance;
    public GameObject selected;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsThisSelected(GameObject go)
    {
        return go.Equals(selected);
    }

}
