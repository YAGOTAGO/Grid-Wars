using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : NetworkBehaviour
{
    public static HighlightManager Instance;

    [Header("Tiles")]
    [SerializeField] private Tile _hoverTile;
    [SerializeField] private Tile _targetTile;

    [Header("Tile Maps")]
    [SerializeField] private Tilemap _hoverMap;
    [SerializeField] private Tilemap _rangeMap;
    [SerializeField] private Tilemap _targetMap;

    private void Awake(){ Instance = this; }

    #region TileMaps Clear
    public void ClearTargetAndRange()
    {
        if (IsServer)
        {
            ClearTiles();
            ClearTargetRangeClientRPC();
        }
        else
        {
            ClearTiles();
            ClearTargeRangeServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClearTargeRangeServerRPC()
    {
        ClearTiles();
    }

    [ClientRpc]
    private void ClearTargetRangeClientRPC()
    {
        if(!IsServer)
        {
            ClearTiles();
        }
    }

    private void ClearTiles()
    {
        _rangeMap.ClearAllTiles();
        _targetMap.ClearAllTiles();
    }

    public void ClearTargetMap()
    {
        if (IsServer)
        {
            _targetMap.ClearAllTiles();
            ClearTargetClientRPC();
        }
        else
        {
            _targetMap.ClearAllTiles();
            ClearTargetServerRPC();
        }
        
    }

    [ClientRpc]
    private void ClearTargetClientRPC()
    {
        if (!IsServer)
        {
            _targetMap.ClearAllTiles();
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void ClearTargetServerRPC()
    {
        _targetMap.ClearAllTiles();
    }

    #endregion
    #region Character Highlighting
    public void CharacterHighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, _hoverTile);
    }

    public void CharacterUnhighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, null);
    }
    #endregion

    #region Range Highlighting
    private void RangeHighlight(Vector3Int gridPos)
    {
        if(IsServer)
        {
            _rangeMap.SetTile(gridPos, _hoverTile);
            RangeHighlightClientRPC(gridPos);
        }
        else
        {
            _rangeMap.SetTile(gridPos, _hoverTile);
            RangeHighlightServerRPC(gridPos);
        }
    }
        
    [ServerRpc(RequireOwnership = false)]
    private void RangeHighlightServerRPC(Vector3Int pos)
    {
        _rangeMap.SetTile(pos, _hoverTile);
    }

    [ClientRpc]
    private void RangeHighlightClientRPC(Vector3Int pos)
    {
        if(!IsServer)
        {
            _rangeMap.SetTile(pos, _hoverTile);
        }
    }
    #endregion

    #region Target Highlighting
    private void TargetHighlight(Vector3Int gridPos)
    {
        if (IsServer)
        {
            _targetMap.SetTile(gridPos, _targetTile);
            TargetHighlightClientRPC(gridPos);
        }
        else
        {
            _targetMap.SetTile(gridPos, _targetTile);
            TargetHighlightServerRPC(gridPos);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TargetHighlightServerRPC(Vector3Int pos)
    {
        _targetMap.SetTile(pos, _targetTile);
    }

    [ClientRpc]
    private void TargetHighlightClientRPC(Vector3Int pos)
    {
        if (!IsServer)
        {
            _targetMap.SetTile(pos, _targetTile);
        }
    }

    #endregion
    public void HighlightHover(HexNode hex, bool highlight)
    {
        Vector3Int currGridPos = hex.GridPos.Value;

        if (highlight)
        {
            _hoverMap.SetTile(currGridPos, _hoverTile);
        }
        else
        {
            _hoverMap.SetTile(currGridPos, null);
        }
    }

    public void HighlightRangeList(List<HexNode> visited)
    {
        foreach (HexNode node in visited)
        {
            RangeHighlight(node.GridPos.Value);
        }
    }

    public void HighlightTargetList(List<HexNode> visited)
    {
        foreach(HexNode node in visited)
        {
            TargetHighlight(node.GridPos.Value);
        }
    }

}
