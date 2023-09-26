using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SurfaceSync : NetworkBehaviour
{
    public static SurfaceSync Instance;
    private NetworkList<int> _objectIds;
    private NetworkList<int> _class;

    public List<int> ObjectsDebug;
    public List<int> RarityDebug;

    private void Awake()
    {
        Instance = this;
        _class = new();
        _objectIds = new();
    }

    public bool ContainsID(int id)
    {
        return _objectIds.Contains(id);
    }

    public void SetClass(int id, Class classType)
    {
        if(IsServer)
        {
            ObjectsDebug.Add(id);
            RarityDebug.Add((int)classType);
            _objectIds.Add(id);
            _class.Add((int)classType);
        }
        else
        {
            SetRarityServerRPC(id, (int)classType);
        }
        
    }

    [ServerRpc(RequireOwnership =false)]
    private void SetRarityServerRPC(int id, int rarity)
    {
        ObjectsDebug.Add(id);
        RarityDebug.Add(rarity);
        _objectIds.Add(id);
        _class.Add(rarity);
    }

    public Class GetClass(int id)
    {
        int index = _objectIds.IndexOf(id);
        return (Class)_class[index];
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
