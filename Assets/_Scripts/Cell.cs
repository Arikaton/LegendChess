using LegendChess.Charactrer;
using LegendChess.Contracts;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess
{
    public class Cell : MonoBehaviour, IInteractible
    {
        public int PositionX => Mathf.CeilToInt(transform.position.x);
        public int PositionY => Mathf.CeilToInt(transform.position.z);
        public Vector2Int Position
        {
            get
            {
                position ??= new Vector2Int(PositionX, PositionY);
                return position.Value;
            }
        }
        public bool IsBusy => PlaceHolder != null;
        public GameObject PlaceHolder { get; set; }
        public SquadType SquadType { get; set; } = SquadType.NotMatter;
        public bool IsHighlighted { get; private set; }
        [SerializeField] private Material highlightedMaterial;
        [SerializeField] private GameObject attackSphere;
        [SerializeField] private GameObject moveSphere;
        private Material startMaterial;
        private MeshRenderer meshRenderer;
        private Vector2Int? position;
    
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            startMaterial = meshRenderer.material;
        }

        public void TurnOn()
        {
            IsHighlighted = true;
            meshRenderer.material = highlightedMaterial;
        }

        public void TurnOff()
        {
            IsHighlighted = false;
            meshRenderer.material = startMaterial;
            DisableEffects();
        }

        private void DisableEffects()
        {
            attackSphere.SetActive(false);
            moveSphere.SetActive(false);
        }

        public void OnInteract(SquadType interactorSquadType)
        {
            if (PlaceHolder is null)
                Character.ActiveCharacter?.OnTapOnCeil(IsHighlighted ? this : null);
            else
            {
                var character = PlaceHolder.GetComponent<Character>();
                if (Character.ActiveCharacter is null)
                {
                    character.OnInteract(interactorSquadType);
                }
                else
                {
                    Character.ActiveCharacter?.OnTapOnCeil(IsHighlighted ? this : null);
                }
            }
        }
    }
}
