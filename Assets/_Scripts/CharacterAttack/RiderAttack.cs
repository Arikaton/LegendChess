using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class RiderAttack : KnehtAttack
    {
        private Vector2Int secondTargetPos;

        protected override void HighLightSelectedAttackCells()
        {
            foreach (var target in TargetPositions.ToArray())
            {
                Field.HighlightCell(target);
            }
        }

        protected override void HighlightPossibleAttackCells(Vector2Int endMovePos)
        {
            if (TargetPositions.Count == 0)
            {
                Field.HighlightCells(HighlightType.AnyExceptMiddle, endMovePos);
            }
            else
            {
                HighlightSecondTargetPos(endMovePos);
            }
        }

        private void HighlightSecondTargetPos(Vector2Int endMovePos)
        {
            for (int i = endMovePos.x - 1; i <= endMovePos.x + 1; i++)
            {
                for (int j = endMovePos.y - 1; j <= endMovePos.y + 1; j++)
                {
                    var point = new Vector2Int(i, j);
                    if (GetDistance(NextTargetPos, point) < 2 &&
                        endMovePos != point)
                        Field.HighlightCell(point);
                }
            }
        }

        private int GetDistance(Vector2Int firstPoint, Vector2Int secondPoint)
        {
            return Mathf.Abs(firstPoint.x - secondPoint.x) + Mathf.Abs(firstPoint.y - secondPoint.y);
        }
    }
}