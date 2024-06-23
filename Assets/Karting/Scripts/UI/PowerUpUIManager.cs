using KartGame.KartSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerUpUIManager : MonoBehaviour
{
    public Image PowerUpImage; // UI Image to display the power-up sprite
    public TextMeshProUGUI PowerUpText; // UI Text to display the power-up name
    private PowerUpManager powerUpManager;

    void Start()
    {
        var kart = FindObjectOfType<ArcadeKart>();
        if (kart != null)
        {
            powerUpManager = kart.GetComponent<PowerUpManager>();
        }

        if (powerUpManager == null)
        {
            Debug.LogError("PowerUpManager not found on ArcadeKart");
        }
    }

    void Update()
    {
        if (powerUpManager != null && powerUpManager.HasPowerUp)
        {
            PowerUpImage.enabled = true;
            PowerUpText.enabled = true;
            PowerUpText.text = powerUpManager.StoredPowerUp.name;

            SpriteRenderer spriteRenderer = powerUpManager.StoredPowerUp.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                PowerUpImage.sprite = spriteRenderer.sprite;
                Color color = PowerUpImage.color;
                color.a = 1f; // Make the image fully opaque
                PowerUpImage.color = color;
            }
        }
        else
        {
            Color color = PowerUpImage.color;
            color.a = 0f; // Make the image fully transparent
            PowerUpImage.color = color;
            PowerUpText.enabled = false;
        }
    }
}