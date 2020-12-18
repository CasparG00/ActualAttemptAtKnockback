using UnityEngine;

public class PlayerStats : MonoBehaviour
{
        [HideInInspector]
        public int health;
        public int maxHealth = 3;

        [HideInInspector]
        public bool hasFallProtection;

        public float fallProtectionForce = 100;
        public float fallProtectionActivator = -150;

        [HideInInspector] 
        public bool hasSecondChance;
        
        [HideInInspector] 
        public bool hasOvercharge;

        public float overchargeDuration = 20f;
        public float overchargeFireRate = 0.5f;

        public static bool IsDead;

        public static int Score = 0;

        private void Start()
        {
                health = maxHealth;
        }

        private void Update()
        {
                CheckHealthStatus();
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

        private void CheckHealthStatus()
        {
                if (health <= 0)
                {
                        if (hasSecondChance)
                        {
                                print(health);
                                health = maxHealth;
                                print(health);
                                hasSecondChance = false;
                        }
                        else
                        {
                                IsDead = true;
                        }
                }
        }
}
