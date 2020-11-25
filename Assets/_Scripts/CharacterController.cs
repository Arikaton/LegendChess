using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Action<Character> CharacterChanged;
    
    public Character currentCharacter;

    private Camera _camera;
    void Start()
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
                var character = hitInfo.transform.GetComponent<Character>();
                if (character != null)
                {
                    currentCharacter = character;
                    CharacterChanged(character);
                }
                else
                {
                    character = null;
                    CharacterChanged(null);
                }
            }
        }
    }
}
