using System;
using System.Collections.Generic;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private int horizontalSize;
        [SerializeField] private int verticalSize;
        private List<Cell> HighlightedFields { get; } = new List<Cell>();
        private Cell[,] cells;

        private void Awake()
        {
            DefineCellsMatrix();
        }

        public void HighlightCell(Vector2Int position)
        {
            if (!CellExist(position.x, position.y)) return;
            var cell = cells[position.x, position.y];
            HighlightedFields.Add(cell);
            cell.TurnOn();
        }

        public void HighlightCells(HighlightType highlightType, Vector2Int position, int lenght = 1,
            SquadType squadType = SquadType.NotMatter)
        {
            GetCellsToHighlight(highlightType, position, lenght, squadType);
            HighlightedFields.ForEach(cell => cell.TurnOn());
        }

        public void SetCellBusy(Vector2Int position, GameObject placeHolder, SquadType squadType)
        {
            var cell = cells[position.x, position.y];
            cell.PlaceHolder = placeHolder;
            cell.SquadType = squadType;
        }

        public void SetCellFree(Vector2Int position)
        {
            var cell = cells[position.x, position.y];
            cell.PlaceHolder = null;
            cell.SquadType = SquadType.NotMatter;

        }

        public bool IsCellHaveCharacter(Vector2Int index) => cells[index.x, index.y].IsBusy;
        
        public GameObject GetGameObjectByIndex(Vector2Int index) => cells[index.x, index.y].PlaceHolder;
        
        public T GetGameObjectByIndex<T>(Vector2Int index) => cells[index.x, index.y].PlaceHolder.GetComponent<T>();

        public SquadType GetSquadTypeByIndex(Vector2Int index) => cells[index.x, index.y].SquadType;

        public void TurnOffCells()
        {
            HighlightedFields.ForEach(field => field.TurnOff());
            HighlightedFields.Clear();
        }

        private void DefineCellsMatrix()
        {
            var cellsArray = GetComponentsInChildren<Cell>();
            cells = new Cell[horizontalSize, verticalSize];
            foreach (var cell in cellsArray)
            {
                if (cell.Position.x >= horizontalSize || cell.Position.y >= verticalSize)
                {
                    cell.gameObject.SetActive(false);
                    continue;
                }
                cells[cell.PositionX, cell.PositionY] = cell;
            }
        }

        private void GetCellsToHighlight(HighlightType highlightType, Vector2Int position, int maxDistance, 
            SquadType squadType)
        {
            switch (highlightType)
            {
                case HighlightType.Straight:
                    StraightPass(maxDistance, position, squadType);
                    AddCellToHighlighted(position);
                    break;
                case HighlightType.Diagonally:
                    DiagonallyPass(maxDistance, position, squadType);
                    AddCellToHighlighted(position);
                    break;
                case HighlightType.Asterisk:
                    StraightPass(maxDistance, position, squadType);
                    DiagonallyPass(maxDistance, position, squadType);
                    AddCellToHighlighted(position);
                    break;
                case HighlightType.Any:
                    SimplePass(maxDistance, position);
                    break;
                case HighlightType.AnyExceptMiddle:
                    SimplePass(maxDistance, position);
                    break;
                case HighlightType.AnyExceptFriendly:
                    SimplePass(maxDistance, position, false, squadType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(highlightType), highlightType, null);
            }
        }

        private void AddCellToHighlighted(Vector2Int position)
        {
            HighlightedFields.Add(cells[position.x, position.y]);
        }
        
        private void SimplePass(int maxDistance, Vector2Int position, bool addMiddle = false, SquadType squadType = SquadType.NotMatter)
        {
            for (int i = position.x - maxDistance; i <= position.x + maxDistance; i++)
            {
                for (int j = position.y - maxDistance; j <= position.y + maxDistance; j++)
                {
                    if (i == position.x && j == position.y && !addMiddle)
                        continue;
                    if (squadType != SquadType.NotMatter)
                    {
                        if (CellNotSameSquad(i, j, squadType))
                            HighlightedFields.Add(cells[i, j]);
                    }
                    else
                    {
                        if (CellExist(i, j))
                            HighlightedFields.Add(cells[i, j]);
                    }
                }
            }
        }
        
        private void DiagonallyPass(int maxDistance, Vector2Int position, SquadType squadType)
        {
            var currentX = position.x + 1;
            var currentY = position.y + 1;
            while (CellNotSameSquad(currentX, currentY, squadType) && (currentX - position.x) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentX, currentY]);
                currentX++;
                currentY++;
            }

            currentX = position.x - 1;
            currentY = position.y + 1;
            while (CellNotSameSquad(currentX, currentY, squadType) && (position.x - currentX) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentX, currentY]);
                currentX--;
                currentY++;
            }

            currentX = position.x + 1;
            currentY = position.y - 1;
            while (CellNotSameSquad(currentX, currentY, squadType) && (currentX - position.x) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentX, currentY]);
                currentX++;
                currentY--;
            }

            currentX = position.x - 1;
            currentY = position.y - 1;
            while (CellNotSameSquad(currentX, currentY, squadType) && (position.x - currentX) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentX, currentY]);
                currentX--;
                currentY--;
            }
        }
        
        private void StraightPass(int maxDistance, Vector2Int position, SquadType squadType)
        {
            var currentXPos = position.x + 1;
            while (CellNotSameSquad(currentXPos, position.y, squadType) && (currentXPos - position.x) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentXPos, position.y]);
                currentXPos++;
            }

            currentXPos = position.x - 1;
            while (CellNotSameSquad(currentXPos, position.y, squadType) && (position.x - currentXPos) <= maxDistance)
            {
                HighlightedFields.Add(cells[currentXPos, position.y]);
                currentXPos--;
            }

            var currentYPos = position.y + 1;
            while (CellNotSameSquad(position.x, currentYPos, squadType) && (currentYPos - position.y) <= maxDistance)
            {
                HighlightedFields.Add(cells[position.x, currentYPos]);
                currentYPos++;
            }

            currentYPos = position.y - 1;
            while (CellNotSameSquad(position.x, currentYPos, squadType) && (position.y - currentYPos) <= maxDistance)
            {
                HighlightedFields.Add(cells[position.x, currentYPos]);
                currentYPos--;
            }
        }
        
        public bool CellNotBusy(int i, int j)
        {
            if (!CellExist(i, j))
                return false;
            return !IsCellHaveCharacter(new Vector2Int(i, j));
        }
        
        public bool CellNotSameSquad(int i, int j, SquadType squadType)
        {
            if (!CellExist(i, j))
                return false;
            var otherSquadType = GetSquadTypeByIndex(new Vector2Int(i, j));
            return otherSquadType != squadType;
        }
        
        public bool CellExist(int i, int j)
        {
            if (i >= horizontalSize || i < 0)
                return false;
            if (j >= verticalSize || j < 0)
                return false;
            return true;
        }
    }
}
