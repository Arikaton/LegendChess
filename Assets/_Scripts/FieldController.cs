using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private int horizontalSize;
    [SerializeField] private int verticalSize;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform fieldsContainer;

    private List<Field> _fields;
    private List<Field> _activeFields = new List<Field>();

    private Field[,] _fieldsMatrix;

    private void Start()
    {
        _fields = new List<Field>(fieldsContainer.childCount);
        foreach (var field in fieldsContainer.GetComponentsInChildren<Field>())
        {
            _fields.Add(field);
        }
        characterController.CharacterChanged += OnChangeCharacter;
        DefineFieldsMatrix();
    }

    private void DefineFieldsMatrix()
    {
        _fieldsMatrix = new Field[horizontalSize, verticalSize];
        foreach (var field in _fields)
        {
            var fieldPosition = field.Position;
            _fieldsMatrix[fieldPosition.x, fieldPosition.y] = field;
        }
    }

    private void OnChangeCharacter(Character character)
    {
        if (character is null)
        {
            TurnOffFields();
            return;
        }
        var maxCharacterDistance = character.moveComponent.MaxMoveDistance;
        var charPos = character.moveComponent.Position;
        GetPossibleFields(maxCharacterDistance, charPos, character.moveComponent.moveType);
        foreach (var possibleField in _activeFields)
        {
            possibleField.TurnOn();
        }
    }

    public void TurnOffFields()
    {
        _activeFields.ForEach(field => field.TurnOff());
        _activeFields = new List<Field>();
    }

    public void PrintFieldsMatrix()
    {
        string holder = "";
        for (int i = 0; i < horizontalSize; i++)
        {
            for (int j = 0; j < verticalSize; j++)
            {
                if (_fieldsMatrix[j, i] != null)
                {
                    holder += "X";
                }
                else
                {
                    holder += "0";
                }
            }

            holder += "\n";
        }

        print(holder);
    }

    private List<Field> GetPossibleFields(int maxDistance, Vector2Int position, Move.MoveType moveType)
    {
        switch (moveType)
        {
            case Move.MoveType.straight:
                for (int i = 1; i <= maxDistance; i++)
                {
                    if (CeilExistAndUsable(position.x + i, position.y))
                        _activeFields.Add(_fieldsMatrix[position.x + i, position.y]);
                    if (CeilExistAndUsable(position.x - i, position.y))
                        _activeFields.Add(_fieldsMatrix[position.x - i, position.y]);
                    if (CeilExistAndUsable(position.x, position.y + i))
                        _activeFields.Add(_fieldsMatrix[position.x, position.y + i]);
                    if (CeilExistAndUsable(position.x, position.y - i))
                        _activeFields.Add(_fieldsMatrix[position.x, position.y - i]);
                }
                break;
            case Move.MoveType.anyCeil:
                for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
                {
                    for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                    {
                        if (i == position.x && j == position.y)
                            continue;
                        if (CeilExistAndUsable(i, j))
                        {
                            _activeFields.Add(_fieldsMatrix[i, j]);
                        }
                    }
                }
                break;
            case Move.MoveType.diagonally:
                for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
                {
                    for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                    {
                        if (i == position.x && j == position.y)
                            continue;
                        if (Mathf.Abs(i - position.x) == Mathf.Abs(j - position.y))
                        {
                            _activeFields.Add(_fieldsMatrix[i, j]);
                        }
                    }
                }
                break;
        }
        
        return _activeFields;
    }

    private bool CeilExistAndUsable(int i, int j)
    {
        if (i >= horizontalSize || i < 0)
            return false;
        if (j >= verticalSize || j < 0)
            return false;
        return true;
    }
    
}
