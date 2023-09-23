using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SurfaceSync : NetworkBehaviour
{
    public static SurfaceSync Instance;
    private NetworkList<int> _objectIds;
    private NetworkList<int> _rarity;

    public List<int> ObjectsDebug;
    public List<int> RarityDebug;

    private void Awake()
    {
        Instance = this;
        _rarity = new();
        _objectIds = new();
    }

    public bool ContainsID(int id)
    {
        return _objectIds.Contains(id);
    }

    public void SetRarity(int id, Rarity rarity)
    {
        if(IsServer)
        {
            ObjectsDebug.Add(id);
            RarityDebug.Add((int)rarity);
            _objectIds.Add(id);
            _rarity.Add((int)rarity);
        }
        else
        {
            SetRarityServerRPC(id, (int)rarity);
        }
        
    }

    [ServerRpc(RequireOwnership =false)]
    private void SetRarityServerRPC(int id, int rarity)
    {
        ObjectsDebug.Add(id);
        RarityDebug.Add(rarity);
        _objectIds.Add(id);
        _rarity.Add(rarity);
    }

    public Rarity GetRarity(int id)
    {
        int index = _objectIds.IndexOf(id);
        return (Rarity)_rarity[index];
    }
/*
    public void RemoveRarity(int id)
    {
        if (IsServer)
        {
            int index = _objectIds.IndexOf(id);
            ObjectsDebug.Remove(id);
            RarityDebug.RemoveAt(index);
            _objectIds.Remove(id);
            _rarity.RemoveAt(index);
        }
        else
        {
            RemoveRarityServerRPC(id);
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void RemoveRarityServerRPC(int id)
    {
        int index = _objectIds.IndexOf(id);
        _objectIds.Remove(id);
        _rarity.RemoveAt(index);
        ObjectsDebug.Remove(id);
        RarityDebug.RemoveAt(index);
    }*/
}
