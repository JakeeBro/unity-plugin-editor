using InteractionSystem;
using UnityEngine;

namespace Interactables
{
    public class ExampleInteractable : Interactable
    {
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

        protected override void TapReaction(GameObject obj)
        {
            Debug.Log("Interactable Tapped");
        }

        protected override void HoldReaction(GameObject obj)
        {
            Debug.Log("Interactable Held");
        }
    }
}
