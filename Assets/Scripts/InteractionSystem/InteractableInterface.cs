/* Defines Functions for Interactable Objects
 Usage
 - Inherit from IInteractable
 - Implement Overrides
*/

using UnityEngine;

namespace InteractionSystem
{
    public interface IInteractable
    {
        public void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction type);
    }
}
