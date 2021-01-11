using System;
using System.Collections;
using _Scripts;
using _Scripts.Charactrer;
using _Scripts.Enums;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static Character activeCharacter;

    public RelationType relationType;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Field field;
    [SerializeField] private Move move;
    [SerializeField] private Health health;
    [SerializeField] private Attack attack;

    private bool moveEndPointChoosed = false;
    private bool attackEndPointChoosed = false;
    public Vector2Int MoveEndPoint { get; private set; }
    public Vector2Int AttackEndPoint { get; private set; }

    public IEnumerator MoveProcess()
    {
        move.PrepareToMove();
        yield return StartCoroutine(move.DoMoveCor(MoveEndPoint));
        moveEndPointChoosed = false;
    }

    public IEnumerator AttackProcess()
    {
        yield return StartCoroutine(move.RotateToPosition(AttackEndPoint));
        
        var aimCharacter = field.GetCharacterByIndex(AttackEndPoint);
        if (aimCharacter != null && relationType != aimCharacter.relationType)
            yield return StartCoroutine(attack.DoAttackCor(aimCharacter));
        attackEndPointChoosed = false;
    }

    private void Awake()
    {
        playerInput.OnClickOnCharacter += OnClickOnCharacter;
        playerInput.OnEmptyClick += OnClickEmpty;
        playerInput.OnClickOnCeil += OnClickOnCeil;
    }

    private void Start()
    {
        field.SetCeilBusy(move.Position, this);
    }

    private void OnClickOnCharacter(Character character)
    {
        if (character != this) return;
        activeCharacter = this;
        ChooseAction();
    }

    private void ChooseAction()
    {
        if (move.IsMoving) return;
        field.TurnOffFields();
        if (moveEndPointChoosed)
        {
            field.HighlightCeilAndShowEffect(MoveEndPoint, EffectType.Move);
            if (attackEndPointChoosed)
            {
                field.HighlightCeilAndShowEffect(AttackEndPoint, EffectType.Attack);
            }
            else
            {
                field.TurnOnFields(attack.AttackCeilCount, MoveEndPoint, MoveDirection.Any);
            }
        }
        else
        {
            field.TurnOnFields(move.MaxMoveDistance, new Vector2Int(move.PositionX, move.PositionY), move.moveDirection);
        }
    }

    private void OnClickOnCeil(Ceil ceil)
    {
        if (activeCharacter is null || activeCharacter != this)
            return;
        if (ceil.IsHighlighted)
        {
            if (attackEndPointChoosed)
                return;
            if (moveEndPointChoosed)
            {
                if (ceil.Position != MoveEndPoint)
                {
                    AttackEndPoint = ceil.Position;
                    attackEndPointChoosed = true;
                    MoveManager.Instance.AddToAttackQueue(this);
                }
            }
            else
            {
                field.SetCeilBusy(ceil.Position, this);
                field.SetCeilFree(move.Position);
                MoveEndPoint = new Vector2Int(ceil.PositionX, ceil.PositionY);
                MoveManager.Instance.AddToMoveQueue(this);
                moveEndPointChoosed = true;
            }
        }
        field.TurnOffFields();
        activeCharacter = null;
    }
    
    private void OnClickEmpty()
    {
        field.TurnOffFields();
        activeCharacter = null;
    }
}
