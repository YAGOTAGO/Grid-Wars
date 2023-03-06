using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerSelected : MonoBehaviour
{
    //Highlight is what will follow the currrently selected player
    [SerializeField] private GameObject highlight;

    //private bool _isPlayerSelected = false;
    public bool CursorInside { get; private set; }

    private void OnMouseEnter()
    {
        CursorInside= true;
    }

    private void OnMouseExit()
    {
        CursorInside= false;
    }

    //If cursor is inside and mouse is clicked player is selected or deselected
    private void Update()
    {

       /* if (CursorInside && Input.GetMouseButtonDown(0))
        {
            _isPlayerSelected = !_isPlayerSelected;
            highlight.SetActive(_isPlayerSelected);

            SelectionManager.Instance.selected = _isPlayerSelected ? this.gameObject : null;
        }*/

    }

}
