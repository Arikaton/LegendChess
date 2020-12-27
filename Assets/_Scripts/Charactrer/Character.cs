using System;
using _Scripts;
using _Scripts.Enums;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static Character activeCharacter;

    public RelationType relationType;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Field field;
    [SerializeField] private Move move;
    [SerializeField] private Health health;
    [SerializeField] private Attack attack;

    private bool canMove = true;

    private void Awake()
    {
        playerInput.OnClickOnCharacter += OnClickOnCharacter;
        playerInput.OnEmptyClick += OnClickEmpty;
        playerInput.OnClickOnCeil += OnClickOnCeil;
    }

    private void Start()
    {
        field.SetCeilBusy(move.PositionX, move.PositionY, true);
    }

    private void OnClickOnCharacter(Character character)
    {
        if (character != this) return;
        MoveAction();
    }

    private void MoveAction()
    {
        if (move.IsMoving) return;
        if (!canMove) return;
        field.TurnOffFields();
        if (activeCharacter != this)
        {
            field.TurnOnFields(move.MaxMoveDistance, new Vector2Int(move.PositionX, move.PositionY), move.Direction);
            activeCharacter = this;
        }
        else
        {
            field.TurnOffFields();
            activeCharacter = null;
        }
    }

    private void OnClickOnCeil(Ceil ceil)
    {
        if (activeCharacter is null || activeCharacter != this)
            return;
        if (move.IsMoving) return;
        if (ceil.IsHighlighted)
        {
            field.SetCeilBusy(ceil.PositionX, ceil.PositionY, true);
            field.SetCeilBusy(move.PositionX, move.PositionY, false);
            move.DoMove(new Vector2Int(ceil.PositionX, ceil.PositionY));
        }
        field.TurnOffFields();
        activeCharacter = null;
    }
    
    private void OnClickEmpty()
    {
        field.TurnOffFields();
        activeCharacter = null;
    }
}
