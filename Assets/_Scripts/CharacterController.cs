using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public event Action OnStartMove;
    public event Action OnEndMove;
    public event Action OnDie;
}
