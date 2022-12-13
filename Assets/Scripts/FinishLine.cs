using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public int FinishId;
    public bool hasItem = true;
    public MeshRenderer Model;
    private Vector3 basePosition;
    //private void Update()
    //{
    //}
    public void Initialize(int _FinishId, bool _hasItem)
    {
       FinishId = _FinishId;       hasItem = _hasItem;       Model.enabled = _hasItem;       basePosition = transform.position;
    }
    public void FinishCollected()
    {
        hasItem = false; Model.enabled = false;
    }
}
