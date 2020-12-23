using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private MoveController moveController;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var character = hitInfo.transform.GetComponent<CharacterDataContainer>();
                var field = hitInfo.transform.GetComponent<Field>();
                
                if (field != null)
                    moveController.OnTapToField(field);
                moveController.OnTapToCharacter(character);
            }
        }
    }
}
