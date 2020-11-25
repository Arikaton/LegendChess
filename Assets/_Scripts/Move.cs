using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public enum MoveType{ straight, diagonally, anyCeil}

    public MoveType moveType;
    [SerializeField] private int maxMoveDistance;
    
    public Vector2Int Position => new Vector2Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

    public int MaxMoveDistance => maxMoveDistance;
}
