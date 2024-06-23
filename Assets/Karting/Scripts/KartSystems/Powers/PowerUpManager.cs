using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using KartGame.KartSystems;

public class PowerUpManager : MonoBehaviour
{
    private GameObject storedPowerUp;
    private bool hasPowerUp = false;
    private ArcadeKart arcadeKart;
    
    private Dictionary<string, GameObject> powerUpModels = new Dictionary<string, GameObject>();

    // Reference to the particle system
    public ParticleSystem speedEffectParticleSystem;

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

        // Ensure the particle system is initially disabled
        if (speedEffectParticleSystem != null)
        {
            speedEffectParticleSystem.Stop();
        }
    }

    public void StorePowerUp(GameObject powerUp)
    {
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

                // Activate the particle system
                if (speedEffectParticleSystem != null)
                {
                    speedEffectParticleSystem.Play();
                    StartCoroutine(DisableParticleSystemAfterDuration(mushroomPowerUp.duration));
                }
            }
        }
    }


    private IEnumerator DisableParticleSystemAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (speedEffectParticleSystem != null)
        {
            speedEffectParticleSystem.Stop();
        }
    }
}
