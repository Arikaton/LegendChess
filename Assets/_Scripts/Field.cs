using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector2Int Position => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));
    public bool IsBusy { get; set; }
    
    [SerializeField] private Material highlightedMaterial;
    private Material _startMaterial;
    private MeshRenderer _meshRenderer;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
    }

    public void TurnOn()
    {
        _meshRenderer.material = highlightedMaterial;
    }

    public void TurnOff()
    {
        _meshRenderer.material = _startMaterial;
    }
}
