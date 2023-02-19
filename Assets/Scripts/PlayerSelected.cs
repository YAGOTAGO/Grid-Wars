using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelected : MonoBehaviour
{
    [SerializeField] private GameObject highlight;

    private bool isPlayerSelected = false;
    private bool cursorInside;
        
    public bool IsPlayerSelected()
    {
        return isPlayerSelected;
    }

    private void OnMouseEnter()
    {
        cursorInside= true;
    }

    private void OnMouseExit()
    {
        cursorInside= false;
    }


    //If cursosr is inside and mouse is clicked player is selected or deselected
    private void Update()
    {
        if (cursorInside)
        {
            if(Input.GetMouseButtonDown(0)) 
            {
                isPlayerSelected = !isPlayerSelected;
                highlight.SetActive(isPlayerSelected);
            }
        }
    }

}
