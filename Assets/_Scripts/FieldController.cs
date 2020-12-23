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

    private void Awake()
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

    public void TurnOnFields(int lenght, Vector2Int startPos, Direction direction)
    {
        GetPossibleFields(lenght, startPos, direction);
        ActiveFields.ForEach(field => field.TurnOn());
    }

    public void ChangeBusyType(Vector2Int index, bool busy)
    {
        _fieldsMatrix[index.x, index.y].IsBusy = busy;
    }

    public bool GetFieldBusyInfo(Vector2Int index) => _fieldsMatrix[index.x, index.y].IsBusy;

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

    private void GetPossibleFields(int maxDistance, Vector2Int position, Direction moveType)
    {
        for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
        {
            for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
            {
                if (i == position.x && j == position.y)
                    continue;
                if (CeilExistAndUsable(i, j))
                {
                    switch (moveType)
                    {
                        case Direction.AnyCeil:
                            ActiveFields.Add(_fieldsMatrix[i, j]);
                            break;
                        case Direction.Diagonally:
                            if (Mathf.Abs(i - position.x) == Mathf.Abs(j - position.y))
                                ActiveFields.Add(_fieldsMatrix[i, j]);
                            break;
                        case Direction.Straight:
                            if (i == position.x || j == position.y)
                                ActiveFields.Add(_fieldsMatrix[i, j]);
                            break;
                    }
                }
            }
        }
    }

    private bool CeilExistAndUsable(int i, int j)
    {
        if (i >= horizontalSize || i < 0)
            return false;
        if (j >= verticalSize || j < 0)
            return false;
        if (GetFieldBusyInfo(new Vector2Int(i, j)))
            return false;
        return true;
    }
    
}
