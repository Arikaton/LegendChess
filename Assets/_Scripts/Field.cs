using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private int horizontalSize;
    [SerializeField] private int verticalSize;
    [SerializeField] private Transform fieldsContainer;

    private List<Ceil> _fields;
    public List<Ceil> HighlightedFields { get; private set; }

    private Ceil[,] _fieldsMatrix;

    private void Awake()
    {
        HighlightedFields = new List<Ceil>();
        _fields = new List<Ceil>(fieldsContainer.childCount);
        foreach (var field in fieldsContainer.GetComponentsInChildren<Ceil>())
        {
            _fields.Add(field);
        }
        DefineFieldsMatrix();
        
    }

    private void DefineFieldsMatrix()
    {
        _fieldsMatrix = new Ceil[horizontalSize, verticalSize];
        foreach (var field in _fields)
        {
            _fieldsMatrix[field.PositionX, field.PositionY] = field;
        }
    }

    public void TurnOnFields(int lenght, Vector2Int startPos, Direction direction)
    {
        GetPossibleFields(lenght, startPos, direction);
        HighlightedFields.ForEach(field => field.TurnOn());
    }

    public void SetCeilBusy(int row, int col, bool busy)
    {
        _fieldsMatrix[row, col].IsBusy = busy;
    }

    public bool GetCeilBusy(Vector2Int index) => _fieldsMatrix[index.x, index.y].IsBusy;

    public void TurnOffFields()
    {
        HighlightedFields.ForEach(field => field.TurnOff());
        HighlightedFields = new List<Ceil>();
    }
    public override string ToString()
    {
        var holder = "";
        for (var i = 0; i < horizontalSize; i++)
        {
            for (var j = 0; j < verticalSize; j++)
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

        return holder;
    }

    private void GetPossibleFields(int maxDistance, Vector2Int position, Direction moveType)
    {
        switch (moveType)
        {
            case Direction.Straight:
                StraightPass(maxDistance, position);
                break;
            case Direction.Diagonally:
                DiagonallyPass(maxDistance, position);
                break;
            case Direction.Asterisk:
                StraightPass(maxDistance, position);
                DiagonallyPass(maxDistance, position);
                break;
        }
    }

    private void DiagonallyPass(int maxDistance, Vector2Int position)
    {
        var currentX = position.x + 1;
        var currentY = position.y + 1;
        while (CeilExistAndUsable(currentX, currentY) && (currentX - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX++;
            currentY++;
        }

        currentX = position.x - 1;
        currentY = position.y + 1;
        while (CeilExistAndUsable(currentX, currentY) && (position.x - currentX) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX--;
            currentY++;
        }

        currentX = position.x + 1;
        currentY = position.y - 1;
        while (CeilExistAndUsable(currentX, currentY) && (currentX - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX++;
            currentY--;
        }

        currentX = position.x - 1;
        currentY = position.y - 1;
        while (CeilExistAndUsable(currentX, currentY) && (position.x - currentX) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX--;
            currentY--;
        }
    }

    private void StraightPass(int maxDistance, Vector2Int position)
    {
        var currentXPos = position.x + 1;
        while (CeilExistAndUsable(currentXPos, position.y) && (currentXPos - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentXPos, position.y]);
            currentXPos++;
        }

        currentXPos = position.x - 1;
        while (CeilExistAndUsable(currentXPos, position.y) && (position.x - currentXPos) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentXPos, position.y]);
            currentXPos--;
        }

        var currentYPos = position.y + 1;
        while (CeilExistAndUsable(position.x, currentYPos) && (currentYPos - position.y) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[position.x, currentYPos]);
            currentYPos++;
        }

        currentYPos = position.y - 1;
        while (CeilExistAndUsable(position.x, currentYPos) && (position.y - currentYPos) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[position.x, currentYPos]);
            currentYPos--;
        }
    }

    private bool CeilExistAndUsable(int i, int j)
    {
        if (i >= horizontalSize || i < 0)
            return false;
        if (j >= verticalSize || j < 0)
            return false;
        if (GetCeilBusy(new Vector2Int(i, j)))
            return false;
        return true;
    }
    
}
