using UnityEngine;

public class PlayerStats : MonoBehaviour
{
        public int health = 3;

        public bool hasFallProtection;

        public float fallProtectionForce = 100;
        public float fallProtectionActivator = -150;

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
