using System.Collections;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class KnehtAttack : BaseAttack
    {
        protected override IEnumerator MainAttack()
        {
            if (TargetPositions.Count == 0) yield break;
            while (TargetPositions.Count > 0)
            {
                var targetPos = TargetPositions.Dequeue();
                var targetSquadType = Field.GetSquadTypeByIndex(targetPos);
                if (targetSquadType == SquadType.NotMatter) continue;
                if (targetSquadType == SquadType) continue;
                yield return StartCoroutine(Move.RotateToPosition(targetPos));
                yield return StartCoroutine(CharacterAnimator.RandomAttackCor());
                var enemyHealth = Field.GetGameObjectByIndex<Health>(targetPos);
                enemyHealth.GetDamage(damage);
                yield return new WaitForSeconds(0.5f);
            }
        }

        protected override void HighlightPossibleAttackCells(Vector2Int endMovePos)
        {
            Field.HighlightCells(highlightType, endMovePos, 1, SquadType);
        }

        protected override void HighLightSelectedAttackCells()
        {
            Field.HighlightCell(NextTargetPos);
        }

        public override void HideAttack()
        {
            Field.TurnOffCells();
        }

        public override void ProcessTapOnCeil(Cell cell, Vector2Int finishMovePos)
        {
            if (IsComplete) return;
            AddTarget(cell.Position);
        }

        protected override void Reset()
        {
            TargetPositions.Clear();
            CollisionEnemyHealth = null;
        }
    }
}