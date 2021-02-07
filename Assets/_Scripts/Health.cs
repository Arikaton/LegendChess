using LegendChess.Charactrer;
using UnityEngine;

namespace LegendChess
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int healthPoints;
        private CharacterAnimator characterAnimator;
        private StepHandler stepHandler;

        private void Awake()
        {
            characterAnimator = GetComponent<CharacterAnimator>();
            stepHandler = FindObjectOfType<StepHandler>();
        }

        public void GetDamage(int damage)
        {
            healthPoints -= damage;
            characterAnimator.GetDamage();
            stepHandler.AddDamagedCharacter(this);
        }

        public void GetHealth(int health)
        {
            healthPoints += health;
        }

        public void Death()
        {
            if (healthPoints > 0) return;
            characterAnimator.Death();
            Destroy(gameObject, 1f);
        }
    }
}