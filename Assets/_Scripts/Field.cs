using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private Material highlightedMaterial;
    private Material _startMaterial;
    //private Outline _outline;
    private MeshRenderer _meshRenderer;
    

    public Vector2Int Position => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

    private void Start()
    {
        //_outline = GetComponent<Outline>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
        //TurnOff();
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
