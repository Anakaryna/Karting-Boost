using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

namespace Karting.Scripts.KartSystems.Powers
{
    public class PowerUpManager : MonoBehaviour
    {
        private GameObject storedPowerUp;
        private bool hasPowerUp = false;
        private ArcadeKart arcadeKart;
    
        private Dictionary<string, GameObject> powerUpModels = new Dictionary<string, GameObject>();

        // References to particle systems
        public ParticleSystem speedEffectParticleSystem;
        public ParticleSystem jumpEffectParticleSystem;

        void Awake()
        {
            arcadeKart = GetComponent<ArcadeKart>();

            // Find and deactivate all power-up models
            GameObject powerUpModelsParent = GameObject.Find("PowerUpModels");
            foreach (Transform child in powerUpModelsParent.transform)
            {
                powerUpModels[child.name] = child.gameObject;
                child.gameObject.SetActive(false);
            }

            // Ensure the particle systems are initially disabled
            if (speedEffectParticleSystem != null)
            {
                speedEffectParticleSystem.Stop();
            }
            if (jumpEffectParticleSystem != null)
            {
                jumpEffectParticleSystem.Stop();
            }
        }

        public void StorePowerUp(GameObject powerUp)
        {
            // Deactivate the current power-up model if one is active
            if (hasPowerUp && storedPowerUp != null)
            {
                string currentPowerUpID = storedPowerUp.name;
                if (powerUpModels.ContainsKey(currentPowerUpID))
                {
                    powerUpModels[currentPowerUpID].SetActive(false);
                }
            }

            storedPowerUp = powerUp;
            hasPowerUp = true;

            // Get the power-up ID and activate the corresponding model
            string powerUpID = powerUp.name;
            if (powerUpModels.ContainsKey(powerUpID))
            {
                powerUpModels[powerUpID].SetActive(true);
            }
        }

        private void Update()
        {
            if (hasPowerUp && arcadeKart.Input.UsePowerUp)
            {
                ActivateStoredPowerUp();
            }
        }

        private void ActivateStoredPowerUp()
        {
            if (storedPowerUp != null)
            {
                string powerUpID = storedPowerUp.name;

                var mushroomPowerUp = storedPowerUp.GetComponent<MushroomPowerUp>();
                var jumpPowerUp = storedPowerUp.GetComponent<JumpPowerUp>(); 

                if (mushroomPowerUp != null)
                {
                    arcadeKart.AddPowerup(new ArcadeKart.StatPowerup
                    {
                        modifiers = new ArcadeKart.Stats
                        {
                            TopSpeed = mushroomPowerUp.speedBoost
                        },
                        PowerUpID = "Mushroom",
                        MaxTime = mushroomPowerUp.duration
                    });

                    hasPowerUp = false;
                    storedPowerUp = null;

                    // Deactivate the power-up model
                    if (powerUpModels.ContainsKey(powerUpID))
                    {
                        powerUpModels[powerUpID].SetActive(false);
                    }

                    // Activate the speed effect particle system
                    if (speedEffectParticleSystem != null)
                    {
                        speedEffectParticleSystem.Play();
                        StartCoroutine(DisableParticleSystemAfterDuration(speedEffectParticleSystem, mushroomPowerUp.duration));
                    }
                }
                else if (jumpPowerUp != null)
                {
                    StartCoroutine(HandleJumpPowerUp(jumpPowerUp.jumpForce, jumpPowerUp.duration));
                
                    hasPowerUp = false;
                    storedPowerUp = null;

                    // Deactivate the power-up model
                    if (powerUpModels.ContainsKey(powerUpID))
                    {
                        powerUpModels[powerUpID].SetActive(false);
                    }

                    // Activate the jump effect particle system
                    if (jumpEffectParticleSystem != null)
                    {
                        jumpEffectParticleSystem.Play();
                        StartCoroutine(DisableParticleSystemAfterDuration(jumpEffectParticleSystem, jumpPowerUp.duration));
                    }
                }
            }
        }

        private IEnumerator HandleJumpPowerUp(float jumpForce, float duration)
        {
            Rigidbody kartRigidbody = arcadeKart.GetComponent<Rigidbody>();
            if (kartRigidbody != null)
            {
                // Apply the jump force
                kartRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                // Wait for the duration of the jump
                yield return new WaitForSeconds(duration);
            }
        }

        private IEnumerator DisableParticleSystemAfterDuration(ParticleSystem particleSystem, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (particleSystem != null)
            {
                particleSystem.Stop();
            }
        }
    }
}
