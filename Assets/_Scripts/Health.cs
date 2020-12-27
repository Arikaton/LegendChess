using UnityEngine;

namespace _Scripts
{
    public class Health : MonoBehaviour
    {
        public int HealthPointPoints => healthPoints;
        [SerializeField] private int healthPoints;

        public void GetDamage(int damage)
        {
            healthPoints -= damage;
        }

        public void GetHealth(int health)
        {
            healthPoints += health;
        }
    }
}