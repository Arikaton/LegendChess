using System;
using System.Collections.Generic;
using LegendChess.Charactrer;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private int horizontalSize;
        [SerializeField] private int verticalSize;
        [SerializeField] private Transform fieldsContainer;

        private List<Ceil> fields;
        private List<Ceil> HighlightedFields { get; set; }

        private Ceil[,] fieldsMatrix;

        private void Awake()
        {
            Init();
            DefineFieldsMatrix();
        }
    
        private void Init()
        {
            HighlightedFields = new List<Ceil>();
            fields = new List<Ceil>(fieldsContainer.childCount);
            foreach (var field in fieldsContainer.GetComponentsInChildren<Ceil>())
            {
                fields.Add(field);
            }
        }

        private void DefineFieldsMatrix()
        {
            fieldsMatrix = new Ceil[horizontalSize, verticalSize];
            foreach (var field in fields)
            {
                fieldsMatrix[field.PositionX, field.PositionY] = field;
            }
        }

        public void HighlightCeil(Vector2Int position)
        {
            if (!CeilExist(position.x, position.y)) return;
            var ceil = fieldsMatrix[position.x, position.y];
            HighlightedFields.Add(ceil);
            ceil.TurnOn();
        }

        public void TurnOnFields(Vector2Int startPos, HighlightType highlightType, int lenght = 1, SquadType squadType = SquadType.NotMatter)
        {
            GetPossibleFields(lenght, startPos, highlightType, squadType);
            HighlightedFields.ForEach(field => field.TurnOn());
        }

        public void SetCeilBusy(Vector2Int position, Character character)
        {
            fieldsMatrix[position.x, position.y].Character = character;
        }
    
        public void SetCeilFree(Vector2Int position)
        {
            fieldsMatrix[position.x, position.y].Character = null;
        }

        public bool GetCeilBusyState(Vector2Int index) => fieldsMatrix[index.x, index.y].IsBusy;

        public Character GetCharacterByIndex(Vector2Int index) => fieldsMatrix[index.x, index.y].Character;

        public void TurnOffFields()
        {
            HighlightedFields.ForEach(field => field.TurnOff());
            HighlightedFields.Clear();
        }

        private void GetPossibleFields(int maxDistance, Vector2Int position, HighlightType moveType, SquadType squadType)
        {
            switch (moveType)
            {
                case HighlightType.Straight:
                    StraightPass(maxDistance, position);
                    AddCeilToHighlighted(position);
                    break;
                case HighlightType.Diagonally:
                    DiagonallyPass(maxDistance, position);
                    AddCeilToHighlighted(position);
                    break;
                case HighlightType.Asterisk:
                    StraightPass(maxDistance, position);
                    DiagonallyPass(maxDistance, position);
                    AddCeilToHighlighted(position);
                    break;
                case HighlightType.Any:
                    SimplePass(maxDistance, position);
                    break;
                case HighlightType.AnyExceptMiddle:
                    SimplePass(maxDistance, position);
                    break;
                case HighlightType.AnyExceptFriendly:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
            }
        }

        private void AddCeilToHighlighted(Vector2Int position)
        {
            HighlightedFields.Add(fieldsMatrix[position.x, position.y]);
        }

        private void SimplePass(int maxDistance, Vector2Int position, bool addMiddle = false)
        {
            for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
            {
                for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                {
                    if (i == position.x && j == position.y && !addMiddle)
                        continue;
                    if (CeilExist(i, j))
                    {
                        HighlightedFields.Add(fieldsMatrix[i, j]);
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
                HighlightedFields.Add(fieldsMatrix[currentX, currentY]);
                currentX++;
                currentY++;
            }

            currentX = position.x - 1;
            currentY = position.y + 1;
            while (CeilExistAndNotBusy(currentX, currentY) && (position.x - currentX) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[currentX, currentY]);
                currentX--;
                currentY++;
            }

            currentX = position.x + 1;
            currentY = position.y - 1;
            while (CeilExistAndNotBusy(currentX, currentY) && (currentX - position.x) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[currentX, currentY]);
                currentX++;
                currentY--;
            }

            currentX = position.x - 1;
            currentY = position.y - 1;
            while (CeilExistAndNotBusy(currentX, currentY) && (position.x - currentX) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[currentX, currentY]);
                currentX--;
                currentY--;
            }
        }

        private void StraightPass(int maxDistance, Vector2Int position)
        {
            var currentXPos = position.x + 1;
            while (CeilExistAndNotBusy(currentXPos, position.y) && (currentXPos - position.x) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[currentXPos, position.y]);
                currentXPos++;
            }

            currentXPos = position.x - 1;
            while (CeilExistAndNotBusy(currentXPos, position.y) && (position.x - currentXPos) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[currentXPos, position.y]);
                currentXPos--;
            }

            var currentYPos = position.y + 1;
            while (CeilExistAndNotBusy(position.x, currentYPos) && (currentYPos - position.y) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[position.x, currentYPos]);
                currentYPos++;
            }

            currentYPos = position.y - 1;
            while (CeilExistAndNotBusy(position.x, currentYPos) && (position.y - currentYPos) <= maxDistance)
            {
                HighlightedFields.Add(fieldsMatrix[position.x, currentYPos]);
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

        private bool CeilExistAndNotFriendly(int i, int j, SquadType squadType)
        {
            var character = GetCharacterByIndex(new Vector2Int(i, j));
            if (!CeilExist(i, j))
                return false;
            if (character is null)
                return true;
            if (squadType == SquadType.NotMatter)
                return true;
            if (character.SquadType == squadType)
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
}
