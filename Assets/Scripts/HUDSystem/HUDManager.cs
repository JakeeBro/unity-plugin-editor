using InteractionSystem;
using InventorySystem;
using PlayerCharacter;
using UnityEngine;
using TMPro;

/* USAGE
    1 - Place on Player
    2 - Link Relevant UI Objects
 */

namespace HUDSystem
{
    public class HUDManager : MonoBehaviour
    {
        [Header("HUD Settings")]
        [Tooltip("Whether or not UI Object References are required to Run the Game. Turn off for Testing Game Functionality before UI is implemented")]
        public bool requireHUD;
        public TMP_Text interactText;

        // Managers
        private PlayerManager _playerManager;
        public PlayerManager PlayerManager
        {
            set => _playerManager = value;
        }

        private void Start()
        {
            // Grab the Player Manager Component
            if (!_playerManager)
                Debug.LogError("HUDManager Missing Reference to PlayerManager");

            // If Interaction should be Linked, Subscribe the UI Update Function to the Interact Event
            if (_playerManager.linkInteraction)
                _playerManager.Interaction.InteractableValidityEvent += UpdateInteractText;
            
            // Disable the Text Object if requireHUD is set to False
            if (!requireHUD && interactText)
                interactText.gameObject.SetActive(false);
        }

        // Updates the Text Value of the Interaction Text UI. Called by an Event Subscription.
        private void UpdateInteractText(bool value, GameObject obj)
        {
            if (requireHUD)
            {
                // If the Object is of type Item
                if (obj && obj.GetComponent<Item>())
                {
                    Item item = obj.GetComponent<Item>();
                    interactText.text = item.interactableProperties.InteractMessage;
                }

                // If the Object is of type Interactable
                if (obj && obj.GetComponent<Interactable>())
                {
                    Interactable interactable = obj.GetComponent<Interactable>();
                    interactText.text = interactable.interactableProperties.InteractMessage;
                }
            
                interactText.gameObject.SetActive(value);
            }
        }
    }
}
