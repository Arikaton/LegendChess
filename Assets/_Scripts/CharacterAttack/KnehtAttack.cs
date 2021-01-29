using System.Collections;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public class KnehtAttack : BaseAttack
    {
        private void Start()
        {
            highlightType = HighlightType.AnyExceptMiddle;
        }
        
        public override IEnumerator DoAttack()
        {
            var character = Character.Field.GetCharacterByIndex(targetPositions.Dequeue());
            if (character != null && character.SquadType != Character.SquadType)
            {
                yield return StartCoroutine(Character.CharacterAnimator.RandomAttackCor());
                character.Health.GetDamage(damage);
            }
        }

        protected override void HighlightPossibleAttackCells(Vector2Int endMovePos)
        {
            Character.Field.TurnOnFields(endMovePos, highlightType);
        }

        protected override void HighLightSelectedAttackCells()
        {
            Character.Field.HighlightCeil(NextTargetPos);
        }

        public override void HideAttack()
        {
            Character.Field.TurnOffFields();
        }

        public override void ProcessTapOnCeil(Ceil ceil)
        {
            if (IsComplete) return;
            AddTarget(ceil.Position);
        }

        public override void Reset()
        {
            targetPositions.Clear();
        }
    }
}