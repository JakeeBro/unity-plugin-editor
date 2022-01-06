using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        public ItemProperties defaultItem;
        public Transform itemHolder;
        
        [Header("Hot Bar")]
        public bool enableHotBar;
        public int hotBarSize;
        public int activeItemIndex;
        public bool canSeeActiveItem;
        public GameObject activeItem;
        public ItemProperties activeItemProperties;
        public List<ItemProperties> hotBarItems = new List<ItemProperties>();
        
        [Header("Hot Bar Options")]
        public bool hotBarAutoSort;
        public bool invertScroll = true;

        [Header("Inventory")]
        public int inventorySize;
        public List<ItemProperties> inventoryItems = new List<ItemProperties>();

        private void Awake()
        {
            // Inflate Hot Bar List
            if (enableHotBar)
                for (int i = 0; i < hotBarSize; i++)
                    hotBarItems.Add(defaultItem);
            
            // Inflate Inventory List
            for (int i = 0; i < inventorySize; i++)
                inventoryItems.Add(defaultItem);
            
            UpdateActiveItem();
        }

        public bool AddItem(ItemProperties properties)
        {
            if (enableHotBar)
            {
                if (hotBarAutoSort)
                {
                    // Iterate through Hot Bar Array
                    for (var i = 0; i < hotBarSize; i++)
                    {
                        if (hotBarItems.Count < hotBarSize)
                        {
                            hotBarItems.Add(properties);
                            UpdateActiveItem();
                            Debug.Log("LIST V2 // Added to HotBar // HotBar Enabled // AutoSort Enabled");
                            return true;
                        }

                        // If there is no Empty Slot, try the Inventory
                        if (i == hotBarSize - 1)
                        {
                            for (var j = 0; j < inventorySize; j++)
                            {
                                // Place in first Empty Slot
                                if (inventoryItems.Count < inventorySize)
                                {
                                    inventoryItems.Add(properties);
                                    UpdateActiveItem();
                                    Debug.Log("LIST V2 // Added to Inventory // HotBar Enabled // AutoSort Enabled");
                                    return true;
                                }

                                // If there is no empty Slot
                                if (j == inventorySize - 1)
                                {
                                    Debug.Log("LIST V2 // No Room in Inventory // HotBar Enabled // AutoSort Enabled");
                                    return false;
                                }
                            }
                        }
                    }
                }

                // AutoSort Disabled
                hotBarItems[activeItemIndex] = properties;
                UpdateActiveItem();
                Debug.Log("LIST V2 // Added to Inventory // HotBar Enabled // AutoSort Disabled");
                return true;
            }
            for (var i = 0; i < inventorySize; i++)
            {
                // Place in first Empty Slot
                if (inventoryItems.Count < inventorySize)
                {
                    inventoryItems.Add(properties);
                    Debug.Log("LIST V2 // Added to Inventory // HotBar Disabled");
                    return true;
                }

                // If there is no empty Slot
                if (i == inventorySize - 1)
                {
                    Debug.Log("LIST V2 // No Room in Inventory");
                    return false;
                }
            }

            Debug.LogError("LIST V2 // Add Item Rejected");
            return false;
        }

        public void Place(Vector3 position, RaycastHit hit, Quaternion rotation)
        {
            if (activeItemProperties.CanPlace)
            {
                var pos = position;
                var rch = hit;
                var rot = rotation;
            
                if (rch.collider != null)
                {
                    pos = rch.point;
                    rot = Quaternion.FromToRotation(Vector3.up, rch.normal);
                }
                else
                {
                    Ray ray = new Ray(position, Vector3.down);

                    if (Physics.Raycast(ray, out var hitNull, Mathf.Infinity))
                        if (hitNull.collider != null)
                        {
                            pos = hitNull.point;
                            rot = Quaternion.FromToRotation(Vector3.up, hitNull.normal);
                        }
                }
            
                // If the HotBar is ENABLED and the User has an Item in the HotBar Selected...
                if (enableHotBar && hotBarItems[activeItemIndex] != null)
                {
                    Debug.Log("Hot Bar PlaceV2");
                    Instantiate(hotBarItems[activeItemIndex].ItemPrefab, pos, rot);
                    hotBarItems.RemoveAt(activeItemIndex);
                    hotBarItems.Insert(activeItemIndex, defaultItem);
                    UpdateActiveItem();
                }

                // If the HotBar is DISABLED and the Inventory has an Item in it...
                if (!enableHotBar && inventoryItems[0] != null)
                {
                    Debug.Log("Inventory PlaceV2");
                    Instantiate(inventoryItems[0].ItemPrefab, pos, rot);
                    inventoryItems.RemoveAt(0);
                    inventoryItems.Insert(0, defaultItem);
                }
            }
        }

        public void HotBarIndexNext()
        {
            if (invertScroll)
            {
                if (activeItemIndex - 1 < 0)
                    activeItemIndex = hotBarSize - 1;
                else
                    activeItemIndex--;

                UpdateActiveItem();
                return;
            }
            
            if (activeItemIndex + 1 >= hotBarSize)
                activeItemIndex = 0;
            else
                activeItemIndex++;
            
            UpdateActiveItem();
        }

        public void HotBarIndexPrevious()
        {
            if (invertScroll)
            {
                if (activeItemIndex + 1 >= hotBarSize)
                    activeItemIndex = 0;
                else
                    activeItemIndex++;
                
                UpdateActiveItem();
                return;
            }
            
            if (activeItemIndex - 1 < 0)
                activeItemIndex = hotBarSize - 1;
            else
                activeItemIndex--;
            
            UpdateActiveItem();
        }

        private void UpdateActiveItem()
        {
            activeItemProperties = hotBarItems[activeItemIndex];

            if (canSeeActiveItem)
            {
                Destroy(activeItem);
                activeItem = Instantiate(hotBarItems[activeItemIndex].ItemPrefab, itemHolder);
                activeItem.transform.localPosition = Vector3.zero;
                // TODO Replace Instantiation with Actual Item Movement. No Destroying and Respawning of Items. Actual Movement.
                
                if (activeItem.GetComponent<Collider>())
                    activeItem.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
