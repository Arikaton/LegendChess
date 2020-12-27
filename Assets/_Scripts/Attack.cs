using UnityEngine;

namespace _Scripts
{
    public class Attack : MonoBehaviour
    {
        public int Damage => damage;
        public int AttackCeilCount => attackCeilCount;
        [SerializeField] private int damage;
        [SerializeField] private int attackCeilCount = 1;
    }
}