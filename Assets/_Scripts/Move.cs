using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterAnimationController animator;
    
    [Header("Settings")]
    public Direction Direction;
    [SerializeField] private int maxMoveDistance;
    [SerializeField] private float rotationTime = 1;
    [SerializeField] private float speed = 2;

    public bool IsMoving { get; private set; }
    public int PositionX => Mathf.CeilToInt(transform.position.x);
    public int PositionY => Mathf.CeilToInt(transform.position.z);
    public int MaxMoveDistance => maxMoveDistance;

    public void DoMove(Vector2Int position)
    {
        if (IsMoving) return;
        IsMoving = true;
        StartCoroutine(DoMoveCor(position));
    }

    private IEnumerator DoMoveCor(Vector2Int position)
    {
        var positionX = transform.position.x;
        var positionY = transform.position.y;
        var positionZ = transform.position.z;
        
        var distance = Mathf.Sqrt(Mathf.Pow(positionX - position.x, 2) + Mathf.Pow(positionZ - position.y, 2));
        var moveTime = distance / speed;
        transform.DOLookAt(new Vector3(position.x, positionY, position.y), rotationTime);
        yield return new WaitForSeconds(rotationTime * 0.8f);
        animator.StartWalk();
        transform.DOMove(new Vector3(position.x, positionY, position.y), moveTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(moveTime - 0.1f);
        animator.StopWalk();
        IsMoving = false;
    }
}

public enum Direction
{
    Straight, 
    Diagonally, 
    Asterisk
}

