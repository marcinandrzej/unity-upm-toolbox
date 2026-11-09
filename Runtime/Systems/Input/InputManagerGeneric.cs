using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MA.Systems.Input
{
    public abstract class InputManagerGeneric<InputContextT> : MonoBehaviour, IInputManagerGeneric<InputContextT> where InputContextT : struct, Enum
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputContextT defaultContext;

        private InputContextT _activeContext;
        private Dictionary<InputContextT, HashSet<object>> _registeredInputRequestDict = new Dictionary<InputContextT, HashSet<object>>();

        public PlayerInput PlayerInput => playerInput;
        public InputContextT ActiveContext => _activeContext;

        protected abstract Dictionary<InputContextT, InputContextData> ContextDataDict { get; }

        public void RegisterInputContext(InputContextT context, object source)
        {
            if (source == null)
                return;

            ValidateContextRequests();

            if (!_registeredInputRequestDict.TryGetValue(context, out HashSet<object> sources))
            {
                sources = new HashSet<object>();
                _registeredInputRequestDict.Add(context, sources);
            }

            if (sources.Add(source))
            {
                InputContextT newContext = ResolveContext();

                if (!newContext.Equals(_activeContext))
                    ApplyContext(newContext);
            }
        }

        public void UnregisterInputContext(InputContextT context, object source)
        {
            ValidateContextRequests();

            if (_registeredInputRequestDict.TryGetValue(context, out HashSet<object> sources) && sources.Remove(source))
            {
                InputContextT newContext = ResolveContext();

                if (!newContext.Equals(_activeContext))
                    ApplyContext(newContext);
            }
        }

        public void ClearInputContext()
        {
            _registeredInputRequestDict.Clear();
            InputContextT newContext = ResolveContext();

            if (!newContext.Equals(_activeContext))
                ApplyContext(newContext);
        }

        private void ApplyContext(InputContextT context)
        {
            if (ContextDataDict.TryGetValue(context, out InputContextData data))
            {
                _activeContext = context;
                playerInput.SwitchCurrentActionMap(data.actionMapNameOrId);
            }
            else
                Debug.LogError($"Context: [{context}] does not have defined InputActionMapNameOrId. Active context has not changed.");
        }

        private void ValidateContextRequests()
        {
            foreach (KeyValuePair<InputContextT, HashSet<object>> pair in _registeredInputRequestDict)
            {
                if (pair.Value != null)
                    pair.Value.Remove(null);
            }
        }

        private InputContextT ResolveContext()
        {
            InputContextT highestPriorityContext = defaultContext;
            int highestPriority = int.MinValue;
            bool foundValidContext = false;

            foreach (KeyValuePair<InputContextT, HashSet<object>> pair in _registeredInputRequestDict)
            {
                if (pair.Value == null || pair.Value.Count == 0)
                    continue;

                if (!ContextDataDict.TryGetValue(pair.Key, out InputContextData data))
                    continue;

                if (!foundValidContext || data.priority > highestPriority)
                {
                    highestPriority = data.priority;
                    highestPriorityContext = pair.Key;
                    foundValidContext = true;
                }
            }

            return foundValidContext ? highestPriorityContext : defaultContext;
        }
    }
}