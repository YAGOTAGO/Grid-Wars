using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [HideInInspector] public static SelectionManager Instance;
    private Character selected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Character clicked = MouseManager.Instance.GetNodeFromMouse().GetCharacter();

            if (IsThisSelected(clicked))
            {
                //Do unselected action
                selected = null;
            }
            else
            {
                selected = clicked;
                //Do selected action
            }

            //Call on character selected actions
        }   
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public bool IsThisSelected(Character go)
    {
        return go.Equals(selected);
    }

}
