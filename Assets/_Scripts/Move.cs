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

        public bool IsMoving { get; private set; }
        public int PositionX => Mathf.CeilToInt(transform.position.x);
        public int PositionY => Mathf.CeilToInt(transform.position.z);
        public Vector2Int Position => new Vector2Int(PositionX, PositionY);
        public int MaxMoveDistance => maxMoveDistance;

        private void Awake()
        {
            animator = GetComponent<CharacterAnimator>();
        }

        public void PrepareToMove()
        {
            if (IsMoving) return;
            IsMoving = true;
        }

        public IEnumerator DoMoveCor(Vector2Int position)
        {
            var positionX = transform.position.x;
            var positionY = transform.position.y;
            var positionZ = transform.position.z;
        
            var distance = Mathf.Sqrt(Mathf.Pow(positionX - position.x, 2) + Mathf.Pow(positionZ - position.y, 2));
            var moveTime = distance / speed;
            yield return StartCoroutine(RotateToPosition(position));
            animator.StartWalk();
            transform.DOMove(new Vector3(position.x, positionY, position.y), moveTime).SetEase(Ease.Linear);
            yield return new WaitForSeconds(moveTime - 0.1f);
            animator.StopWalk();
            IsMoving = false;
        }

        public IEnumerator RotateToPosition(Vector2Int position)
        {
            transform.DOLookAt(new Vector3(position.x, transform.position.y, position.y), rotationTime);
            yield return new WaitForSeconds(rotationTime * 0.8f);
        }
    }
}

