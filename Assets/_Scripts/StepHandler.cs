using System;
using System.Collections;
using System.Collections.Generic;
using LegendChess.Charactrer;
using LegendChess.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LegendChess
{
    public class StepHandler: MonoBehaviour
    {
        [SerializeField] private int maxCharacterPerMove = 1;
        private List<Character> characters = new List<Character>();
        private List<Health> damagedCharacters = new List<Health>();
        public bool IsFull => characters.Count == maxCharacterPerMove;

        public void AddCharacter(Character character) => characters.Add(character);
        public void AddDamagedCharacter(Health health) => damagedCharacters.Add(health);

        public IEnumerator StepCor()
        {
            yield return StartCoroutine(MoveCor());
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(AttackCor());
            damagedCharacters.ForEach(h => h.Death());
            Reset();
        }

        private IEnumerator MoveCor()
        {
            foreach (var character in characters)
            {
                character.Move().ParallelCoroutinesGroup(character, "MoveCor");
            }
            yield return new WaitWhile(() => CoroutineExtension.GroupProcessing("MoveCor"));
        }

        private IEnumerator AttackCor()
        {
            foreach (var character in characters)
            {
                yield return StartCoroutine(character.Attack());
                yield return new WaitForSeconds(1f);
            }
        }

        private void Reset()
        {
            characters = new List<Character>();
            damagedCharacters = new List<Health>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StepCor());
            }
        }
    }
}