using System.Collections;
using System.Collections.Generic;
using LegendChess.Charactrer;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.CharacterAttack
{
    public abstract class BaseAttack : MonoBehaviour
    {
        public bool IsComplete => targetPositions.Count == targetsCount;
        public int TargetsCount => targetsCount;
        public Vector2Int NextTargetPos => targetPositions.Peek();

        [SerializeField] protected int damage;
        [SerializeField] protected int targetsCount;
        
        protected Queue<Vector2Int> targetPositions = new Queue<Vector2Int>();
        protected HighlightType highlightType;

        private Character character;
        protected Character Character => character ?? GetComponent<Character>();

        public abstract IEnumerator DoAttack();

        public void AddTarget(Vector2Int position)
        {
            if (targetPositions.Count < targetsCount)
                targetPositions.Enqueue(position);
        }

        protected abstract void HighlightPossibleAttackCells(Vector2Int endMovePos);
        protected abstract void HighLightSelectedAttackCells();
        public abstract void HideAttack();

        public void ShowVisual(Vector2Int endMovePos)
        {
            if (IsComplete)
                HighLightSelectedAttackCells();
            else
                HighlightPossibleAttackCells(endMovePos);
        }

        public abstract void ProcessTapOnCeil(Ceil ceil);

        public abstract void Reset();
    }
}