using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
   public static MoveManager Instance { get; private set; }
   
   private Queue<Character> moveQueue = new Queue<Character>();
   private Queue<Character> attackQueue = new Queue<Character>();
   private bool isMoveProcess = false;

   private void Awake()
   {
      Instance = this;
   }

   public void DoGlobalMove()
   {
      if (isMoveProcess)
         return;
      StartCoroutine(DoGlobalMoveCor());
   }

   private IEnumerator DoGlobalMoveCor()
   {
      isMoveProcess = true;
      yield return StartCoroutine(MoveCor());
      yield return StartCoroutine(AttackCor());
      isMoveProcess = false;
   }

   public void AddToMoveQueue(Character character)
   {
      moveQueue.Enqueue(character);
   }
   
   public void AddToAttackQueue(Character character)
   {
      attackQueue.Enqueue(character);
   }
   
   private IEnumerator MoveCor()
   {
      while (moveQueue.Count > 0)
      {
         var character = moveQueue.Dequeue();
         yield return StartCoroutine(character.MoveProcess());
      }
   }

   private IEnumerator AttackCor()
   {
      while (attackQueue.Count > 0)
      {
         var character = attackQueue.Dequeue();
         yield return StartCoroutine(character.AttackProcess());
      }
   }
}
