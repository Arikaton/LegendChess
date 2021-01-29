using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class ArcherAttack : KnehtAttack
    {
        [SerializeField] private int maxAttackDistance = 3;
        protected override void HighlightPossibleAttackCells(Vector2Int endMovePos)
        {
            Character.Field.TurnOnFields(endMovePos, highlightType, maxAttackDistance);
        }

        protected override void HighLightSelectedAttackCells()
        {
            base.HighLightSelectedAttackCells();
        }
    }
}