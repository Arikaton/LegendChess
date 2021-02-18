using LegendChess.Charactrer;
using LegendChess.Contracts;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private CameraSwitcher cameraSwitcher;
        [SerializeField] private SquadType squadType = SquadType.White;
        
        private Camera mainCamera;
        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    var hitTransform = hitInfo.transform;
                    if (hitTransform is null) return;
                    var interactible = hitTransform.GetComponent<IInteractible>();
                    if (interactible is null) return;
                    interactible.OnInteract(squadType);
                }
            }
        }

        public void ChangeSquad()
        {
            squadType = squadType == SquadType.Black ? SquadType.White : SquadType.Black;
            Character.ActiveCharacter?.HideVisual();
            cameraSwitcher.SwitchPosition();
        }
    }
}
