using System.Collections;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class SpearAttack : KnehtAttack
    {
        private Vector2Int secondTargetPos;

        // protected override IEnumerator MainAttack()
        // {
        //     if (TargetPositions.Count == 0) yield break;
        //     yield return StartCoroutine(CharacterAnimator.RandomAttackCor());
        //     var firstCharacter = Field.GetCharacterByIndex(TargetPositions.Dequeue());
        //     if (firstCharacter != null && firstCharacter.SquadType != Character.SquadType)
        //     {
        //         firstCharacter.ProvideDamage(damage);
        //     }
        //     
        //     var secondCharacter = field.GetCharacterByIndex(secondTargetPos);
        //     if (secondCharacter != null && secondCharacter.SquadType != Character.SquadType)
        //     {
        //         secondCharacter.ProvideDamage(damage - 1);
        //     }
        // }
        //
        // protected override void HighLightSelectedAttackCells()
        // {
        //     Field.HighlightCell(NextTargetPos);
        //     Field.HighlightCell(secondTargetPos);
        // }
        //
        // public override void ProcessTapOnCeil(Cell cell)
        // {
        //     base.ProcessTapOnCeil(cell);
        //     CalculateSecondAttackPos();
        // }
        //
        // private void CalculateSecondAttackPos()
        // {
        //     var xDistance = Character.FinishMovePosition.Value.x - NextTargetPos.x;
        //     var yDistance = Character.FinishMovePosition.Value.y - NextTargetPos.y;
        //     var secondPosX = NextTargetPos.x - MathfExtensions.GetSign(xDistance);
        //     var secondPosY = NextTargetPos.y - MathfExtensions.GetSign(yDistance);
        //     secondTargetPos = new Vector2Int(secondPosX, secondPosY);
        // }
    }
}