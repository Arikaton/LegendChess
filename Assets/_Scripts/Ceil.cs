using LegendChess.Charactrer;
using LegendChess.Contracts;
using UnityEngine;

namespace LegendChess
{
    public class Ceil : MonoBehaviour, IInteractable
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public Vector2Int Position {get; private set;}
        public bool IsBusy => Character != null;
        public Character Character { get; set; }
        public bool IsHighlighted { get; private set; }
    
        [SerializeField] private Material highlightedMaterial;
        [SerializeField] private GameObject attackSphere;
        [SerializeField] private GameObject moveSphere;
        private Material _startMaterial;
        private MeshRenderer _meshRenderer;
    
        private void Awake()
        {
            var position = transform.position;
            PositionX = Mathf.CeilToInt(position.x);
            PositionY = Mathf.CeilToInt(position.z);
            Position = new Vector2Int(PositionX, PositionY);
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
            DisableEffects();
        }

        private void DisableEffects()
        {
            attackSphere.SetActive(false);
            moveSphere.SetActive(false);
        }

        public void OnInteract()
        {
            if (IsHighlighted)
            {
                Character.ActiveCharacter?.OnTapOnCeil(this);
            }
            else
            {
                Character.ActiveCharacter?.OnTapOnCeil(null);
            }
        }
    }
}
