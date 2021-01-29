using System;
using System.Collections;
using _Scripts.Enums;
using UnityEngine;

namespace _Scripts
{
    public class KnehtAttack : BaseAttack
    {
        private void Start()
        {
            highlightType = HighlightType.AnyExceptMiddle;
        }
        
        public override IEnumerator DoAttack()
        {
            yield return StartCoroutine(Character.CharAnimController.RandomAttackCor());
            var character = Character.Field.GetCharacterByIndex(targetPositions.Dequeue());
            if (character != null && character.SquadType != Character.SquadType)
            {
                character.Health.GetDamage(damage);
            }
        }

        public override void HighlightAttack(Vector2Int endMovePos)
        {
            if (!IsComplete)
                Character.Field.TurnOnFields(endMovePos, highlightType);
            else
            {
                Character.Field.HighlightCeil(NextTargetPos);
            }
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