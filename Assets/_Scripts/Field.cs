using System.Collections.Generic;
using _Scripts.Charactrer;
using _Scripts.Enums;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private int horizontalSize;
    [SerializeField] private int verticalSize;
    [SerializeField] private Transform fieldsContainer;

    private List<Ceil> _fields;
    private List<Ceil> HighlightedFields { get; set; }

    private Ceil[,] _fieldsMatrix;

    private void Awake()
    {
        Init();
        DefineFieldsMatrix();
    }
    
    private void Init()
    {
        HighlightedFields = new List<Ceil>();
        _fields = new List<Ceil>(fieldsContainer.childCount);
        foreach (var field in fieldsContainer.GetComponentsInChildren<Ceil>())
        {
            _fields.Add(field);
        }
    }

    private void DefineFieldsMatrix()
    {
        _fieldsMatrix = new Ceil[horizontalSize, verticalSize];
        foreach (var field in _fields)
        {
            _fieldsMatrix[field.PositionX, field.PositionY] = field;
        }
    }

    public Ceil HighlightCeil(Vector2Int position)
    {
        var ceil = _fieldsMatrix[position.x, position.y];
        HighlightedFields.Add(ceil);
        ceil.TurnOn();
        return ceil;
    }

    public void HighlightCeilAndShowEffect(Vector2Int position, EffectType effectType)
    {
        var ceil = HighlightCeil(position);
        switch (effectType)
        {
            case EffectType.Attack:
               ceil.ShowAttack();
               break;
            case EffectType.Move:
                ceil.ShowMove();
                break;
        }
    }

    public void TurnOnFields(int lenght, Vector2Int startPos, MoveDirection moveDirection)
    {
        GetPossibleFields(lenght, startPos, moveDirection);
        HighlightedFields.ForEach(field => field.TurnOn());
    }

    public void SetCeilBusy(Vector2Int position, Character character)
    {
        _fieldsMatrix[position.x, position.y].Character = character;
    }
    
    public void SetCeilFree(Vector2Int position)
    {
        _fieldsMatrix[position.x, position.y].Character = null;
    }

    public bool GetCeilBusyState(Vector2Int index) => _fieldsMatrix[index.x, index.y].IsBusy;

    public Character GetCharacterByIndex(Vector2Int index) => _fieldsMatrix[index.x, index.y].Character;

    public void TurnOffFields()
    {
        HighlightedFields.ForEach(field => field.TurnOff());
        HighlightedFields.Clear();
    }

    private void GetPossibleFields(int maxDistance, Vector2Int position, MoveDirection moveType)
    {
        switch (moveType)
        {
            case MoveDirection.Straight:
                StraightPass(maxDistance, position);
                AddCeilToHighlighted(position);
                break;
            case MoveDirection.Diagonally:
                DiagonallyPass(maxDistance, position);
                AddCeilToHighlighted(position);
                break;
            case MoveDirection.Asterisk:
                StraightPass(maxDistance, position);
                DiagonallyPass(maxDistance, position);
                AddCeilToHighlighted(position);
                break;
            case MoveDirection.Any:
                SimplePass(maxDistance, position);
                break;
        }
    }

    private void AddCeilToHighlighted(Vector2Int position)
    {
        HighlightedFields.Add(_fieldsMatrix[position.x, position.y]);
    }

    private void SimplePass(int maxDistance, Vector2Int position)
    {
        for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
        {
            for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
            {
                if (i == position.x && j == position.y)
                    continue;
                if (CeilExist(i, j))
                {
                    HighlightedFields.Add(_fieldsMatrix[i, j]);
                }
            }
        }
    }

    private void DiagonallyPass(int maxDistance, Vector2Int position)
    {
        var currentX = position.x + 1;
        var currentY = position.y + 1;
        while (CeilExistAndNotBusy(currentX, currentY) && (currentX - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX++;
            currentY++;
        }

        currentX = position.x - 1;
        currentY = position.y + 1;
        while (CeilExistAndNotBusy(currentX, currentY) && (position.x - currentX) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX--;
            currentY++;
        }

        currentX = position.x + 1;
        currentY = position.y - 1;
        while (CeilExistAndNotBusy(currentX, currentY) && (currentX - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX++;
            currentY--;
        }

        currentX = position.x - 1;
        currentY = position.y - 1;
        while (CeilExistAndNotBusy(currentX, currentY) && (position.x - currentX) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentX, currentY]);
            currentX--;
            currentY--;
        }
    }

    private void StraightPass(int maxDistance, Vector2Int position)
    {
        var currentXPos = position.x + 1;
        while (CeilExistAndNotBusy(currentXPos, position.y) && (currentXPos - position.x) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentXPos, position.y]);
            currentXPos++;
        }

        currentXPos = position.x - 1;
        while (CeilExistAndNotBusy(currentXPos, position.y) && (position.x - currentXPos) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[currentXPos, position.y]);
            currentXPos--;
        }

        var currentYPos = position.y + 1;
        while (CeilExistAndNotBusy(position.x, currentYPos) && (currentYPos - position.y) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[position.x, currentYPos]);
            currentYPos++;
        }

        currentYPos = position.y - 1;
        while (CeilExistAndNotBusy(position.x, currentYPos) && (position.y - currentYPos) <= maxDistance)
        {
            HighlightedFields.Add(_fieldsMatrix[position.x, currentYPos]);
            currentYPos--;
        }
    }

    private bool CeilExistAndNotBusy(int i, int j)
    {
        if (!CeilExist(i, j))
            return false;
        if (GetCeilBusyState(new Vector2Int(i, j)))
            return false;
        return true;
    }

    private bool CeilExist(int i, int j)
    {
        if (i >= horizontalSize || i < 0)
            return false;
        if (j >= verticalSize || j < 0)
            return false;
        return true;
    }
    
}
