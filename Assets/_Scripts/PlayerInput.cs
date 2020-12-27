using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action OnEmptyClick;
    public event Action<Character> OnClickOnCharacter;
    public event Action<Ceil> OnClickOnCeil;
    
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
                    OnEmptyClick?.Invoke();
                    return;
                }

                var character = hitTransform.GetComponent<Character>();
                if (character != null)
                {
                    OnClickOnCharacter?.Invoke(character);
                    return;
                }

                var ceil = hitTransform.GetComponent<Ceil>();
                if (ceil != null)
                {
                    OnClickOnCeil?.Invoke(ceil);
                }
            }
        }
    }
}
