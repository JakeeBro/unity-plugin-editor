using InteractionSystem;
using InventorySystem;
using PlayerCharacter;
using SessionSystem;
using UnityEngine;

namespace Items
{
    public class Cube3 : Item
    {
        private struct Cube3Status
        {
            public float F1;
            public float F2;
        }

        private Cube3Status _status;
        
        public float f1 = 3f;
        public float f2 = 7f;

        public override void UpdateItemStatus()
        {
            _status.F1 = f1;
            _status.F2 = f2;
        }

        public override void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction type)
        {
            switch (type)
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
            Debug.Log("Used " + itemProperties.ItemName);
        }

        public override void SecondaryUse()
        {
            throw new System.NotImplementedException();
        }

        protected override void TapReaction(GameObject obj)
        {
            //Debug.Log(itemProperties.ItemName + " Tapped by " + obj.name);
        
            //Debug.Log(GlobalFunctions.GetPlayer(0).name);

            //var player = GlobalFunctions.CastToPlayer(obj);

            var player = GlobalFunctions.CastToPlayer(obj);
        
            if (player != null)
            {
                if (player is InteractionPlayer intPlayer)
                {
                    // Do Something...
                    Debug.Log("Interacted");
                }
            }
        }

        protected override void HoldReaction(GameObject obj)
        {
            // Ensure there is an Inventory Component
            //if (!obj.GetComponent<Inventory>()) return;
            
            // Ensure the Player Manager is connected to the Inventory, and Send the Item Information if True
            if (!obj.GetComponent<PlayerManager>().linkInventory) return;
                obj.GetComponent<PlayerManager>().SendToLinkedInventory(itemProperties, gameObject);
            
            // Add the Item to the Inventory
            //if (obj.GetComponent<Inventory>().AddItem(itemProperties))
                //Destroy(gameObject.transform.parent.gameObject);
        }

        public void FloatMath()
        {
            Debug.Log(f1 * f2);
        }
    }
}
