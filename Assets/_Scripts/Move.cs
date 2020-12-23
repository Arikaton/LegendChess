using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Move : MonoBehaviour
{
    public CharacterMoveType moveType;
    [SerializeField] private CharacterAnimationController animator;
    [SerializeField] private int maxMoveDistance;
    [SerializeField] private float rotationTime = 1;
    [SerializeField] private float moveSpeedScaler = 2;
    private bool isMoving = false;

    public Vector2Int Position => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

    public int MaxMoveDistance => maxMoveDistance;

    public void DoMove(Vector2Int position)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(DoMoveCor(position));
        }
    }

    private IEnumerator DoMoveCor(Vector2Int position)
    {
        float speed = Mathf.Sqrt(Mathf.Pow(transform.position.x - position.x, 2) +
                                 Mathf.Pow(transform.position.z - position.y, 2));
        speed /= moveSpeedScaler;
        transform.DOLookAt(new Vector3(position.x, transform.position.y, position.y), rotationTime);
        yield return new WaitForSeconds(rotationTime * 0.8f);
        animator.StartWalkAnimation();
        transform.DOMove(new Vector3(position.x, transform.position.y, position.y), speed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(speed - 0.1f);
        animator.StopMoveAnimation();
        isMoving = false;
    }
}

public enum CharacterMoveType
{
    Straight, 
    Diagonally, 
    AnyCeil
}

