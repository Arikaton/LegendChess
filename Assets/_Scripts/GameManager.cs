using System.Collections;
using System.Collections.Generic;
using LegendChess.Charactrer;
using UnityEngine;

namespace LegendChess
{
    public class GameManager: MonoBehaviour
    {
        public bool IsFull => charactersToMove.Count == maxCharacterPerMove;
        
        private Queue<Character> charactersToMove = new Queue<Character>();
        [SerializeField] private int maxCharacterPerMove = 1;

        public void AddCharacterToQueue(Character character)
        {
            if (IsFull) return;
            charactersToMove.Enqueue(character);
        }

        public void DoMove()
        {
            StartCoroutine(DoMoveCor());
        }

        public IEnumerator DoMoveCor()
        {
            while (charactersToMove.Count > 0)
            {
                var character = charactersToMove.Dequeue();
                yield return StartCoroutine(character.DoMove());
            }
        }
    }
}