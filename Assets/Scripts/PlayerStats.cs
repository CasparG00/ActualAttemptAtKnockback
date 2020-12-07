using UnityEngine;

public class PlayerStats : MonoBehaviour
{
        [HideInInspector]
        public int health;
        public int maxHealth = 3;

        public bool hasFallProtection;

        public float fallProtectionForce = 100;
        public float fallProtectionActivator = -150;

        private void Start()
        {
                health = maxHealth;
        }

        public void Damage(int amount)
        {
                if (health > 0)
                {
                        health -= amount;
                }
        }

        public void Heal(int amount)
        {
                health += amount;
        }
}
