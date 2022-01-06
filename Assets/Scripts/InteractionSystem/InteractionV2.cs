using System.Collections;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionV2 : MonoBehaviour
    {
        private enum InputEvent
        {
            Pressed,
            Released
        }
    
        public enum InteractionAction
        {
            None,
            Tap,
            Hold
        }
    
        private enum InteractionMethod
        {
            None,
            Trace,
            // TODO Implement Overlap
        }
    
        private struct InteractionResults
        {
            public GameObject Target;
            public InteractionMethod Method;
        }

        private struct InteractionParameters
        {
            public InteractionResults Results;
            public InteractionAction Action;
        }

        [Header("Trace Settings")]
        public bool doTrace = true;
        public float traceDistance = 3f;
        public Transform traceStart;

        private InteractionResults _traceResults;
    
        [Header("Variables")]
        public GameObject traceObject;

        [Header("Interaction Settings")]
        public float holdTime = 1f;

        private bool _blockTap;
        private IEnumerator _holdTimer;

        [Header("Debug Settings")]
        public bool printTrace;
        public bool printAction;
        public bool printTimer;

        #region Ray Trace Event
        
        private Ray _rayTrace;

        public delegate void RayTraceDirection(Vector3 origin, Vector3 direction);
        public RayTraceDirection RayTraceEvent;
        void SetRayTrace(Vector3 origin, Vector3 direction)
        {
            if (RayTraceEvent != null)
                RayTraceEvent(origin, direction);

            _rayTrace = new Ray(origin, direction);
        }
        
        #endregion
        
        #region Ray Out Hit Event

        private RaycastHit _rayHit;

        public delegate void RayOutHit(RaycastHit hit);
        public RayOutHit RayOutHitEvent;
        void SetRayOutHit(RaycastHit hit)
        {
            if (RayOutHitEvent != null)
                RayOutHitEvent(hit);

            _rayHit = hit;
        }
        #endregion

        #region Interactable Validity Event
        
        private bool _validInteractable;

        public delegate void InteractableValidity(bool value, GameObject obj);
        public InteractableValidity InteractableValidityEvent;
        void SetInteractableValidity(bool value, GameObject obj)
        {
            if (InteractableValidityEvent != null)
                InteractableValidityEvent(value, obj);

            _validInteractable = value;
            traceObject = obj;
        }
        
        #endregion

        private void Update()
        {
            var results = InteractionTrace(traceDistance, traceStart);
        
            if (Input.GetKeyDown(KeyCode.E))
                Interact(InputEvent.Pressed, results, InteractionAction.Tap, InteractionAction.Hold);
        
            if (Input.GetKeyUp(KeyCode.E))
                Interact(InputEvent.Released, results, InteractionAction.Tap, InteractionAction.Hold);
        }

        private InteractionResults InteractionTrace(float distance, Transform start)
        {
            if (!doTrace) return NullifyTrace();
        
            SetRayTrace(start.position, start.forward);

            if (!Physics.Raycast(_rayTrace, out var hit, distance)) return NullifyTrace();
            SetRayOutHit(hit);
            if (_rayHit.transform.GetComponent<IInteractable>() == null) return NullifyTrace();

            if (printTrace)
                Debug.Log("TRACE: Valid Interaction Target");
            
            traceObject = _rayHit.transform.gameObject;
            SetInteractableValidity(true, traceObject);

            _traceResults.Target = traceObject;
            _traceResults.Method = InteractionMethod.Trace;

            return _traceResults;
        }

        // Resets _traceResults when there is no Valid Interaction Target
        private InteractionResults NullifyTrace()
        {
            SetInteractableValidity(false, null);

            _traceResults.Target = traceObject;
            _traceResults.Method = InteractionMethod.None;

            return _traceResults;
        }

        private void Interact(InputEvent input, InteractionResults results, InteractionAction tap, InteractionAction hold)
        {
            StopAllCoroutines();
        
            // Store Interaction Target and Method
            InteractionParameters parameters;
            parameters.Results = results;

            switch (input)
            {
                case InputEvent.Pressed:
                    // Assume Hold Input
                    parameters.Action = hold;
                    _blockTap = true;

                    _holdTimer = HoldTimer(parameters);
                    StartCoroutine(_holdTimer);
                    break;
                case InputEvent.Released:
                    if (_blockTap)
                    {
                        if (printAction)
                            Debug.Log("ACTION: Hold Interaction Cancelled");

                        parameters.Action = tap;
                        _blockTap = false;
                    
                        ExecuteInteraction(parameters);
                    }
                    else if (!_blockTap)
                        _blockTap = true;
                    break;
            }
        }

        private IEnumerator HoldTimer(InteractionParameters parameters)
        {
            if (printTimer)
                Debug.Log("TIMER: Hold Timer Reached");
        
            if (printAction)
                Debug.Log("ACTION: Hold Interaction Started");
        
            yield return new WaitForSeconds(holdTime);

            if (printTimer)
                Debug.Log("TIMER: Hold Timer Finished");

            ExecuteInteraction(parameters);
        }

        private void ExecuteInteraction(InteractionParameters parameters)
        {
            switch (parameters.Results.Method)
            {
                case InteractionMethod.Trace:
                    if (!_validInteractable || !_traceResults.Target) return;
                    _traceResults.Target.GetComponent<IInteractable>().ReceiveInteraction(gameObject, parameters.Action);
                    _blockTap = false;
                    break;
            }
        }
    }
}
