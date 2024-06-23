using UnityEngine;
using KartGame.KartSystems;

public class PowerUpManager : MonoBehaviour
{
    public GameObject StoredPowerUp { get; private set; }
    public bool HasPowerUp { get; private set; } = false;
    private ArcadeKart arcadeKart;

    void Awake()
    {
        arcadeKart = GetComponent<ArcadeKart>();
    }

    public void StorePowerUp(GameObject powerUp)
    {
        StoredPowerUp = powerUp;
        HasPowerUp = true;
    }

    private void Update()
    {
        if (HasPowerUp && arcadeKart.Input.UsePowerUp)
        {
            ActivateStoredPowerUp();
        }
    }

    private void ActivateStoredPowerUp()
    {
        if (StoredPowerUp != null)
        {
            var mushroomPowerUp = StoredPowerUp.GetComponent<MushroomPowerUp>();
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

                HasPowerUp = false;
                StoredPowerUp = null;
            }
        }
    }
}