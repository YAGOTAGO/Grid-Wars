using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    private PlayerSelected _pSelect;

    private void Awake()
    {
        _pSelect = GetComponent<PlayerSelected>();
    }
    private void LateUpdate()
    {
        if (_pSelect.IsPlayerSelected() && Input.GetMouseButtonDown(0))
        {
            MovePlayerToHex(GridManager.Instance.GetTileAtMousePos());
        }
    }
     
    public void MovePlayerToHex(Vector3 position)
    {
        Hashtable args = new()
        {
            ["speed"] = _playerSpeed,
            ["easetype"] = _easeType,
            ["islocal"] = true,
            ["position"] = position
        };

        iTween.MoveTo(this.gameObject, args);
    }
}
