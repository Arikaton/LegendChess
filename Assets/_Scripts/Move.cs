using System;
using System.Collections;
using DG.Tweening;
using LegendChess.Charactrer;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess
{
    public class Move : MonoBehaviour
    {
        public HighlightType highlightType;
        [SerializeField] private int maxMoveDistance;
        [SerializeField] private float rotationTime = 1;
        [SerializeField] private float speed = 2;
        private CharacterAnimator animator;
        private Field field;
        private int PositionY => Mathf.CeilToInt(transform.position.z);
        private int PositionX => Mathf.CeilToInt(transform.position.x);
        public bool IsMoving { get; private set; }
        public Vector2Int Position => new Vector2Int(PositionX, PositionY);

        private void Awake()
        {
            animator = GetComponent<CharacterAnimator>();
            field = FindObjectOfType<Field>();
        }

        public void HighlightPossible(SquadType squadType)
        {
            field.HighlightCells(highlightType, Position, maxMoveDistance, squadType);
        }
        
        public void Stop()
        {
            IsMoving = false;
        }

        public IEnumerator MoveAction(Vector2Int position, Character character)
        {
            if (position == Position)
            {
                field.SetCellBusy(Position, gameObject, character.SquadType);
                yield break;
            }
            IsMoving = true;
            yield return StartCoroutine(RotateToPosition(position));
            animator.StartWalk();
            yield return StartCoroutine(MoveToPosition(position, character));
            animator.StopWalk();
            IsMoving = false;
        }

        private IEnumerator MoveToPosition(Vector2Int position, Character character)
        {
            var distance = GetDistanceTo(position);
            var direction = GetDirection(position);
            var stepsCount = GetStepsCount(position);
            var moveTime = distance / (speed * stepsCount);
            
            for (var i = 0; i < stepsCount; i++)
            {
                var nextStepVector = new Vector2Int(PositionX + direction.x, PositionY + direction.y);
                var nextStepPos = new Vector3(nextStepVector.x, transform.position.y, nextStepVector.y);
                if (NextStepIsBusy(nextStepVector))
                {
                    character.OnCollision(nextStepVector);
                    Stop();
                    yield break;
                }
                field.SetCellFree(Position);
                field.SetCellBusy(nextStepVector, gameObject, character.SquadType);
                var moveTween = 
                    transform
                        .DOMove(nextStepPos, moveTime)
                        .SetEase(Ease.Linear);
                yield return moveTween.WaitForCompletion();
                if (!IsMoving) yield break;
            }
        }

        private int GetStepsCount(Vector2Int position)
        {
            var xStepCount = Mathf.Abs(position.x - PositionX);
            return xStepCount != 0 ? xStepCount : Mathf.Abs(position.y - PositionY);
        }

        private bool NextStepIsBusy(Vector2Int nextStep)
        {
            return !field.CellNotBusy(nextStep.x, nextStep.y);
        }

        private Vector2Int GetDirection(Vector2Int position) =>
            new Vector2Int(
                MathfExtensions.GetSign(position.x - PositionX), 
                MathfExtensions.GetSign(position.y - PositionY));

        private float GetDistanceTo(Vector2Int destination)
        {
            var position = transform.position;
            return Mathf.Sqrt(Mathf.Pow(position.x - destination.x, 2) + 
                              Mathf.Pow(position.z - destination.y, 2));
        }

        public IEnumerator RotateToPosition(Vector2Int position)
        {
            var rotationPos = new Vector3(position.x, transform.position.y, position.y);
            var rotateTween = transform
                .DOLookAt(rotationPos, rotationTime);
            yield return rotateTween.WaitForCompletion();
        }
    }
}

