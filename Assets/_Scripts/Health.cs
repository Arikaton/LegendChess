using System;
using LegendChess.Charactrer;
using UnityEngine;

namespace LegendChess
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int healthPoints;
        private CharacterAnimator characterAnimator;

        private void Awake()
        {
            characterAnimator = GetComponent<CharacterAnimator>();
        }

        public void GetDamage(int damage)
        {
            characterAnimator.GetDamage();
            healthPoints -= damage;
            if (healthPoints <= 0)
            {
                Death();
            }
        }

        public void GetHealth(int health)
        {
            healthPoints += health;
        }

        public void Death()
        {
            characterAnimator.Death();
            Destroy(gameObject, 1f);
        }
    }
}