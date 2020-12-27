using UnityEngine;

public class Ceil : MonoBehaviour
{
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public bool IsBusy { get; set; }
    public bool IsHighlighted { get; private set; }
    
    [SerializeField] private Material highlightedMaterial;
    private Material _startMaterial;
    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        var position = transform.position;
        PositionX = Mathf.CeilToInt(position.x);
        PositionY = Mathf.CeilToInt(position.z);
        _meshRenderer = GetComponent<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
    }

    public void TurnOn()
    {
        IsHighlighted = true;
        _meshRenderer.material = highlightedMaterial;
    }

    public void TurnOff()
    {
        IsHighlighted = false;
        _meshRenderer.material = _startMaterial;
    }
}
