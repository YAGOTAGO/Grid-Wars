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

    public void ClearTargetAndRange()
    {
        if (IsServer)
        {
            ClearRangeMap();
            ClearTargetMap();
            ClearTargetRangeClientRPC(); //server can execute on all clients
        }
        else
        {
            ClearRangeMap();
            ClearTargetMap();
            ClearTargeRangeServerRPC(); //client has to ask server to execute
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void ClearTargeRangeServerRPC()
    {
        ClearRangeMap();
        ClearTargetMap();
    }

    [ClientRpc]
    private void ClearTargetRangeClientRPC()
    {
        if(!IsServer)
        {
            ClearRangeMap();
            ClearTargetMap();
        }
    }

    public void ClearTargetMap()
    {
        _targetMap.ClearAllTiles();
    }
    private void ClearRangeMap()
    {
        _rangeMap.ClearAllTiles();
    }

    #region character highlighting
    public void CharacterHighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, _hoverTile);
    }

    public void CharacterUnhighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, null);
    }
    #endregion

    //Highlights tile at grid pos
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
        
    private void TargetHighlight(Vector3Int pos)
    {
        _targetMap.SetTile(pos, _targetTile);
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
