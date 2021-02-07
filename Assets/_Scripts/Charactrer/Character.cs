using System.Collections;
using LegendChess.CharacterAttack;
using LegendChess.Contracts;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.Charactrer
{
    [RequireComponent(typeof(BaseAttack), typeof(Health), typeof(Move))]
    public class Character : MonoBehaviour, IInteractible
    {
        public static Character ActiveCharacter { get; private set; }

        [SerializeField] private SquadType squadType;
        [SerializeField] private GameObject moveVFX;
        private StepHandler stepHandler;
        private BaseAttack attack;

        private Field field;
        private Health health;
        private Move move;
        public SquadType SquadType => squadType;
        public Vector2Int? FinishMovePosition { get; set; }

        private void Awake() => GetReferences();
        
        private void Start()
        {
            field.SetCellBusy(move.Position, gameObject, squadType);
        }

        public IEnumerator Move()
        {
            HideVisual();
            yield return StartCoroutine(move.MoveAction(FinishMovePosition.Value, this));
            FinishMovePosition = null;
        }

        public IEnumerator Attack()
        {
            yield return StartCoroutine(attack.Attack());
        }

        public void OnCollision(Vector2Int nextStep)
        {
            var otherCharacter = field.GetGameObjectByIndex<Character>(nextStep);
            if (otherCharacter is null) return;
            if (otherCharacter.SquadType == squadType) return;
            attack.AddCollisionTarget(otherCharacter.health);
            otherCharacter.attack.AddCollisionTarget(health);
            otherCharacter.Stop();
        }

        public void OnInteract(SquadType interactorSquadType)
        {
            if (squadType != interactorSquadType) return;
            ActiveCharacter?.HideVisual();
            ActiveCharacter = this;
            UpdateVisual();
        }

        public void OnTapOnCeil(Cell cell)
        {
            if (cell is null)
            {
                Reset();
                return;
            }
            if (FinishMovePosition == null)
            {
                if (stepHandler.IsFull) return;
                FinishMovePosition = cell.Position;
                stepHandler.AddCharacter(this);
            }
            else
            {
                attack.ProcessTapOnCeil(cell);
            }
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            field.TurnOffCells();
            if (FinishMovePosition == null)
            {
                move.HighlightPossible(squadType);
            }
            else
            {
                ShowEndMovePos();
                attack.ShowVisual(FinishMovePosition.Value);
            }
        }

        public void HideVisual()
        {
            moveVFX.SetActive(false);
            attack.HideAttack();
            field.TurnOffCells();
        }

        private void Stop()
        {
            move.Stop();
        }

        private void OnDestroy()
        {
            field.SetCellFree(move.Position);
        }

        private void ShowEndMovePos()
        {
            moveVFX.SetActive(true);
            moveVFX.transform.position = new Vector3(FinishMovePosition.Value.x, transform.position.y, FinishMovePosition.Value.y);
        }

        private void GetReferences()
        {
            field = FindObjectOfType<Field>();
            stepHandler = FindObjectOfType<StepHandler>();
            attack = GetComponent<BaseAttack>();
            move = GetComponent<Move>();
            health = GetComponent<Health>();
            attack.SetSquadType(squadType);
        }

        private void Reset()
        {
            HideVisual();
            ActiveCharacter = null;
        }
    }
}
