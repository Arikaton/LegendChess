using System.Collections;
using LegendChess.CharacterAttack;
using LegendChess.Contracts;
using LegendChess.Enums;
using UnityEngine;

namespace LegendChess.Charactrer
{
    [RequireComponent(typeof(BaseAttack), typeof(Health), typeof(Move))]
    public class Character : MonoBehaviour, IInteractable
    {
        public static Character ActiveCharacter { get; private set; }

        [SerializeField] private SquadType squadType;
        [SerializeField] private GameObject moveSphere = null;
        private GameManager gameManager;
        private BaseAttack attack;

        public SquadType SquadType => squadType;
        public Field Field { get; private set; }
        public Health Health { get; private set; }
        public CharacterAnimator CharacterAnimator { get; private set; }
        public Move Move { get; private set; }
        public Vector2Int? EndMovePosition { get; private set; }

        private bool isMoving = false;
        
        private void Awake() => GetReferences();
        private void Start()
        {
            Field.SetCeilBusy(Move.Position, this);
        }

        private void GetReferences()
        {
            Field = FindObjectOfType<Field>();
            gameManager = FindObjectOfType<GameManager>();
            attack = GetComponent<BaseAttack>();
            Move = GetComponent<Move>();
            Health = GetComponent<Health>();
            CharacterAnimator = GetComponent<CharacterAnimator>();
        }

        public IEnumerator DoMove()
        {
            isMoving = true;
            HideVisual();
            yield return StartCoroutine(Move.DoMoveCor(EndMovePosition.Value));
            for (int i = 0; i < attack.TargetsCount; i++)
            {
                yield return StartCoroutine(Move.RotateToPosition(attack.NextTargetPos));
                yield return StartCoroutine(attack.DoAttack());
            }
            attack.Reset();
            EndMovePosition = null;
            isMoving = false;
        }

        public void OnInteract()
        {
            if (isMoving) return;
            ActiveCharacter?.HideVisual();
            ActiveCharacter = this;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            Field.TurnOffFields();
            if (EndMovePosition == null)
            {
                HighlightMove();
            }
            else
            {
                ShowEndMovePos();
                attack.ShowVisual(EndMovePosition.Value);
            }
        }

        private void HighlightMove()
        {
            Field.TurnOnFields(Move.Position, Move.highlightType, Move.MaxMoveDistance);
        }

        private void HideVisual()
        {
            moveSphere.SetActive(false);
            attack.HideAttack();
        }

        private void ShowEndMovePos()
        {
            moveSphere.SetActive(true);
            moveSphere.transform.position = new Vector3(EndMovePosition.Value.x, transform.position.y, EndMovePosition.Value.y);
        }

        public void OnTapOnCeil(Ceil ceil)
        {
            if (isMoving) return;
            if (ceil is null)
            {
                Field.TurnOffFields();
                HideVisual();
                ActiveCharacter = null;
                return;
            }
            if (EndMovePosition == null)
            {
                Field.SetCeilFree(Move.Position);
                Field.SetCeilBusy(ceil.Position, this);
                EndMovePosition = ceil.Position;
                gameManager.AddCharacterToQueue(this);
            }
            else
            {
                attack.ProcessTapOnCeil(ceil);
            }
            UpdateVisual();
        }
    }
}
