using System.Collections;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class SpearAttack : KnehtAttack
    {
        private Vector2Int secondTargetPos;

        protected override IEnumerator MainAttack()
        {
            if (targetsCount == 0) yield break;
            var targetPos = TargetPositions.Dequeue();
            var canAttackFirst = CanAttack(targetPos);
            var canAttackSecond = CanAttack(secondTargetPos);
            if (canAttackFirst || canAttackSecond)
                yield return StartCoroutine(CharacterAnimator.RandomAttackCor());
            else
                yield break;

            yield return StartCoroutine(Move.RotateToPosition(targetPos));
            var firstEnemy = Field.GetGameObjectByIndex<Health>(targetPos);
            if (firstEnemy)
                firstEnemy.GetDamage(damage);

            var secondEnemy = Field.GetGameObjectByIndex<Health>(secondTargetPos);
            if (secondEnemy)
                secondEnemy.GetDamage(damage - 1);
        }

        private bool CanAttack(Vector2Int targetPos)
        {
            var enemyTargetPos = Field.GetSquadTypeByIndex(targetPos);
            if (enemyTargetPos == SquadType.NotMatter) return false;
            return enemyTargetPos != SquadType;
        }

        protected override void HighLightSelectedAttackCells()
        {
            Field.HighlightCell(NextTargetPos);
            Field.HighlightCell(secondTargetPos);
        }
        
        public override void ProcessTapOnCeil(Cell cell, Vector2Int finishMovePos)
        {
            base.ProcessTapOnCeil(cell, finishMovePos);
            CalculateSecondAttackPos(finishMovePos);
        }
        
        private void CalculateSecondAttackPos(Vector2Int finishMovePos)
        {
            var xDistance = finishMovePos.x - NextTargetPos.x;
            var yDistance = finishMovePos.y - NextTargetPos.y;
            var secondPosX = NextTargetPos.x - MathfExtensions.GetSign(xDistance);
            var secondPosY = NextTargetPos.y - MathfExtensions.GetSign(yDistance);
            secondTargetPos = new Vector2Int(secondPosX, secondPosY);
        }
    }
}