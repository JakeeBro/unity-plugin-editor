using System;
using InteractionSystem;
using InventorySystem;
using UnityEngine;

namespace Items
{
    public class ExampleItem : Item
    {
        private struct ExampleItemStatus
        {
            public string ItemStatus;
        }

        private ExampleItemStatus _status;

        public string itemStatus = "Item Status";

        private void Awake()
        {
            UpdateItemStatus();
        }

        public override void UpdateItemStatus()
        {
            _status.ItemStatus = itemStatus;
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
            Debug.Log("Item Primary Use");
        }

        public override void SecondaryUse()
        {
            Debug.Log("Item Secondary Use");
        }

        protected override void TapReaction(GameObject obj)
        {
            Debug.Log("Item Tapped");
        }

        protected override void HoldReaction(GameObject obj)
        {
            Debug.Log("Item Held");
        }
    }
}
