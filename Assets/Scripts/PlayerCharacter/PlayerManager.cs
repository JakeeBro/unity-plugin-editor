using System;
using HUDSystem;
using InteractionSystem;
using InventorySystem;
using UnityEngine;

/* PURPOSE
    Used as a Hub for various components to communicate with each other on the Player
 */

/* USAGE
    1 - Attach to the Object containing other components such as Interaction, Inventory, HUD, etc.
 */

namespace PlayerCharacter
{
    public class PlayerManager : MonoBehaviour
    {
        // Managers
        [Header("HUD")]
        public bool linkHUD;
        private HUDManager _hudManager;
        
        // Interaction
        [Header("Interaction")]
        public bool linkInteraction;
        private InteractionV2 _interaction;
        public InteractionV2 Interaction
        {
            get => _interaction;
            set => _interaction = value;
        }
        
        // Interaction Ray
        private Ray _look;
        private RaycastHit _lookHit;
        private float _lookDistance;

        // Inventory
        [Header("Inventory")]
        public bool linkInventory;

        [Header("Inventory Input")] 
        public KeyCode primaryUse = KeyCode.Mouse0;
        public KeyCode secondaryUse = KeyCode.R;
        public KeyCode placeItem = KeyCode.Mouse1;

        private Inventory _inventory;

        private void Awake()
        {
            if (linkHUD)
                LinkHUDManager();

            if (linkInteraction)
                LinkInteraction();
            
            if (linkInventory)
                LinkInventory();
        }

        private void Update()
        {
            // Inventory Input Actions
            if (linkInventory)
            {
                // Use Active Item
                if (Input.GetKeyDown(primaryUse))
                {
                    _inventory.activeItem.GetComponentInChildren<Item>().PrimaryUse();
                }
                
                if (Input.GetKeyDown(secondaryUse))
                {
                    _inventory.activeItem.GetComponentInChildren<Item>().SecondaryUse();
                }
                
                // Place Item
                if (Input.GetKeyDown(placeItem))
                {
                    //_inventory.Place(_look.origin + _look.direction * _lookDistance, Quaternion.identity);
                    _inventory.Place(_look.origin + _look.direction * _lookDistance, _lookHit, Quaternion.identity);
                }
                
                // Hot Bar Scrolling
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                    _inventory.HotBarIndexNext();

                if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
                    _inventory.HotBarIndexPrevious();
            }
        }

        #region Component Links
        private void LinkHUDManager()
        {
            if (GetComponent<HUDManager>() && !_hudManager)
            {
                _hudManager = GetComponent<HUDManager>();
                _hudManager.PlayerManager = this;
            }
            
            if (!GetComponent<HUDManager>() && !_hudManager)
                Debug.LogError("PlayerManager Missing Reference to HUDManager");
        }

        private void LinkInteraction()
        {
            if (GetComponent<InteractionV2>() && !_interaction)
            {
                _interaction = GetComponent<InteractionV2>();
                _lookDistance = _interaction.traceDistance;
                _interaction.RayTraceEvent += UpdateLook;
                _interaction.RayOutHitEvent += UpdateLookHit;
            }
            
            if (!GetComponent<InteractionV2>() && !_interaction)
                Debug.LogError("PlayerManager Missing Reference to Interaction");
        }

        private void UpdateLook(Vector3 origin, Vector3 direction)
        {
            _look = new Ray(origin, direction);
        }

        private void UpdateLookHit(RaycastHit hit)
        {
            _lookHit = hit;
        }

        private void LinkInventory()
        {
            if (GetComponent<Inventory>() && !_inventory)
                _inventory = GetComponent<Inventory>();
            
            if (!GetComponent<Inventory>() && !_inventory)
                Debug.LogError("PlayerManager Missing Reference to Inventory");
        }
        #endregion

        public void SendToLinkedInventory(ItemProperties properties, GameObject itemObj)
        {
            if (linkInventory && GetComponent<Inventory>())
                if (GetComponent<Inventory>().AddItem(properties))
                    Destroy(itemObj.transform.parent.gameObject);
        }
    }
}
