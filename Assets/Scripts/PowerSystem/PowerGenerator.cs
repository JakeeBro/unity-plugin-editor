using System;
using System.Collections;
using InteractionSystem;
using UnityEngine;

namespace PowerSystem
{
    public class PowerGenerator : Interactable
    {
        [Header("Power Generation")]
        [Tooltip("This Amount of Power...")] public float genRate;
        [Tooltip("...Every [X] Seconds")] public float genInterval;

        public float storedPower;

        [Header("Power Drain")]
        [Tooltip("This Amount of Drain...")] public float drainRate = 0f;
        [Tooltip("...Every [X] Seconds")] public float drainInterval = 1f;
        
        
        [Header("Power Limit")]
        public bool limitPower;
        public float powerLimit;

        private float _storedTime;
        
        [Header("Options")]
        public bool autoUpdate;

        private void Update()
        {
            if (autoUpdate)
                UpdatePowerLevel();
        }

        private float UpdatePowerLevel()
        {
            // TODO Fix
            _storedTime = Time.time;
            if (drainRate > genRate && storedPower <= 0)
                return storedPower = 0;
            if (limitPower && genRate > drainRate && storedPower >= powerLimit)
            {
                _storedTime = 0;
                return storedPower = powerLimit;
            }
            var gain = (_storedTime / genInterval) * genRate;
            var loss = (_storedTime / drainInterval) * drainRate;
            
            return storedPower = gain - loss;
        }

        public override void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction action)
        {
            switch (action)
            {
                case InteractionV2.InteractionAction.Tap:
                    TapReaction(interactingGameObject);
                    break;
                case InteractionV2.InteractionAction.Hold:
                    HoldReaction(interactingGameObject);
                    break;
            }
        }

        protected override void TapReaction(GameObject obj)
        {
            Debug.Log(UpdatePowerLevel() + " // " + Time.time);
        }

        protected override void HoldReaction(GameObject obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
