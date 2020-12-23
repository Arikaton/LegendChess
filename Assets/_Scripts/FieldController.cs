using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private int horizontalSize;
    [SerializeField] private int verticalSize;
    [SerializeField] private Transform fieldsContainer;

    private List<Field> _fields;
    public List<Field> ActiveFields { get; private set; }

    private Field[,] _fieldsMatrix;

    private void Start()
    {
        ActiveFields = new List<Field>();
        _fields = new List<Field>(fieldsContainer.childCount);
        foreach (var field in fieldsContainer.GetComponentsInChildren<Field>())
        {
            _fields.Add(field);
        }
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

    public void TurnOnFields(int lenght, Vector2Int startPos, CharacterMoveType characterMoveType)
    {
        GetPossibleFields(lenght, startPos, characterMoveType);
        ActiveFields.ForEach(field => field.TurnOn());
    }

    public void TurnOffFields()
    {
        ActiveFields.ForEach(field => field.TurnOff());
        ActiveFields = new List<Field>();
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

    private void GetPossibleFields(int maxDistance, Vector2Int position, CharacterMoveType moveType)
    {
        switch (moveType)
        {
            case CharacterMoveType.Straight:
                for (int i = 1; i <= maxDistance; i++)
                {
                    if (CeilExistAndUsable(position.x + i, position.y))
                        ActiveFields.Add(_fieldsMatrix[position.x + i, position.y]);
                    if (CeilExistAndUsable(position.x - i, position.y))
                        ActiveFields.Add(_fieldsMatrix[position.x - i, position.y]);
                    if (CeilExistAndUsable(position.x, position.y + i))
                        ActiveFields.Add(_fieldsMatrix[position.x, position.y + i]);
                    if (CeilExistAndUsable(position.x, position.y - i))
                        ActiveFields.Add(_fieldsMatrix[position.x, position.y - i]);
                }
                break;
            case CharacterMoveType.AnyCeil:
                for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
                {
                    for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                    {
                        if (i == position.x && j == position.y)
                            continue;
                        if (CeilExistAndUsable(i, j))
                        {
                            ActiveFields.Add(_fieldsMatrix[i, j]);
                        }
                    }
                }
                break;
            case CharacterMoveType.Diagonally:
                for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
                {
                    for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                    {
                        if (i == position.x && j == position.y)
                            continue;
                        if (Mathf.Abs(i - position.x) == Mathf.Abs(j - position.y))
                        {
                            if (CeilExistAndUsable(i, j))
                                ActiveFields.Add(_fieldsMatrix[i, j]);
                        }
                    }
                }
                break;
        }
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
