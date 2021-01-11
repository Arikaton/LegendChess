using System.Collections;
using UnityEngine;

namespace _Scripts
{
    public class Attack : MonoBehaviour
    {
        public int Damage => damage;
        public int AttackCeilCount => attackCeilCount;
        [SerializeField] private CharacterAnimationController characterAnimationController;
        [SerializeField] private int damage;
        [SerializeField] private int attackCeilCount = 1;

        public IEnumerator DoAttackCor(Character characrter)
        {
            characterAnimationController.RandomAttack();
            yield return new WaitForSeconds(0.5f);
            characrter.GetComponent<Health>().GetDamage(Damage);
        }
    }
}