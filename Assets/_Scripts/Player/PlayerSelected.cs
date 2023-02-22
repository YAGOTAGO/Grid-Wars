using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerSelected : MonoBehaviour
{
    //Highlight is what will follow the currrently selected player
    [SerializeField] private GameObject highlight;

    private bool isPlayerSelected = false;
    public bool cursorInside { get; private set; }
        
    public bool IsPlayerSelected(){return isPlayerSelected;}

    private void OnMouseEnter()
    {
        cursorInside= true;
    }

    private void OnMouseExit()
    {
        cursorInside= false;
    }


    //If cursor is inside and mouse is clicked player is selected or deselected
    private void Update()
    {

        if (cursorInside && Input.GetMouseButtonDown(0))
        {
            isPlayerSelected = !isPlayerSelected;
            highlight.SetActive(isPlayerSelected);

        }

    }

}
