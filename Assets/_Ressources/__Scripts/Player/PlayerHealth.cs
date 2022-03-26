using System;
using UnityEngine;

namespace Game.Player
{
    public class Health : MonoBehaviour
    {
        public int health = 100;

        private void Update()
        {
            if (health <= 0)
            {
                KillPlayer();
            }
        }

        public void DealDmg(int dmg)
        {
            if (health > 0)
            {
                health -= dmg;
            }
        }

        private static void KillPlayer()
        {
            print("you died");
        }
    }
}