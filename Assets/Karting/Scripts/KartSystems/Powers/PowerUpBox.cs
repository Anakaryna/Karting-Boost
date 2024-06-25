using KartGame.KartSystems;
using UnityEngine;

namespace Karting.Scripts.KartSystems.Powers
{
    public class PowerUpBox : MonoBehaviour
    {
        public GameObject[] powerUpPrefabs; 

        private void OnTriggerEnter(Collider other)
        {
            ArcadeKart kart = other.GetComponentInParent<ArcadeKart>(); 
            if (kart != null)
            {
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                GameObject selectedPowerUp = powerUpPrefabs[randomIndex];
                kart.GetComponent<PowerUpManager>().StorePowerUp(selectedPowerUp);

                Destroy(gameObject); 
            }
        }
    }
}