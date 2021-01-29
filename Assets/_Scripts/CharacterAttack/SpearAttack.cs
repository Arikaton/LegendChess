using System.Collections;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class SpearAttack : KnehtAttack
    {
        private Vector2Int secondTargetPos;
        public override IEnumerator DoAttack()
        {
            yield return StartCoroutine(Character.CharacterAnimator.RandomAttackCor());
            var firstCharacter = Character.Field.GetCharacterByIndex(targetPositions.Dequeue());
            if (firstCharacter != null && firstCharacter.SquadType != Character.SquadType)
            {
                firstCharacter.Health.GetDamage(damage);
            }

            var secondCharacter = Character.Field.GetCharacterByIndex(secondTargetPos);
            if (secondCharacter != null && secondCharacter.SquadType != Character.SquadType)
            {
                secondCharacter.Health.GetDamage(damage - 1);
            }
        }

        protected override void HighLightSelectedAttackCells()
        {
            Character.Field.HighlightCeil(NextTargetPos);
            Character.Field.HighlightCeil(secondTargetPos);
        }

        public override void ProcessTapOnCeil(Ceil ceil)
        {
            base.ProcessTapOnCeil(ceil);
            CalculateSecondAttackPos();
        }

        private void CalculateSecondAttackPos()
        {
            var xDistance = Character.EndMovePosition.Value.x - NextTargetPos.x;
            var yDistance = Character.EndMovePosition.Value.y - NextTargetPos.y;
            var secondPosX = NextTargetPos.x - GetSign(xDistance);
            var secondPosY = NextTargetPos.y - GetSign(yDistance);
            secondTargetPos = new Vector2Int(secondPosX, secondPosY);
        }

        private int GetSign(int value)
        {
            if (value == 0)
                return 0;
            return value > 0 ? 1 : -1;
        }
        
        
    }
}