using UnityEngine;

namespace InteractionSystem
{
    [CreateAssetMenu(fileName = "Interactable Properties", menuName = "Interactable/Interactable Properties")]
    public class InteractableProperties : ScriptableObject
    {
        [SerializeField] private string interactMessage;
        
        public string InteractMessage => interactMessage;
    }
}
