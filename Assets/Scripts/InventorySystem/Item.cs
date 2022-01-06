/* Base Class for Item Objects
 Usage
 - Create new Script
 - Inherit from Item
 - Implement Functionality
 - Place created Script on Object
*/

using InteractionSystem;
using UnityEngine;

namespace InventorySystem
{
    public enum ItemRarity
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
    public abstract class Item : Interactable, IItem
    {
        public ItemProperties itemProperties;
        
        public abstract void UpdateItemStatus(); // <- Call At the Start of the Awake Method
        public abstract override void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction action);

        public abstract void PrimaryUse();

        public abstract void SecondaryUse();
    }
}
