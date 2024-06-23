using UnityEngine;

namespace KartGame.KartSystems
{
    public class MushroomPowerUp : MonoBehaviour
    {
        public float speedBoost = 30f; 
        public float duration = 5f; 

        private void OnTriggerEnter(Collider other)
        {
            PowerUpManager powerUpManager = other.GetComponent<PowerUpManager>();
            if (powerUpManager != null)
            {
                powerUpManager.StorePowerUp(gameObject);
                Destroy(gameObject); 
            }
        }
    }
}