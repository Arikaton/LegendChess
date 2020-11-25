using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IInteractible
{
    public Move moveComponent;

    private void Start()
    {
        moveComponent = GetComponent<Move>();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
