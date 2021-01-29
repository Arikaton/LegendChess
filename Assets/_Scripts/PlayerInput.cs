using LegendChess.Contracts;
using UnityEngine;

namespace LegendChess
{
    public class PlayerInput : MonoBehaviour
    {
        private Camera _camera;
        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    var hitTransform = hitInfo.transform;
                    if (hitTransform is null)
                    {
                        return;
                    }

                    var interactible = hitTransform.GetComponent<IInteractable>();
                    if (interactible is null) return;
                    interactible.OnInteract();
                }
            }
        }
    }
}
