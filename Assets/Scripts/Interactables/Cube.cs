using System;
using InteractionSystem;
using UnityEngine;

namespace Interactables
{
    public class Cube : Interactable
    {
        public override void ReceiveInteraction(GameObject interactingGameObject, InteractionV2.InteractionAction action)
        {
            switch (action)
            {
                case InteractionV2.InteractionAction.Tap:
                    Debug.Log($"{name} Tapped by " + interactingGameObject.name);
                    break;
                case InteractionV2.InteractionAction.Hold:
                    Debug.Log($"{name} Held by " + interactingGameObject.name);
                    break;
                case InteractionV2.InteractionAction.None:
                    break;
            }
        }

        protected override void TapReaction(GameObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void HoldReaction(GameObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
