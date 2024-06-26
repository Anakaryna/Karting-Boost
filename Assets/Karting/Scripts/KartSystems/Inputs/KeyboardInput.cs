﻿using UnityEngine;

namespace KartGame.KartSystems {
    
    public class KeyboardInput : BaseInput {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";
        public string PowerUpButtonName = "PowerUp";

        public override InputData GenerateInput() {
            InputData inputData = new InputData {
                Accelerate = Input.GetButton(AccelerateButtonName),
                Brake = Input.GetButton(BrakeButtonName),
                TurnInput = Input.GetAxis(TurnInputName),
                UsePowerUp = Input.GetButton(PowerUpButtonName)
            };
            return inputData;
        }
    }
}