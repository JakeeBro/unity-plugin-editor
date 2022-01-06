/* Base Class for Interactable Objects
 Usage
 - Create new Script
 - Inherit from Interactable
 - Implement Functionality
 - Place created Script on Object
*/

using UnityEngine;

namespace InteractionSystem
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public InteractableProperties interactableProperties;

        public abstract void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction action);

        protected abstract void TapReaction(GameObject obj);
        protected abstract void HoldReaction(GameObject obj);
    }
}