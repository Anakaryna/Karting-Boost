using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karting.Scripts.KartSystems.Powers
{
    public class PowerUpRandomizer : MonoBehaviour
    {
        public List<GameObject> powerUpModels;
        public float randomizationDuration = 2.0f;
        public float switchInterval = 0.1f;

        private int currentIndex = 0;
        private bool isRandomizing = false;

        public void StartRandomization()
        {
            if (!isRandomizing)
            {
                StartCoroutine(RandomizePowerUp());
            }
        }

        private IEnumerator RandomizePowerUp()
        {
            isRandomizing = true;

            float endTime = Time.time + randomizationDuration;

            while (Time.time < endTime)
            {
                ShowNextPowerUp();
                yield return new WaitForSeconds(switchInterval);
            }

            isRandomizing = false;

            int finalIndex = Random.Range(0, powerUpModels.Count);
            ShowPowerUp(finalIndex);
        }

        private void ShowNextPowerUp()
        {
            powerUpModels[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % powerUpModels.Count;
            powerUpModels[currentIndex].SetActive(true);
        }

        private void ShowPowerUp(int index)
        {
            for (int i = 0; i < powerUpModels.Count; i++)
            {
                powerUpModels[i].SetActive(i == index);
            }
        }
    }
}