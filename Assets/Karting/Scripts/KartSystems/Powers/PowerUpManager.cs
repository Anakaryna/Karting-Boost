using UnityEngine;
using KartGame.KartSystems;

public class PowerUpManager : MonoBehaviour
{
    private GameObject storedPowerUp;
    private bool hasPowerUp = false;
    private ArcadeKart arcadeKart;

    void Awake()
    {
        arcadeKart = GetComponent<ArcadeKart>();
    }

    public void StorePowerUp(GameObject powerUp)
    {
        storedPowerUp = powerUp;
        hasPowerUp = true;
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
            }
        }
    }
}