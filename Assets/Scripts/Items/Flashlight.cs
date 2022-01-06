using System;
using InteractionSystem;
using InventorySystem;
using PlayerCharacter;
using UnityEngine;

namespace Items
{
    public class Flashlight : Item
    {
        private struct FlashlightStatus
        {
            // Vars
            public bool LightOn;
            public bool HasBattery;
            public float ChargeLevel;
            public float MaxCharge;
            public float PowerUsage;
            public int MaxIntensity;
            // Options
            public bool RequireBattery;
        }

        private FlashlightStatus _status;
        
        [Header("Flashlight Variables")]
        public bool lightOn;
        public bool hasBattery = true;
        public float chargeLevel = 100f;
        public float maxCharge = 100f;
        public float powerUsage = .25f;
        public int maxIntensity = 100000;

        [Header("Flashlight Options")]
        public bool requireBattery = true;
        
        private Light _lightComponent;

        private void Awake()
        {
            UpdateItemStatus();
            
            if (GetComponentInChildren<Light>())
                _lightComponent = GetComponentInChildren<Light>();
        }

        private void Update()
        {
            if (lightOn)
                if (chargeLevel > 0)
                {
                    chargeLevel -= powerUsage * Time.deltaTime;
                    _lightComponent.intensity = (maxIntensity * chargeLevel / maxCharge);
                }

            if (chargeLevel == 0)
            {
                hasBattery = false;
            }
        }

        // Used to reset Object Variables to previous USED Values when switching between Items
        public override void UpdateItemStatus()
        {
            // Vars
            _status.LightOn = lightOn;
            _status.HasBattery = hasBattery;
            _status.ChargeLevel = chargeLevel;
            _status.MaxCharge = maxCharge;
            _status.PowerUsage = powerUsage;
            _status.MaxIntensity = maxIntensity;
            // Options
            _status.RequireBattery = requireBattery;
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

        public override void PrimaryUse()
        {
            if (requireBattery)
                if (hasBattery && chargeLevel > 0)
                {
                    lightOn = !lightOn;
                    _lightComponent.enabled = !_lightComponent.enabled;
                    UpdateItemStatus();
                    return;
                }

            if (chargeLevel > 0)
            {
                lightOn = !lightOn;
                _lightComponent.enabled = !_lightComponent.enabled;
                UpdateItemStatus();
            }
        }

        public override void SecondaryUse()
        {
            hasBattery = true;
            chargeLevel = maxCharge;
            _lightComponent.intensity = maxIntensity;
            UpdateItemStatus();
        }

        protected override void TapReaction(GameObject obj)
        {
            return;
        }

        protected override void HoldReaction(GameObject obj)
        {
            //if (!obj.GetComponent<Inventory>()) return;
            //if (!obj.GetComponent<PlayerManager>().linkInventory) return;
            //if (obj.GetComponent<Inventory>().AddItem(itemProperties))
                //(gameObject.transform.parent.gameObject);
            
            // Ensure the Player Manager is connected to the Inventory, and Send the Item Information if True
            if (!obj.GetComponent<PlayerManager>().linkInventory) return;
                obj.GetComponent<PlayerManager>().SendToLinkedInventory(itemProperties, gameObject);
        }
    }
}
