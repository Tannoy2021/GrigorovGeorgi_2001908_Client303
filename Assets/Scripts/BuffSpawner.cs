using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public MeshRenderer Model;
    public int spawnerId;
    public bool hasItem;
    private Vector3 basePosition;
    public void Initialize(int _spawnerId, bool _hasItem)
    {
        spawnerId = _spawnerId;       hasItem = _hasItem;      Model.enabled = _hasItem;      basePosition = transform.position;
    }
    public void BuffSpawned()
    {
        hasItem = true; Model.enabled = true;
    }
    public void BuffPickedUp()
    {
        hasItem = false; Model.enabled = false;
    }
}
