using System.Collections;
using System.Collections.Generic;
using LegendChess.Charactrer;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public abstract class BaseAttack : MonoBehaviour
    {
        public bool IsComplete => TargetPositions.Count == targetsCount;
        protected Vector2Int NextTargetPos => TargetPositions.Count == 0 ? Vector2Int.zero : TargetPositions.Peek();

        [SerializeField] protected int targetsCount;
        [SerializeField] protected int damage;
        [SerializeField] protected HighlightType highlightType;

        protected SquadType SquadType;
        protected readonly Queue<Vector2Int> TargetPositions = new Queue<Vector2Int>();
        protected CharacterAnimator CharacterAnimator;
        protected Field Field;
        protected Health CollisionEnemyHealth;
        protected Move Move;

        protected virtual void Start()
        {
            CharacterAnimator = GetComponent<CharacterAnimator>();
            Field = FindObjectOfType<Field>();
            Move = GetComponent<Move>();
        }

        public void SetSquadType(SquadType st)
        {
            SquadType = st;
        }

        public IEnumerator Attack()
        {
            if (CollisionEnemyHealth is null)
            {
                if (targetsCount == 0)
                    yield break;
                yield return StartCoroutine(MainAttack());
            }
            else
                yield return StartCoroutine(CollisionAttack());
            Reset();
        }

        protected abstract IEnumerator MainAttack();

        public IEnumerator CollisionAttack()
        {
            yield return StartCoroutine(Move.RotateToPosition(CollisionEnemyHealth.GetComponent<Move>().Position));
            yield return StartCoroutine(CharacterAnimator.RandomAttackCor());
            CollisionEnemyHealth.GetDamage(damage);
        }

        public void AddTarget(Vector2Int position)
        {
            if (TargetPositions.Count < targetsCount)
                TargetPositions.Enqueue(position);
        }

        public void AddCollisionTarget(Health enemyHealth)
        {
            CollisionEnemyHealth ??= enemyHealth;
        }

        public void ShowVisual(Vector2Int endMovePos)
        {
            if (IsComplete)
                HighLightSelectedAttackCells();
            else
                HighlightPossibleAttackCells(endMovePos);
        }

        public abstract void HideAttack();
        public abstract void ProcessTapOnCeil(Cell cell, Vector2Int finishMovePos);
        protected abstract void HighlightPossibleAttackCells(Vector2Int endMovePos);
        protected abstract void HighLightSelectedAttackCells();
        protected abstract void Reset();
    }
}