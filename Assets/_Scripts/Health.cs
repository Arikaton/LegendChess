using UnityEngine;

namespace _Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationController characterAnimationController;
        [SerializeField] private int healthPoints;

        public void GetDamage(int damage)
        {
            characterAnimationController.GetDamage();
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
            characterAnimationController.Death();
            Destroy(gameObject, 1f);
        }
    }
}