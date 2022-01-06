/* Defines Item Values
 Usage
 - In Asset Browser, Right Click -> Item -> Item Properties
 - Edit Property Values
 - Assign to ItemProperties Component on a class Inheriting from Item
*/

using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Item Properties", menuName = "Item/Item Properties")]
    public class ItemProperties : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        [SerializeField] private ItemRarity itemRarity;
        [SerializeField] private bool canPlace = true;
        
        [SerializeField] private GameObject itemPrefab;

        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public ItemRarity ItemRarity => itemRarity;
        public bool CanPlace => canPlace;

        public GameObject ItemPrefab => itemPrefab;
    }
}
